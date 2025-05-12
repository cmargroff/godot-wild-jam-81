using Godot;
using System;

public partial class PauseManager : Node
{
    private SceneTree _tree;
    public override void _EnterTree()
    {
        _tree = GetTree();
        ProcessMode = Node.ProcessModeEnum.Always;
        
    }
    

    public void Pause()
    {
        _tree.Paused = true;
    }

    public void Unpause()
    {
        _tree.Paused = false;
    }

    public void Toggle()
    {
        _tree.Paused = !GetTree().Paused;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("pause")) //testing pause
        {
            Toggle();
        }
    }

}
