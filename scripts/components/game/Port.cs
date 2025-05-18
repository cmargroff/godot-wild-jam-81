using Godot;
using System;

public partial class Port : Node3D
{
    [Export]
    float SPEED = 3f;
    private Vector3 _position;
    public override void _EnterTree()
    {
        _position = Position;
    }
    public override void _PhysicsProcess(double delta)
    {
        _position.X -= SPEED * (float)delta;
        Position = _position;
        if (GlobalPosition.X <= -30.0f) QueueFree();
    }

}
