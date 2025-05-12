using Godot;
using ShipOfTheseus2025.Enum;
using System;

namespace ShipOfTheseus2025.Resources;

[GlobalClass]
public partial class ItemResource : Resource
{
    [Export]
    public string ItemName { get; set; }

    [Export]
    public string Description { get; set; }

    [Export]
    public int MinGoldValue { get; set; }

    [Export]
    public int MaxGoldValue { get; set; }

    [Export]
    public GoldValueDistribution GoldValueDistribution { get; set; }

    [Export]
    public float MinWeight { get; set; }

    [Export]
    public float MaxWeight { get; set; }

    [Export]
    public Texture2D IconTexture { get; set; }

    [Export]
    public PackedScene ItemScene { get; set; }

    [Export]
    public bool Disabled { get; set; }
}
