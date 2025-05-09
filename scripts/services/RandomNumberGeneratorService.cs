using Godot;

namespace JamTemplate.Services;

public partial class RandomNumberGeneratorService
{
  private RandomNumberGenerator _rng;
  public RandomNumberGeneratorService()
  {
    _rng = new();
  }
  public void SetSeed(string seed)
  {
    _rng.Seed = (ulong)seed.GetHashCode();
    _rng.State = 0;
  }
  public float GetFloat()
  {
    return _rng.Randf();
  }
  public uint GetInt()
  {
    return _rng.Randi();
  }
}