public interface IGameManager
{
  Godot.Collections.Array<string> EnabledItems { get; set; }
  void LoadConfig();
  void StartGame();
#if DEBUG

  void LoadItemsDirectly();
#endif
}