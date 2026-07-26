using System;
using Godot;

public interface IDroppable
{
  public bool CanDrop(IDraggable draggable);
  public void HandleOver(IDraggable draggable);
  public void HandleDrop(IDraggable draggable);
  public Action<IDraggable> OnOver { get; set; }
  public Action<IDraggable> OnOut { get; set; }
  public Action<IDraggable> OnDrop { get; set; }
  public Action<IDraggable> OnPickup { get; set; }
  public Vector3 GetDropPosition();
  public Shape2D GetDropShape();
}