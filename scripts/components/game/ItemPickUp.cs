using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Managers;

namespace ShipOfTheseus2025.Components.Game;
public partial class ItemPickUp : Node3D
{
    [Export]
    float SPEED = 5f;
    [Export]
    float VELOCITY = 2f;

    private Vector3 _position;
    private bool _hovered = false;
    private ItemDragManager _dragManager;
    private HoverPanelManager _hoverManager;
    private Area3D _area;
    public InventoryItem InventoryItem { get; set; }
    public ItemPickupState State;

    public enum ItemPickupState
    {
        Floating,
        Held,
        Dropped,
        Attached
    }

    public override void _EnterTree()
    {
        State = ItemPickupState.Floating;
        _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
        _hoverManager = Globals.ServiceProvider.GetRequiredService<HoverPanelManager>();
        _position = Position;
        var area = GetNode<Area3D>("Area3D");
        _area = area;
        area.Connect(Area3D.SignalName.MouseEntered, Callable.From(MouseEntered));
        area.Connect(Area3D.SignalName.MouseExited, Callable.From(MouseExited));
    }

    public override void _PhysicsProcess(double delta)
    {

        if (State == ItemPickupState.Floating)
        {
            _position.X -= SPEED * (float)delta;
            Position = _position;
        }

        if (State == ItemPickupState.Dropped)
        {
            _position.Y -= VELOCITY * (float)delta;
            Position = _position;
            VELOCITY += 0.1f;

        }


        if (GlobalPosition.X <= -20.0f || GlobalPosition.Y <= -5)
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
            }
            // if (State == ItemPickupState.Held && _dragManager.CanSnap())
            // {
            //     _dragManager.Snap();
            //     State = ItemPickupState.Snapped;

            // }

        }
        if (@event.IsPressed() && @event.IsAction("rmb"))
        {
            if (State == ItemPickupState.Held)
            {
                _dragManager.EndDragItem();
                MouseExited();
                State = ItemPickupState.Dropped;
                GD.Print(GlobalPosition);
                _position = GlobalPosition;

            }
        }
    }

    public void Attach()
    {
        State = ItemPickupState.Attached;
    }

    public void MouseEntered()
    {
        GD.Print("mouseEntered");
        _hovered = (State == ItemPickupState.Floating) ? true : false;
        _hoverManager.ShowItem(InventoryItem);
    }
    public void MouseExited()
    {
        GD.Print("mouseExited");
        _hovered = false;
        _hoverManager.HidePage();
    }
}
