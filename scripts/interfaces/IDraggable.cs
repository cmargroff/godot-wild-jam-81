using Godot;

public interface IDraggable
{
  public ItemCategory Category { get; }
  public Node3D GetVisualComponent();
  public Shape2D GetDragShape();
}