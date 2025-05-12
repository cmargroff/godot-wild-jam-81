using Godot;
using System;

namespace ShipOfTheseus2025.Components.Game;

public class InventoryItem
{
    public string Name { get; set; }

    public int GoldValue { get; set; }

    public string Description { get; set; }

    public float Weight { get; set; }

    public Texture2D IconTexture { get; set; }

    public Node3D ItemScene { get; set; }
}
