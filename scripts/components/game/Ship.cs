using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Util;

public partial class Ship : Node3D
{

  private RandomNumberGeneratorService _rng;
  private AnimationPlayer _animationPlayer;
  private MeshInstance3D _model;
  private Node3D _damagePoints;
  private List<DamagePoint> _pointsList = new();
  [FromServices]
  public void Inject(RandomNumberGeneratorService rng)
  {
    _rng = rng;
  }
  public override void _EnterTree()
  {
    // _model = GetNode<MeshInstance3D>("Model");
    // _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
    // _animationPlayer.CurrentAnimation = "rocking";
    _damagePoints = GetNode<Node3D>("%Damage points");
    GetDamagePoints();
    
  }
  public override void _Ready()
  {
    Break();
  }


  private void GetDamagePoints()
  {
    var childrenCount = _damagePoints.GetChildCount();
    if (childrenCount > 0)
    {
      for (int i = 0; i < childrenCount; i++)
      {
        _pointsList.Add(_damagePoints.GetChild<DamagePoint>(i));
      }
    }
  }

  public void Break()
  {
    var snapPoint = _pointsList[_rng.GetIntRange(0, _pointsList.Count - 1)];
    snapPoint.Enable();
  }
}
