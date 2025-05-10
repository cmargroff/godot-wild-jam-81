using Godot;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;
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
