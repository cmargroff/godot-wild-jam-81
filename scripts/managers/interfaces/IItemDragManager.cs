using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Interfaces;

public interface IItemDragManager
{
  bool Dragging { get; }
  AudioStreamPlayer3D PickupAudioStreamPlayer { get; set; }
  void SetCamera(Camera3D camera);
  void StartDragItem(ItemPickUp item);
  void EndDragItem();
  void SnapPoint(ISnapPoint snapPoint, bool snap);
  void Attach();
  void Unsnap();
  ItemPickUp GetItem();
}