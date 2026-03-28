using System;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;

public partial class PauseMenu : Control, IManagedScene
{
    private Button _continue;
    private Button _menu;
    private PauseManager _pauseManager;

    public event Action<string> SceneClosing;

    public override void _Ready()
    {
        Visible = false;
        _continue = GetNode<Button>("%Continue");
        _continue.Pressed += Continue;
        _menu = GetNode<Button>("%Menu");
        _menu.Pressed += Menu;
        _pauseManager = Globals.Instance.ServiceProvider.GetRequiredService<PauseManager>();
        _pauseManager.GamePauseChanged += PauseManager_GamePauseChanged;
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

    public void PauseManager_GamePauseChanged(bool paused)
    {
        GD.Print("menu");
        Visible = paused;
    }


}
