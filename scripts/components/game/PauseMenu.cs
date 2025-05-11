using Godot;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;

public partial class PauseMenu : Control
{
    private Button _continue;
    private PauseManager _pauseManager;

    public override void _Ready()
    {
        Visible = false;
        _continue = GetNode<Button>("%Continue");
        _continue.Pressed += Continue;
        _pauseManager = Globals.ServiceProvider.GetRequiredService<PauseManager>();
        GD.Print(_pauseManager);
        _pauseManager.GamePauseChanged += PauseManager_GamePauseChanged;
        

    }

    private void Continue()
    {
        _pauseManager.Unpause();
    }

    public void PauseManager_GamePauseChanged(bool paused)
    {
        GD.Print("menu");
        Visible = paused;
    }


}
