using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;

public partial class ItemPickUp : Node3D
{
    [Export]
    float SPEED = 5f;
    private Vector3 _position;
    private bool _hovered = false;
    private ItemDragManager _dragManager;
    public override void _EnterTree()
    {
        _dragManager = Globals.ServiceProvider.GetRequiredService<ItemDragManager>();
        _position = Position;
        var area = GetNode<Area3D>("Area3D");
        area.Connect(Area3D.SignalName.MouseEntered, Callable.From(MouseEntered));
        area.Connect(Area3D.SignalName.MouseExited, Callable.From(MouseExited));
    }

    public override void _PhysicsProcess(double delta)
    {
        _position.X -= SPEED * (float)delta;
        Position = _position;
        if (Position.X <= -20.0f)
        {
            QueueFree();
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed() && @event.IsAction("lmb"))
        {
            if (_hovered)
            {
                _dragManager.StartDragItem(GetNode<MeshInstance3D>("MeshInstance3D"));
                QueueFree(); // TODO: maybe tell the item spawn manager to pool this item?
            }
        }
    }

    public void MouseEntered()
    {
        GD.Print("mouseEntered");
        _hovered = true;
    }
    public void MouseExited()
    {
        GD.Print("mouseExited");
        _hovered = false;
    }
}
