using System.Collections.Generic;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;

namespace ShipOfTheseus2025.Components.Game;

public partial class ItemPickUp : Node3D
{
    [Export]
    float SPEED = 5f;
    [Export]
    float VELOCITY = 5f;

    private Vector3 _globalPosition;
    private bool _hovered = false;
    private ItemDragManager _dragManager;
    private HoverPanelManager _hoverManager;
    private Area3D _area;
    public InventoryItem InventoryItem { get; set; }
    public ItemPickupState State;

    private float _time = 0;
    private float noise_scale;
    private Vector2 noise_speed;
    private float noise_strength;
    private Image noise;

    private List<Node3D> _waterSamplers = new();

    public enum ItemPickupState
    {
        Floating,
        Held,
        Dropped,
        Attached
    }
    public AudioStreamPlayer3D ItemPickupAudioPlayer { get; set; }

    public override void _EnterTree()
    {
        State = ItemPickupState.Floating;
        _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
        _dragManager.PickupAudioStreamPlayer = ItemPickupAudioPlayer;
        _hoverManager = Globals.ServiceProvider.GetRequiredService<HoverPanelManager>();
        _globalPosition = GlobalPosition;
        AddChild(InventoryItem.ItemScene);

        // create references to water noise
        var water = GetNode<MeshInstance3D>("/root/SceneManager/Game/Water");
        var material = water.Mesh.SurfaceGetMaterial(0) as ShaderMaterial;
        noise = material.GetShaderParameter("noise1").As<NoiseTexture2D>().Noise.GetSeamlessImage(512, 512, false, false, 0.1f, true);
        noise_scale = (float)material.GetShaderParameter("noise1_scale");
        noise_speed = (Vector2)material.GetShaderParameter("noise1_speed");
        noise_strength = (float)material.GetShaderParameter("noise1_strength");
        _time = (float)material.GetShaderParameter("wave_time");

        var samplerParent = InventoryItem.ItemScene.GetNode("WaterSamplers");
        if (samplerParent is null)
        {
            // use root node as sampler
            _waterSamplers = new List<Node3D> { InventoryItem.ItemScene };
        }
        else
        {
            // get all children of the sampler parent
            for (int i = 0; i < samplerParent.GetChildCount(); i++)
            {
                var child = samplerParent.GetChild(i);
                if (child is Node3D node)
                {
                    _waterSamplers.Add(node);
                }
            }
            // limit list to 2
            if (_waterSamplers.Count > 2)
            {
                _waterSamplers = _waterSamplers.GetRange(0, 2);
            }
        }
        var area = InventoryItem.ItemScene.GetNode<Area3D>("Area3D");
        _area = area;
        _area.Connect(Area3D.SignalName.MouseEntered, Callable.From(MouseEntered));
        _area.Connect(Area3D.SignalName.MouseExited, Callable.From(MouseExited));
    }

    public override void _PhysicsProcess(double delta)
    {

        if (State == ItemPickupState.Floating)
        {
            _globalPosition.X -= SPEED * (float)delta;
            GlobalPosition = _globalPosition;

            UpdateBuoyancy(delta);
        }

        if (State == ItemPickupState.Dropped)
        {
            _globalPosition.Y -= VELOCITY * (float)delta;
            //maybe
            // _position.X -= (SPEED/2) * (float)delta;
            GlobalPosition = _globalPosition;
            VELOCITY += 0.1f;

        }


        if (GlobalPosition.X <= -30.0f || GlobalPosition.Y <= -10)
        {
            QueueFree();
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed() && @event.IsAction("lmb"))
        {
            if (_hovered && State == ItemPickupState.Floating)
            {
                _dragManager.StartDragItem(this);
                State = ItemPickupState.Held;
                _area.InputRayPickable = false;
                if (InventoryItem.Name == "Seagull")
                {
                    InventoryItem.ItemScene.GetChild<Node3D>(1).Position = Vector3.Zero;
                }
                _hoverManager.ShowItem(InventoryItem, HoverType.Item);
                GD.Print("hover page");

            }


        }
        if (@event.IsPressed() && @event.IsAction("rmb"))
        {
            if (State == ItemPickupState.Held)
            {
                _dragManager.EndDragItem();
                MouseExited();
                State = ItemPickupState.Dropped;
                GD.Print(GlobalPosition);
                _globalPosition = GlobalPosition;
                _hoverManager.HidePage();

            }
        }
    }

    public void Attach()
    {
        State = ItemPickupState.Attached;
        _hoverManager.HidePage();
    }

    public void Grab()
    {
        State = ItemPickupState.Held;
        GD.Print("hover page");
        _hoverManager.ShowItem(InventoryItem, HoverType.Item);

    }

    public void Drop()
    {
        State = ItemPickupState.Dropped;
    }

    public void MouseEntered()
    {
        if (_dragManager.Dragging == false)
        {
            _hovered = (State == ItemPickupState.Floating) ? true : false;
            if (_hovered) _hoverManager.ShowItem(InventoryItem, HoverType.Item);
        }

    }
    public void MouseExited()
    {
        _hovered = false;
        if (_dragManager.Dragging == false && State == ItemPickupState.Floating) _hoverManager.HidePage();
        GD.Print("mouse exited");
    }
    private void UpdateBuoyancy(double delta)
    {
        _time += (float)delta;
        if (_waterSamplers.Count == 0) return;
        if (_waterSamplers.Count == 1)
        {
            var height = GetHeight(_waterSamplers[0].GlobalPosition);
            GlobalPosition = new Vector3(GlobalPosition.X, height + 2.25f, GlobalPosition.Z);
        }
        else
        {
            var height = GetHeight(_waterSamplers[0].GlobalPosition);
            var height2 = GetHeight(_waterSamplers[1].GlobalPosition);
            var heightdiff = height2 - height;
            Vector2 slopevect = new Vector2(1, heightdiff).Normalized();
            float newrotx = new Vector2(1, 0).AngleTo(slopevect);
            newrotx = Mathf.Clamp(newrotx, -0.35f, 0.35f);
            Rotation = new Vector3(newrotx, Rotation.Y, Rotation.Z);
        }
    }
    private float GetHeight(Vector3 position)
    {
        var uv_x = Mathf.Wrap(position.X / noise_scale + _time * noise_speed.X, 0, 1);
        var uv_y = Mathf.Wrap(position.Z / noise_scale + _time * noise_speed.Y, 0, 1);


        var pixel_pos = new Vector2I(
            Mathf.RoundToInt(uv_x * (noise.GetWidth() - 1)),
            Mathf.RoundToInt(uv_y * (noise.GetHeight() - 1))
        );
        return noise.GetPixelv(pixel_pos).R * noise_strength;
    }
}
