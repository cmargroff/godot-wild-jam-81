using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class GameOver : Control, IManagedScene
{
    private IScoreManager _scoreManager;
    private int _score;
    private Label _scoreLabel;
    private Label _label;
    private Button _home;
    private Button _restart;

    public event Action<string> SceneClosing;

    [FromServices]
    public void Inject(IScoreManager scoreManager)
    {
        _scoreManager = scoreManager;
    }

    public override void _EnterTree()
    {
        _scoreLabel = GetNode<Label>("%Score");
        _label = GetNode<Label>("%Label");
    }

    public void ShowScreen(bool win)
    {
        if (win) _label.Text = "You Won!";
        else _label.Text = "You Lost";
        _scoreLabel.Text = $"Score: {_score}";

        Modulate = new Color(1, 1, 1, 0);
        Visible = true;
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 1f, 0.5f);
    }

    public void Home()
    {
        SceneClosing?.Invoke("Title");
    }

    public void Restart()
    {
        //code to restart game
        GD.Print("restart");
        SceneClosing?.Invoke("Game");
    }
}
