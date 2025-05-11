using Godot;

public partial class ItemPickUp : Node3D
{
    [Export]
    int SPEED = 5;
    private Vector3 _position;
    public override void _Ready()
    {
        _position = GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        _position.X -= SPEED * (float)delta;
        GlobalPosition = _position;
        if (GlobalPosition.X <= -20.0f)
        {
            QueueFree();
        }
    }

}
