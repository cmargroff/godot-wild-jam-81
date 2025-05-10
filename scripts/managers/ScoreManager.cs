using Godot;
using System;

public partial class ScoreManager
{
    public int Score { get; private set; } = 0;

    public void ScoreChanged(int delta) => Score += delta;
}
