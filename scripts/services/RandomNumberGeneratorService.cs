using Godot;
using System;

namespace ShipOfTheseus2025.Services;

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
    public float GetFloat() => _rng.Randf();
    public float GetFloatRange(float from, float to) => _rng.RandfRange(from, to);
    public float GetFloatRange(FloatRange range) => GetFloatRange(range.Min, range.Max);
    public uint GetInt() => _rng.Randi();

    public int GetIntRange(int from, int to) => _rng.RandiRange(from, to);

  /// <summary>
  /// Copy of Super Mario 64's randomizer
  /// </summary>
  public static ushort RandomU16()
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

    public float NextInNormalDistribution(float mean, float stdDev)
    {
        Random rand = new(); //reuse this if you are generating many
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random floats
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double randNormal =
                     mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        return (float)randNormal;
    }
}