using Godot;

public interface IDraggable
{
  public Item GetItem();
  public Node3D GetVisualComponent();
  public CollisionShape2D GetDragShape();
}