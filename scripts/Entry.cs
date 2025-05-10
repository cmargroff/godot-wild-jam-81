using Godot;
using JamTemplate;
using JamTemplate.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;

public partial class Entry : Node
{
    public override void _EnterTree()
    {
        var gameManager = Globals.ServiceProvider.GetRequiredService<GameManager>();
        GetTree().Root.CallDeferred("add_child", gameManager);
    }
}
