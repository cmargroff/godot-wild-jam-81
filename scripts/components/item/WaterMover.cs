using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class WaterMover : Node, IWaterMover
{
  private float _speed;
  private ObservableStat _speedStat;
  private Item _item;
  [FromServices]
  public void Inject(IStatsManager statsManager)
  {
    _speedStat = statsManager[Stat.Speed];
    _speedStat.OnChanged += OnSpeedChanged;
    OnSpeedChanged(_speedStat.Value);
  }
  // only want start when this node is added to the item
  public override void _EnterTree()
  {
    _item = GetParent<Item>();
    _item.WaterMover = this;
  }
  public void Start()
  {
    ProcessMode = ProcessModeEnum.Pausable;
  }
  public void Stop()
  {
    ProcessMode = ProcessModeEnum.Disabled;
  }
  public override void _Process(double delta)
  {
    _item.GlobalPosition = new Vector3(
      _item.GlobalPosition.X - _speed * (float)delta,
      _item.GlobalPosition.Y,
      _item.GlobalPosition.Z
    );
    if (_item.GlobalPosition.X <= -30.0f || _item.GlobalPosition.Y <= -10)
    {
      _item.QueueFree();
    }
  }
  public override void _ExitTree()
  {
    if (_speedStat != null)
    {
      _speedStat.OnChanged -= OnSpeedChanged;
    }
  }
  public void OnSpeedChanged(float newValue)
  {
    _speed = newValue;
  }
}
