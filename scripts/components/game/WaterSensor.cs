

using System;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;

public partial class WaterSensor : Marker3D
{
#if DEBUG
  private MeshInstance3D _mesh;
  private float _time;
  private EnvironmentManager _envManager;
  private float _targetHeight = 0;
  public override void _EnterTree()
  {
    _mesh = new MeshInstance3D();
    _mesh.Mesh = new BoxMesh();
    var mat = new StandardMaterial3D();
    mat.AlbedoColor = new Color(1, 1, 0, 1);
    _mesh.Mesh.SurfaceSetMaterial(0, mat);
    _mesh.Scale = Vector3.One * 2f;
    AddChild(_mesh);
    _envManager = Globals.ServiceProvider.GetRequiredService<EnvironmentManager>();
    _envManager.GetHeightForPosition(GlobalPosition);
  }
  public override void _Process(double delta)
  {
    _time += (float)delta;
    var height = _envManager.GetHeightForPosition(Position, _time);

    _targetHeight = height;
    var d = (_targetHeight - _mesh.Position.Y) * .1f;
    _mesh.Position = Vector3.Up * (_mesh.Position.Y + d);
  }
#endif
}