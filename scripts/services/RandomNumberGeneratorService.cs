using Godot;

namespace JamTemplate.Services;

public partial class RandomNumberGeneratorService
{
  static ushort gRandomSeed16;
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
  public ushort RandomU16()
  {
    ushort temp1, temp2;

    if (gRandomSeed16 == 22026)
    {
      gRandomSeed16 = 0;
    }

    temp1 = (ushort)((gRandomSeed16 & 0x00FF) << 8);
    temp1 = (ushort)(temp1 ^ gRandomSeed16);

    gRandomSeed16 = (ushort)(((temp1 & 0x00FF) << 8) + ((temp1 & 0xFF00) >> 8));

    temp1 = (ushort)(((temp1 & 0x00FF) << 1) ^ gRandomSeed16);
    temp2 = (ushort)((temp1 >> 1) ^ 0xFF80);

    if ((temp1 & 1) == 0)
    {
      if (temp2 == 43605)
      {
        gRandomSeed16 = 0;
      }
      else
      {
        gRandomSeed16 = (ushort)(temp2 ^ 0x1FF4);
      }
    }
    else
    {
      gRandomSeed16 = (ushort)(temp2 ^ 0x8180);
    }

    return gRandomSeed16;
  }
}