using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;

namespace ShipOfTheseus2025.Components.Game;

public partial class TravelProgress : Control
{
    private TextureProgressBar _progressBar;

    [FromServices]
    public void Inject(IStatsManager statsManager)
    {
        statsManager[Stat.Progress].OnChanged += Progress_OnChanged;
    }

    private void Progress_OnChanged(float val)
    {
        _progressBar.Value = val;
    }

    public override void _Ready()
    {
        _progressBar = GetNode<TextureProgressBar>("%TravelProgressBar");
    }
}
