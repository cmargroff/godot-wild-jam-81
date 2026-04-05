using System;
using System.Runtime.CompilerServices;
using Godot;

public partial class PauseManager : Node, IPauseManager
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
    }

    public void Unpause()
    {
        _tree.Paused = false; //here
        Engine.TimeScale = 1;
        GamePauseChanged?.Invoke(_tree.Paused);
    }

    public void Toggle()
    {
        _tree.Paused = !_tree.Paused;
        GamePauseChanged?.Invoke(_tree.Paused);
        Engine.TimeScale = _tree.Paused ? 0 : 1;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed() && @event.IsAction("pause"))
        {
            Toggle();
        }
    }

}
