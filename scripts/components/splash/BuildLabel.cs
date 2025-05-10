using Godot;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Util;

public partial class BuildLabel : Label
{
  [FromServices]
  public void Inject(ConfigManager config)
  {
    Text = $"GWJ #{config.GetValue("game", "GWJ_NUMBER")}";
  }
}
