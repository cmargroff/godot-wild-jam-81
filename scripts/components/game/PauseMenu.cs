using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class PauseMenu : CanvasLayer, IManagedScene
{
    private Button _continue;
    private Button _menu;
    private IPauseManager _pauseManager;
    public event Action<string> SceneClosing;

    [FromServices]
    public void Inject(IPauseManager pauseManager)
    {
        _pauseManager = pauseManager;
    }

    public override void _Ready()
    {
        Visible = false;
        _continue = GetNode<Button>("%Continue");
        _continue.Pressed += Continue;
        _menu = GetNode<Button>("%Menu");
        _menu.Pressed += Menu;
        _pauseManager.GamePauseChanged += IPauseManager_GamePauseChanged;
    }

    private void Continue()
    {
        _pauseManager.Unpause();
    }

    private void Menu()
    {
        _pauseManager.Unpause();
        SceneClosing?.Invoke("Title");
    }

    public void IPauseManager_GamePauseChanged(bool paused)
    {
        Visible = paused;
    }
}
