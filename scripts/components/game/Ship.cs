using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Util;

public partial class Ship : Node3D
{
  [Export]
  public float yOffset;
  [Export]
  public float BoatSpeed = 1;
  private MeshInstance3D _model;
  private float _noiseStrength;
  private float _time;
  private Vector2 _boatPos;
  private Marker3D _heightSamplerY1;
  private Marker3D _heightSamplerY2;
  private Marker3D _heightSamplerX1;
  private Marker3D _heightSamplerX2;
  private Node3D waterparent;
  private EnvironmentManager _environmentManager;
  public override void _EnterTree()
  {
    _environmentManager = Globals.ServiceProvider.GetRequiredService<EnvironmentManager>();
    _model = GetNode<MeshInstance3D>("Model");
    _heightSamplerY1 = GetNode<Marker3D>("HeightSamplerY1");
    _heightSamplerY2 = GetNode<Marker3D>("HeightSamplerY2");
    _heightSamplerX1 = GetNode<Marker3D>("HeightSamplerX1");
    _heightSamplerX2 = GetNode<Marker3D>("HeightSamplerX2");
  }

  public override void _Process(double delta)
  {
    // // BUOYANCY
    var pitchSample = GetSampleForMarkers(_heightSamplerY1, _heightSamplerY2, 15);
    var rollSample = GetSampleForMarkers(_heightSamplerX1, _heightSamplerX2, 20);

    // // TODO: update this with a circular fn
    Rotation = new Vector3(rollSample.Rotation, Rotation.Y, pitchSample.Rotation);

    // // position
    var newPosY = (pitchSample.Height1 + pitchSample.Height2) / 2 + yOffset;

    // // TODO: update this with a sine fn
    Position = new Vector3(Position.X, newPosY, Position.Z);

    _time += (float)delta;
    // // TODO: is this necessary
    _boatPos = new Vector2(BoatSpeed * _time, 0);
  }

  private NoiseSample GetSampleForMarkers(Node3D a, Node3D b, float angleBasis)
  {
    var (slopevect, h1, h2) = GetSlopeForPositions(
      a.GlobalPosition, b.GlobalPosition
    );
    var newrotx = Vector2.Right.AngleTo(slopevect);

    return new NoiseSample
    {
      Rotation = Mathf.Clamp(newrotx, Mathf.DegToRad(-angleBasis), Mathf.DegToRad(angleBasis)),
      Height1 = h1,
      Height2 = h2,
      // we exported this because some other code needs to use this and we should only sample the heights once each time
    };
  }

  private (Vector2, float, float) GetSlopeForPositions(Vector3 a, Vector3 b)
  {
    var height1 = GetHeightForPosition(a);
    var height2 = GetHeightForPosition(b);
    var heightdiff = height2 - height1;
    return (new Vector2(1, heightdiff).Normalized(), height1, height2);
  }

  private float GetHeightForPosition(Vector3 worldPos)
  {
    GD.Print((worldPos.X, worldPos.Z));
    var pos = new Vector2(
      Mathf.Wrap(
        worldPos.X / _environmentManager.WaterNoiseParams.Scale
          + _time * _environmentManager.WaterNoiseParams.Speed.X
          + _boatPos.X
        ,
        0,
        1
      ),
      Mathf.Wrap(
        worldPos.Z / _environmentManager.WaterNoiseParams.Scale
          + _time * _environmentManager.WaterNoiseParams.Speed.Y
          + _boatPos.Y
        ,
        0,
        1
      )
    ) * _environmentManager.WaterNoise.GetSize();
    return _environmentManager.WaterNoise.GetPixelv(pos.ToVector2I()).R
      * _environmentManager.WaterNoiseParams.Strength
      + yOffset;
  }

  private struct NoiseSample
  {
    public float Rotation;
    public float Height1;
    public float Height2;
  }
}
