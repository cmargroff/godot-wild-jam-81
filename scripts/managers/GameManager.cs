using Godot;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Managers;

public partial class GameManager : Node
{
  private ConfigFile _config;
  private SceneManager _sceneManager;
  [FromServices]
  public void Inject(SceneManager sceneManager)
  {
    GD.Print(GetType().Name, " Constructed");
    _config = new ConfigFile();
    _config.Load("res://config.ini");
    _sceneManager = sceneManager;
  }
  public override void _Ready()
  {
    GD.Print(GetType().Name, " Started");
    var InitialScenePath = (string)_config.GetValue("game", "INITIAL_SCENE_NAME");
    if (InitialScenePath != "")
    {
      _sceneManager.ChangeScene(InitialScenePath);
    }
  }
}
