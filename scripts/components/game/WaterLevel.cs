using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;

public partial class WaterLevel : Control
{
  private ProgressBar _progressBar;
  // private TextureRect _icon;
  private StatsManager _statsManager;
  public override void _EnterTree()
  {
    _progressBar = GetNode<ProgressBar>("%ProgressBar");
    // _icon = GetNode<TextureRect>("%Icon");
    _statsManager = Globals.Instance.ServiceProvider.GetRequiredService<StatsManager>();
    _statsManager.StatChanged += StatsManager_StatChanged;

    _progressBar.Value = _statsManager.GetStats(Stat.WaterLevel);
    GD.Print(_progressBar.Value);
    GD.Print(_statsManager.GetStats(Stat.WaterLevel));
  }
  public override void _ExitTree()
  {
    _statsManager.StatChanged -= StatsManager_StatChanged;
  }
  public void StatsManager_StatChanged(Stat stat, float amount)
  {
    if (stat == Stat.WaterLevel)
    {
      _progressBar.Value = amount;
    }
  }
}
