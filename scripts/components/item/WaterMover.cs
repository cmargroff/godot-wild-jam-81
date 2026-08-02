using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

public partial class ItemMover : Node, IItemMover
{
  private float _buoyancy = 9.8f * .6f;
  private float _speed;
  private float _speedOffset;
  private ObservableStat _speedStat;
  private float _gravity;
  private float _waterLevel = 0.0f;
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
    _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").As<float>();
    _item = GetParent<Item>();
    _waterLevel = _item.GlobalPosition.Y;
    _item.ItemMover = this;
    _speedOffset = _item.InventoryItem.SpeedOffset;
    _speed = _speedStat + _speedOffset;
    GD.Print("SpeedStat: " + _speedStat);
    GD.Print("SpedOffset: " + _item.InventoryItem.SpeedOffset);
    GD.Print("Speed: " + _speed);
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
      _item.GlobalPosition.Y - (_item.GlobalPosition.Y > _waterLevel ? _gravity : 0) * (float)delta,
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
    _speed = newValue + _speedOffset;
  }
}
