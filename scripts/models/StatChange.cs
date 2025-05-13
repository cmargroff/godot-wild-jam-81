using ShipOfTheseus2025.Enum;

namespace ShipOfTheseus2025.Models;

public struct StatChange
{
  public StatChangeMode Mode;
  public Stat Stat;
  public float Amount;
}