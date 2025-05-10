using Godot;

public partial class WaterLevel : Control
{
  private ProgressBar _progressBar;
  private TextureRect _icon;
  // private StatManager _statManager;
  public override void _EnterTree()
  {
    _progressBar = GetNode<ProgressBar>("%ProgressBar");
    _icon = GetNode<TextureRect>("%Icon");
    // _statManager = Entry.ServiceProvider.GetRequiredService<StatManager>();
    // _statManager.StatChanged += StatManager_StatChanged;
  }
  public override void _ExitTree()
  {
    // _statManager.StatChanged -= StatManager_StatChanged;
  }
  public void StatManager_StatChanged(object stat)
  {
    // if (stat.Type = Stat.WaterLevel)
    // {
    //   _progressBar.Value = stat.Amount;
    // }
  }
}
