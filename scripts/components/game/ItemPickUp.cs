using Godot;
using System;

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
        GlobalPosition =_position;
        GD.Print(GlobalPosition.X);
        if (GlobalPosition.X <= -30.0f) 
        {
            QueueFree();
        }
    }

}
