using Godot;

namespace ShipOfTheseus2025.Resources;

[GlobalClass]
public partial class SplashWildcard : Resource
{
  [Export]
  public bool Enabled = false;
  [Export]
  public string Title = "";
  [Export]
  public string Description = "";
  [Export]
  public Texture2D Icon;
}
