using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Managers;

namespace ShipOfTheseus2025.Components.Game;

public partial class Score : Control
{
  private int _score;
  private Label _label;
  private ScoreManager _scoreManager;
  public override void _EnterTree()
  {
    _label = GetNode<Label>("%Label");
    _scoreManager = Globals.ServiceProvider.GetRequiredService<ScoreManager>();
    _scoreManager.ScoreChanged += ScoreManager_ScoreChanged;
    _label.Text = _scoreManager.Score.ToString();
  }
  private void ScoreManager_ScoreChanged(int Score)
  {
    _score = Score;
    // maybe we do some animation here
    // animation would be a matter of holding a target value and then updating
    // the current value per frame until they are the same
    _label.Text = _score.ToString();
  }
}