using System.Collections.Generic;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.DependencyInjection;
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
    private IWaterManager _waterManager;
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

    [FromServices]
    public void Inject(ItemDragManager itemDragManager, HoverPanelManager hoverPanelManager)
    {
        _dragManager = itemDragManager;
        _hoverManager = hoverPanelManager;
    }

    public override void _EnterTree()
    {
        State = ItemPickupState.Floating;
        _dragManager.PickupAudioStreamPlayer = ItemPickupAudioPlayer;
        _globalPosition = GlobalPosition;
        AddChild(InventoryItem.ItemScene);

        // add events of item pickup area
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

            // UpdateBuoyancy(delta);
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
}
