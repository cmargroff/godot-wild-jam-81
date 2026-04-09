using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class GameOver : CanvasLayer, IManagedScene
{
    private IScoreManager _scoreManager;
    private Control _root;
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
        _root = GetNode<Control>("%Root");
        Visible = false;
    }

    public void ShowScreen(bool win)
    {
        if (win) _label.Text = "You Won!";
        else _label.Text = "You Lost";
        _scoreLabel.Text = $"Score: {_scoreManager.Score}";

        _root.Modulate = new Color(1, 1, 1, 0);
        Visible = true;
        var tween = CreateTween();
        tween.TweenProperty(_root, "modulate:a", 1f, 0.5f);
    }

    public void Home()
    {
        SceneClosing?.Invoke("Title");
    }

    public void Restart()
    {
        SceneClosing?.Invoke("Game");
    }
}
