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
        GD.Print("paused");

    }

    public void Unpause()
    {
        _tree.Paused = false; //here
        GamePauseChanged?.Invoke(_tree.Paused);
        GD.Print("unpaused");
        

    }

    public void Toggle()
    {
        _tree.Paused = !_tree.Paused;
        GamePauseChanged?.Invoke(_tree.Paused);
        GD.Print("pause toggled");
        GD.Print(GamePauseChanged?.GetInvocationList().Length);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("pause")) //testing pause
        {
            Toggle();
        }
    }

}
