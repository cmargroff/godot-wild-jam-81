using Godot;
using ShipOfTheseus2025.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipOfTheseus2025.Managers;

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
