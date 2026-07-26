using Godot;

public interface IItemDragManager
{
  void SetCamera(Camera3D camera);
  void StartDragItem(IDraggable draggable);
  void EndDragItem();
  void Register(IDroppable droppable);
}