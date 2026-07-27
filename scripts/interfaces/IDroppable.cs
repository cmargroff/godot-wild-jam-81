using System;
using Godot;

public interface IDroppable
{
  public bool CanDrop(IDraggable draggable);
  public void OnDragOver(IDraggable draggable);
  public void OnDragOut(IDraggable draggable);
  public void OnDragDrop(IDraggable draggable);
  public Vector3 GetDropPosition();
  public Area2D GetDropArea();
}