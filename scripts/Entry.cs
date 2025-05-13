using Godot;
using Godot.Collections;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Resources;

namespace ShipOfTheseus2025;

public partial class Entry : Node
{
  public override void _EnterTree()
  {
    var gameManager = Globals.ServiceProvider.GetRequiredService<GameManager>();
    GetTree().Root.CallDeferred("add_child", gameManager);
  }
}