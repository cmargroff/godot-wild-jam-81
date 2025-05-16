
using System;
using Godot;

namespace ShipOfTheseus2025.Managers;

public class EnvironmentManager
{
  public event Action<Noise> WaterHeightChanged;
  public Image WaterNoise;
  public event Action<NoiseParams> WaterParamsChanged;
  public NoiseParams WaterNoiseParams;

  public struct NoiseParams
  {
    public Vector2 Speed;
    public float Scale;
    public float Strength;
  }

  // TODO: use this globally instead of implemented per object type
  private float GetHeightForPosition(Vector3 position, float time = 0)
  {
    var pixelPos = new Vector2I(
      (int)((
        position.X / WaterNoiseParams.Scale
          + time * WaterNoiseParams.Speed.X
      ) % 1 * WaterNoise.GetWidth()),
      (int)((
        position.Z / WaterNoiseParams.Scale
          + time * WaterNoiseParams.Speed.Y
      ) % 1 * WaterNoise.GetHeight())
    );

    return WaterNoise.GetPixelv(pixelPos).R * WaterNoiseParams.Strength;
  }
  public void ChangeWeather() { }
  public void ChangeTime() { }
}