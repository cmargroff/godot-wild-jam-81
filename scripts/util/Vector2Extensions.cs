using Godot;

namespace ShipOfTheseus2025.Util;

public static class Vector2Extensions
{
  public static Vector2I ToVector2I(this Vector2 vec)
  {
    return new Vector2I(Mathf.RoundToInt(vec.X), Mathf.RoundToInt(vec.Y));
  }
}