using System;

public interface IScoreManager
{
  int Score { get; }
  event Action<int> ScoreChanged;
}