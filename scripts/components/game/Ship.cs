
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
  private Marker3D heightsampler_z1;
  private Marker3D heightsampler_z2;
  private Marker3D heightsampler_x1;
  private Marker3D heightsampler_x2;
  private MeshInstance3D _water;
  private ShaderMaterial _material;
  private float noise_scale;
  private Vector2 noise_speed;
  private float noise_strength;
  private Image noise;
  private float _time = 0;
  [Export]
  public float offset_y = -2.5f;

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
    _water = GetNode<MeshInstance3D>("../Water");
    heightsampler_z1 = GetNode<Marker3D>("%PitchSamplers/0");
    heightsampler_z2 = GetNode<Marker3D>("%PitchSamplers/1");
    heightsampler_x1 = GetNode<Marker3D>("%RollSamplers/0");
    heightsampler_x2 = GetNode<Marker3D>("%RollSamplers/1");
    _material = _water.Mesh.SurfaceGetMaterial(0) as ShaderMaterial;

    noise_scale = (float)_material.GetShaderParameter("noise1_scale");
    noise_speed = (Vector2)_material.GetShaderParameter("noise1_speed");
    noise_strength = (float)_material.GetShaderParameter("noise1_strength");
    noise = _material.GetShaderParameter("noise1").As<NoiseTexture2D>().Noise.GetSeamlessImage(512, 512, false, false, 0.1f, true);

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
    UpdateBuoyancy(delta);
  }

  public void UpdateBuoyancy(double delta)
  {
    _time += (float)delta;

    //rotation z
    float height_z1 = GetHeight(heightsampler_z1.GlobalPosition);
    float height_z2 = GetHeight(heightsampler_z2.GlobalPosition);
    float heightdiff_z = height_z2 - height_z1;
    Vector2 slopevect_z = new Vector2(1, heightdiff_z).Normalized();

    var targetHeight = (height_z1 + height_z2) / 2;
    var heightDelta = (targetHeight - GlobalPosition.Y - offset_y) * .1f;
    GlobalPosition = new Vector3(
      GlobalPosition.X,
      GlobalPosition.Y + heightDelta,
      GlobalPosition.Z
    );

    //rotation x
    float height_x1 = GetHeight(heightsampler_x1.GlobalPosition);
    float height_x2 = GetHeight(heightsampler_x2.GlobalPosition);
    float heightdiff_x = height_x2 - height_x1;
    Vector2 slopevect_x = new Vector2(1, heightdiff_x).Normalized();

    var targetXRot = Mathf.Clamp(Mathf.RadToDeg(slopevect_x.Angle()), -10, 10);
    var targetZRot = Mathf.Clamp(Mathf.RadToDeg(slopevect_z.Angle()), -5, 5);

    var rotXD = (targetXRot - RotationDegrees.X) * .005f;
    var rotZD = (targetZRot - RotationDegrees.Z) * .005f;

    RotationDegrees = new Vector3(
      RotationDegrees.X + rotXD,
      RotationDegrees.Y,
      RotationDegrees.Z + rotZD
    );
    // float newrotz = Vector2.Up.AngleTo(slopevect_z);
    // float newrotx = Vector2.Up.AngleTo(slopevect_x);

    // var rotXD = (newrotx - RotationDegrees.X) * .1f;
    // var rotZD = (newrotz - RotationDegrees.Z) * .1f;


    // Rotation = new Vector3(
    //   Rotation.X + rotXD,
    //   0,
    //   Rotation.Z + rotZD
    // );

  }
  private float GetHeight(Vector3 position)
  {
    var uv_x = Mathf.Wrap(position.X / noise_scale + _time * noise_speed.X, 0, 1);
    var uv_y = Mathf.Wrap(position.Z / noise_scale + _time * noise_speed.Y, 0, 1);
    var pixel_pos = new Vector2I(Mathf.RoundToInt(uv_x * (noise.GetWidth() - 1)), Mathf.RoundToInt(uv_y * (noise.GetHeight() - 1)));
    return noise.GetPixelv(pixel_pos).R * noise_strength;
  }
}
