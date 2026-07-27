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
  public static Shape2D GetShape2D(this CollisionShape3D collisionShape)
  {
    if (collisionShape.Shape is BoxShape3D boxShape)
    {
      return boxShape.GetShape2D();
    }
    else if (collisionShape.Shape is SphereShape3D sphereShape)
    {
      return sphereShape.GetShape2D();
    }
    else if (collisionShape.Shape is CapsuleShape3D capsuleShape)
    {
      return capsuleShape.GetShape2D();
    }
    return null; // Default for unsupported shapes
  }
  public static RectangleShape2D GetShape2D(this BoxShape3D boxShape)
  {
    var rectangleShape2D = new RectangleShape2D();
    rectangleShape2D.Size = new Vector2(boxShape.Size.X, boxShape.Size.Y);
    return rectangleShape2D;
  }
  public static CircleShape2D GetShape2D(this SphereShape3D sphereShape)
  {
    var circleShape2D = new CircleShape2D();
    circleShape2D.Radius = sphereShape.Radius;
    return circleShape2D;
  }
  public static CapsuleShape2D GetShape2D(this CapsuleShape3D capsuleShape)
  {
    var capsuleShape2D = new CapsuleShape2D();
    capsuleShape2D.Radius = capsuleShape.Radius;
    capsuleShape2D.Height = capsuleShape.Height;
    return capsuleShape2D;
  }
}