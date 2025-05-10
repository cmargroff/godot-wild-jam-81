using Godot;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Views;
public partial class Splash : Control
{
  private SceneManager _sceneManager;
  [FromServices]
  public void Inject(SceneManager sceneManager)
  {
    _sceneManager = sceneManager;
  }
  public void Timeout()
  {
    _sceneManager.ChangeScene("Title");
  }
  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey keyEvent && keyEvent.IsPressed())
    {
      _sceneManager.ChangeScene("Title");
    }
  }
}
