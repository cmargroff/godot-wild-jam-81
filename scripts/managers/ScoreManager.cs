using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipOfTheseus2025.Managers;

public partial class ScoreManager : Node
{
    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);
    public int Score { get; private set; } = 0;

    private InventoryManager _inventoryManager;

    [FromServices]
    public void Inject(InventoryManager inventoryManager)
    {
        _inventoryManager = inventoryManager;
        _inventoryManager.InventoryUpdated += InventoryManager_InventoryChanged;
    }

    public void InventoryManager_InventoryChanged(IEnumerable<InventoryItem> items)
    {
        Score = items.Sum(i => i.GoldValue);
        EmitSignal(SignalName.ScoreChanged, Score);
    }

    public void AddGold(int gold)
    {
        Score += gold;
        EmitSignal(SignalName.ScoreChanged, Score);
    }

    public void RemoveGold(int gold)
    {
        Score -= gold;
        EmitSignal(SignalName.ScoreChanged, Score);
    }

}
