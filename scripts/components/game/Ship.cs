
using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Util;

public partial class Ship : Node3D
{

  private RandomNumberGeneratorService _rng;
  private AnimationPlayer _animationPlayer;
  private MeshInstance3D _model;
  private Node3D _damagePoints;
  private List<DamagePoint> _pointsList = new();
  private GameEventManager _gameEventManager;
  private StatsManager _statsManager;
  private float _leakRate;
  [FromServices]
  public void Inject(RandomNumberGeneratorService rng, GameEventManager gameEventManager, StatsManager statsManager)
  {
    _rng = rng;
    _gameEventManager = gameEventManager;
    _statsManager = statsManager;
  }
  public override void _EnterTree()
  {
    // _model = GetNode<MeshInstance3D>("Model");
    // _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    // _animationPlayer.CurrentAnimation = "rocking";
    _damagePoints = GetNode<Node3D>("%Damage points");
    GetDamagePoints();
    _gameEventManager.EnvironmentEvent += Break;
  }
  public override void _Ready()
  {

  }


  private void GetDamagePoints()
  {
    var childrenCount = _damagePoints.GetChildCount();
    if (childrenCount > 0)
    {
      for (int i = 0; i < childrenCount; i++)
      {
        var point = _damagePoints.GetChild<DamagePoint>(i);
        point.LeakingChanged += DamagePoint_LeakingChanged;
        _pointsList.Add(point);
      }
    }
  }

  private void DamagePoint_LeakingChanged()
  {
    UpdateLeaking();
  }


  private void UpdateLeaking()
  {
    var leakRate = 0;
    foreach (var point in _pointsList)
    {
      leakRate += point.Leaking ? 1 : 0;
    }
    _leakRate = leakRate;
  }
  public void Break()
  {
    var snapPoint = _pointsList[_rng.GetIntRange(0, _pointsList.Count - 1)];
    snapPoint.Enable();
    UpdateLeaking();
  }
  public override void _PhysicsProcess(double delta)
  {
    _statsManager.ChangeStat(new()
    {
      Stat = ShipOfTheseus2025.Enum.Stat.WaterLevel,
      Mode = StatChangeMode.Relative,
      Amount = _leakRate * (float)delta
    });
  }

}
