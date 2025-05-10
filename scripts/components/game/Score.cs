using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace JamTemplate.Components.Game;

public partial class Score : Control
{
  private int _score;
  private Label _label;
  private ScoreManager _scoreManager;
  public override void _EnterTree()
  {
    _label = GetNode<Label>("Label");
    _scoreManager = Entry.ServiceProvider.GetRequiredService<ScoreManager>();
    _scoreManager.ScoreChanged += ScoreManager_ScoreChanged;
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