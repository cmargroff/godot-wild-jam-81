using System;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;

public partial class PauseMenu : Control
{
    private Button _continue;
    private Button _menu;
    private PauseManager _pauseManager;
    private SceneManager _sceneManager;

    public override void _Ready()
    {
        Visible = false;
        _continue = GetNode<Button>("%Continue");
        _continue.Pressed += Continue;
        _menu = GetNode<Button>("%Menu");
        _menu.Pressed += Menu;
        _pauseManager = Globals.Instance.ServiceProvider.GetRequiredService<PauseManager>();
        _pauseManager.GamePauseChanged += PauseManager_GamePauseChanged;
        _sceneManager = Globals.Instance.ServiceProvider.GetRequiredService<SceneManager>();

    }

    private void Continue()
    {
        _pauseManager.Unpause();
    }

    private void Menu()
    {
        _pauseManager.Unpause();
        _sceneManager.ChangeScene("Title");
    }

    public void PauseManager_GamePauseChanged(bool paused)
    {
        GD.Print("menu");
        Visible = paused;
    }


}
