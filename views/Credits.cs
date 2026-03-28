using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Managers;

public partial class Credits : Control, IManagedScene
{
  [Export]
  public string CopyResourcePath;
  private SceneManager _sceneManager;

  public event Action<string> SceneClosing;

  [FromServices]
  public void Inject(SceneManager sceneManager)
  {
    _sceneManager = sceneManager;
  }
  public override void _EnterTree()
  {
    if (CopyResourcePath != null)
    {
      var handle = FileAccess.Open(CopyResourcePath, FileAccess.ModeFlags.Read);
      var copy = handle.GetAsText();
      GetNode<RichTextLabel>("%BodyCopy").Text = copy + "\r\n\r\n\r\n[center]Press any key to go back[/center]";
    }
  }
  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey keyEvent && keyEvent.IsPressed())
    {
      SceneClosing?.Invoke("Title");
    }
  }
}
