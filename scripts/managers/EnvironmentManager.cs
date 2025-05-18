
using System;
using Godot;
using ShipOfTheseus2025.Models;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Managers;

public partial class EnvironmentManager : Node
{
  public event Action<Noise> WaterHeightChanged;
  public Image WaterNoise;
  public event Action<NoiseParams> WaterParamsChanged;
  public NoiseParams WaterNoiseParams;
  private float _time = 0;
  private StatsManager _statsManager;
  public EnvironmentManager()
  {
    // var texture = new NoiseTexture2D();
    // texture.Width = 512;
    // texture.Height = 512;
    // texture.Seamless = true;
    // texture.Changed += () => UpdateTexture(texture);
    // texture.Noise = new FastNoiseLite( );
    // texture.Noise.fra
    WaterNoiseParams = new NoiseParams
    {
      Scale = 200,
      Speed = Vector2.One * 0.01f,
      Strength = 10
    };
  }
  [FromServices]
  public void Inject(StatsManager statsManager)
  {
    _statsManager = statsManager;
  }
  // private void UpdateTexture(NoiseTexture2D texture)
  // {
  //   WaterNoise = texture.GetImage();
  // }
  public struct NoiseParams
  {
    public Vector2 Speed;
    public float Scale;
    public float Strength;
  }

  public void SetNoise(Image img)
  {
    WaterNoise = img;
  }

  // TODO: use this globally instead of implemented per object type
  public float GetHeightForPosition(Vector3 position)
  {
    // var scale = 200f;
    // var offset = scale / 2;
    // var coord = new Vector2(
    //   position.X, position.Z
    // );
    // GD.Print("Input  ", coord);
    // coord.X += offset;
    // coord.Y += scale / 2;
    // var UV = (coord / scale).Wrap(0, 1);
    // GD.Print("Output ", UV * 512f);


    var uv_x = Mathf.Wrap(position.X / WaterNoiseParams.Scale + _time * WaterNoiseParams.Speed.X /* + boatpos.x */, 0, 1);
    var uv_y = Mathf.Wrap(position.Z / WaterNoiseParams.Scale + _time * WaterNoiseParams.Speed.Y /* + boatpos.y */, 0, 1);

    var pixel_pos = new Vector2I(
      Mathf.RoundToInt(uv_x * WaterNoise.GetWidth()), Mathf.RoundToInt(uv_y * WaterNoise.GetHeight())
    );
    return WaterNoise.GetPixelv(pixel_pos).R * WaterNoiseParams.Strength;

    // return WaterNoise.GetPixelv(pixelPos.ToVector2I()).R * WaterNoiseParams.Strength;
  }
  public override void _PhysicsProcess(double delta)
  {
    // advance global time
    _time += (float)delta;
    // GD.Print("noise time ", _time);
    _statsManager.ChangeStat(new StatChange
    {
      Stat = Enum.Stat.WaterNoiseTime,
      Mode = Enum.StatChangeMode.Absolute,
      Amount = _time
    });
  }

  public void ChangeWeather() { }
  public void ChangeTime() { }
}