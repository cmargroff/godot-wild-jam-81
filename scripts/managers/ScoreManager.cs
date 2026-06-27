using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;

namespace ShipOfTheseus2025.Managers;

public partial class ScoreManager : Node, IScoreManager
{
    public event Action<int> ScoreChanged;
    public int Score { get; private set; } = 0;

    private IInventoryManager _inventoryManager;

    [FromServices]
    public void Inject(IInventoryManager inventoryManager)
    {
        _inventoryManager = inventoryManager;
        _inventoryManager.InventoryUpdated += InventoryManager_InventoryChanged;
    }

    public void InventoryManager_InventoryChanged(IEnumerable<InventoryItem> items)
    {
        Score = items.Sum(i => i.GoldValue);
        ScoreChanged?.Invoke(Score);
    }

}
