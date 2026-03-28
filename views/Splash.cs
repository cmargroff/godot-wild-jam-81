using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Managers;

namespace ShipOfTheseus2025.Views;

public partial class Splash : Control, IManagedScene
{
  private SceneManager _sceneManager;

  public event Action<string> SceneClosing;

  [FromServices]
  public void Inject(SceneManager sceneManager)
  {
    _sceneManager = sceneManager;
  }
  public void Timeout()
  {
    SceneClosing?.Invoke("Title");
  }
  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey keyEvent && keyEvent.IsPressed())
    {
      SceneClosing?.Invoke("Title");
    }
  }
}
