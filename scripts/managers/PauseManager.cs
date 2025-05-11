using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class PauseManager : Node
{
    public event Action<bool> GamePauseChanged;
    


    private SceneTree _tree;
    public override void _Ready()
    {
        _tree = GetTree();
        ProcessMode = Node.ProcessModeEnum.Always;
        Name = "PauseManager";
    }
    

    public void Pause()
    {
        _tree.Paused = true;
        GamePauseChanged?.Invoke(_tree.Paused);
        Engine.TimeScale = 0;
        GD.Print("paused");

    }

    public void Unpause()
    {
        _tree.Paused = false; //here
        Engine.TimeScale = 1;
        GamePauseChanged?.Invoke(_tree.Paused);

        GD.Print("unpaused");
        

    }

    public void Toggle()
    {
        _tree.Paused = !_tree.Paused;
        GamePauseChanged?.Invoke(_tree.Paused);
        Engine.TimeScale = _tree.Paused ? 0 : 1;
        GD.Print("pause toggled");
        GD.Print(GamePauseChanged?.GetInvocationList().Length);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed() && @event.IsAction("pause"))
        {
            Toggle();
        }
    }

    // public override void _PhysicsProcess(double delta)
    // {
    //     if (Input.IsActionJustPressed("pause")) //testing pause
    //     {
    //         Toggle();
    //     }
    // }

}
