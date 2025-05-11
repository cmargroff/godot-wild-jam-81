using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Stores;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025;

public partial class Entry : Node
{
  public override void _EnterTree()
  {
    var gameManager = Globals.ServiceProvider.GetRequiredService<GameManager>();
    GetTree().Root.CallDeferred("add_child", gameManager);
  }
}