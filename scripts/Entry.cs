using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Managers;

namespace ShipOfTheseus2025;

public partial class Entry : Node
{
  public override void _EnterTree()
  {
    var gameManager = Globals.ServiceProvider.GetRequiredService<GameManager>();
    GetTree().Root.CallDeferred("add_child", gameManager);
  }
}