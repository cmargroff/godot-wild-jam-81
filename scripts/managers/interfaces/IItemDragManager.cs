using Godot;
using ShipOfTheseus2025.Interfaces;

public interface IItemDragManager
{
  bool Dragging { get; }
  AudioStreamPlayer PickupAudioStreamPlayer { get; set; }
  void SetCamera(Camera3D camera);
  void StartDragItem(Item item);
  void EndDragItem();
  void SnapPoint(ISnapPoint snapPoint, bool snap);
  void Attach();
  void Unsnap();
  Item GetItem();
}