using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class Speed : Control
{
  private const float MetersPerSecondToKnots = 1.94384f;
  private const string Suffix = "kts";
  private ObservableStat _speed;
  private Label _speedLabel;
  [FromServices]
  public void Inject(IStatsManager statsManager)
  {
    _speed = statsManager[Stat.Speed];
  }
  public override void _Ready()
  {
    _speedLabel = GetNode<Label>("%SpeedLabel");
    _speed.OnChanged += Speed_OnChanged;
    Speed_OnChanged(_speed.Value);
  }
  public void Speed_OnChanged(float val)
  {
    _speedLabel.Text = $"{val * MetersPerSecondToKnots:0.0} {Suffix}";
  }
  public override void _ExitTree()
  {
    _speed.OnChanged -= Speed_OnChanged;
  }
}
