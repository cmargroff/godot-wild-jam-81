using Godot;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Util;
using System;

namespace ShipOfTheseus2025.Components.Game;

public partial class TravelProgress : Control
{
    private ProgressBar _progressBar;

   


    private StatsManager _statsManager;

    [FromServices]
    public void Inject(StatsManager statsManager)
    {
        _statsManager = statsManager;
        statsManager.StatChanged += StatsManager_StatChanged;
    }

    private void StatsManager_StatChanged(Stat stat, float val)
    {
        if (stat != Stat.Progress)
            return;
        //convert knots to relative change to speed scale
        // SpeedScale = val / InitialKnots;
        _progressBar.Value = val;
    }

    public override void _Ready()
    {
        _progressBar = GetNode<ProgressBar>("%TravelProgressBar");
    }

 
}
