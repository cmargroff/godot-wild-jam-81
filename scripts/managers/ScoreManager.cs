using Godot;
using JamTemplate.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ScoreManager : Node
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);
    public int Score { get; private set; } = 0;

    public void InventoryManager_InventoryChanged(IEnumerable<InventoryItem> items)
    {
        Score = items.Sum(i => i.GoldValue);
        EmitSignal(SignalName.ScoreChanged, Score);
    }
}
