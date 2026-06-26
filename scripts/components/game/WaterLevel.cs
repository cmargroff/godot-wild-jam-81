using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class WaterLevel : Control
{
  private ProgressBar _progressBar;
  private IStatsManager _statsManager;
  [FromServices]
  public void Inject(IStatsManager statsManager)
  {
    _statsManager = statsManager;
  }
  public override void _EnterTree()
  {
    _progressBar = GetNode<ProgressBar>("%ProgressBar");

    var waterLevel = _statsManager[Stat.WaterLevel];
    _progressBar.Value = waterLevel;
    waterLevel.OnChanged += WaterLevel_OnChanged;
  }
  public override void _ExitTree()
  {
    var waterLevel = _statsManager[Stat.WaterLevel];
    waterLevel.OnChanged -= WaterLevel_OnChanged;
  }
  private void WaterLevel_OnChanged(float val)
  {
    _progressBar.Value = val;
  }
}
