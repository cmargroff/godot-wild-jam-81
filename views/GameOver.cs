using Godot;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ShipOfTheseus2025.Util;

public partial class GameOver : Control
{
    private ScoreManager _scoreManager;
    private SceneManager _sceneManager;
    private int _score;
    private Label _scoreLabel;
    private Button _home;
    private Button _restart;
    [FromServices]
    public void Inject(ScoreManager scoreManager, SceneManager sceneManager)
    {
        _scoreManager = scoreManager;
        _sceneManager = sceneManager;  
    }

    public override void _EnterTree()
    {
        _scoreLabel = GetNode<Label>("%Score");
        _scoreLabel.Text = $"Score: {_score}";
    }

    public void Home()
    {
        _sceneManager.ChangeScene("Title");
    }

    public void Restart()
    {
        //code to restart game
        GD.Print("restart");
    }
}
