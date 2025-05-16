using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;

namespace ShipOfTheseus2025.Components.Game;

public partial class Water : MeshInstance3D
{
  private EnvironmentManager _environmentManager;
  private StatsManager _statsManager;
  private ShaderMaterial _material;
  public override void _EnterTree()
  {
    _environmentManager = Globals.ServiceProvider.GetRequiredService<EnvironmentManager>();
    _statsManager = Globals.ServiceProvider.GetRequiredService<StatsManager>();
    _statsManager.StatChanged += StatsManager_StatChanged;
  }

  private void StatsManager_StatChanged(Stat stat, float val)
  {
    if (stat == Stat.Speed)
      _material.SetShaderParameter("boat_speed", val);
    else if (stat == Stat.WaterNoiseTime)
      _material.SetShaderParameter("wave_time", val / 1000);
  }

  public override void _Ready()
  {
    _material = Mesh.SurfaceGetMaterial(0) as ShaderMaterial;
    _environmentManager.WaterNoise = _material.GetShaderParameter("noise1").As<Image>();
  }
}