using Godot;
using Godot.Collections;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Managers;

public partial class GameManager : Node
{
  private SceneManager _sceneManager;
  private ConfigManager _configManager;

  [FromServices]
  public void Inject(SceneManager sceneManager, ConfigManager configManager)
  {
    GD.Print(GetType().Name, " Constructed");
    _sceneManager = sceneManager;
    _configManager = configManager;
  }

  public override void _Ready()
  {
    GD.Print(GetType().Name, " Started");
    var initialScenePath = (string)_configManager.GetValue("game", "INITIAL_SCENE_NAME");
    if (initialScenePath != "")
    {
      _sceneManager.ChangeScene(initialScenePath);
    }
  }
}
