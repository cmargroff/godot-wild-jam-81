using Godot;

public interface IEnvironmentManager
{
  Image WaterNoise { get; }
  void SetNoise(Image img);
  float GetHeightForPosition(Vector3 position);
  void ChangeWeather() { }
  void ChangeTime() { }
}