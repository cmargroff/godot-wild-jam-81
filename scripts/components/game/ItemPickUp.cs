using Godot;

public partial class ItemPickUp : Node3D
{
    [Export]
    float SPEED = 5f;
    private Vector3 _position;
    private bool _hovered = false;

    public override void _Ready()
    {
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
