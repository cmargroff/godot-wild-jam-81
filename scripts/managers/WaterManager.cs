using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;

public partial class WaterManager : MeshInstance3D, IWaterManager
{
  private EnvironmentManager _environmentManager;
  private IStatsManager _statsManager;
  private ShaderMaterial _material;
  private int seed;
  private bool isRunning = false;
  private float _waveTime = 0;
  [FromServices]
  public void Inject(EnvironmentManager environmentManager, IStatsManager statsManager)
  {
    _environmentManager = environmentManager;
    _statsManager = statsManager;
    _statsManager.StatChanged += StatsManager_StatChanged;
  }

  public void SetSeed(int seed)
  {
    this.seed = seed;
    // var tex = _material.GetShaderParameter("noise1").As<NoiseTexture2D>();
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
    var tex = _material.GetShaderParameter("noise1").As<NoiseTexture2D>();
    tex.Changed += () => { SetEnvironmentNoise(tex); };
  }
  private void SetEnvironmentNoise(NoiseTexture2D tex)
  {
    var img = tex.GetImage();
    // _environmentManager.WaterNoise = img;
  }
  public override void _Process(double delta)
  {
    _waveTime += (float)delta;
    _material.SetShaderParameter("wave_time", _waveTime / 1000);
  }
}