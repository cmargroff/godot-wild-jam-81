using Godot;
using JamTemplate;
using JamTemplate.Enum;
using JamTemplate.Managers;
using Microsoft.Extensions.DependencyInjection;

public partial class WaterLevel : Control
{
  private ProgressBar _progressBar;
  private TextureRect _icon;
  private StatsManager _statsManager;
  public override void _EnterTree()
  {
    _progressBar = GetNode<ProgressBar>("%ProgressBar");
    _icon = GetNode<TextureRect>("%Icon");
    _statsManager = Entry.ServiceProvider.GetRequiredService<StatsManager>();
    _statsManager.StatChanged += StatsManager_StatChanged;
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
