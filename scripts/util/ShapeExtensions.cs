using Godot;

public static class ShapeExtensions
{
  public static Vector3 GetRandomPoint(this Shape3D shape)
  {
    var rng = new RandomNumberGenerator();
    if (shape is BoxShape3D boxShape)
    {
      var size = boxShape.Size;
      return new Vector3(
        rng.Randf(),
        rng.Randf(),
        rng.Randf()
      ) * 2f - Vector3.One * size;
    }
    return Vector3.Zero; // Default for unsupported shapes
  }
  public static Vector3 GetRandomPoint(this CollisionShape3D collisionShape)
  {
    if (collisionShape.Shape is Shape3D shape)
    {
      return shape.GetRandomPoint() + collisionShape.Position;
    }
    return Vector3.Zero; // Default for unsupported shapes
  }
}