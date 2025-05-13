using System;
using Godot;

public partial class InventoryItem : Node
{
    [Export]
    public string Name { get; set; }

    [Export]
    public int GoldValue { get; set; }
}
