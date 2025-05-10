using Godot;
using JamTemplate.Managers;
using JamTemplate.Util;

namespace JamTemplate.Views;
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
