using Godot;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Util;
using System;

namespace ShipOfTheseus2025.Components.Game;

public partial class TravelProgress : Control
{
    private ProgressBar _progressBar;

    /// <summary>
    /// The expected time for the game to finish at normal speed, in seconds.
    /// </summary>
    public float RunTimeAt1X { get; set; } = 600f;

    /// <summary>
    /// The time remaining
    /// </summary>
    public float RemainingTime { get; set; } = 600f;

    public float InitialKnots { get; set; } = 7f;

    /// <summary>
    /// The rate at which the remaining time changes, relative to 1x.
    /// NOTE: This is NOT distance per time but time per time.
    /// </summary>
    [Export]
    public float SpeedScale { get; set; } = 1f;

    private StatsManager _statsManager;

    [FromServices]
    public void Inject(StatsManager statsManager)
    {
        _statsManager = statsManager;
        statsManager.StatChanged += StatsManager_StatChanged;
    }

    private void StatsManager_StatChanged(Stat stat, float val)
    {
        if (stat != Stat.Speed)
            return;
        //convert knots to relative change to speed scale
        SpeedScale = val / InitialKnots;
    }

    public override void _Ready()
    {
        _progressBar = GetNode<ProgressBar>("%TravelProgressBar");
    }

    public override void _PhysicsProcess(double delta)
    {
        RemainingTime = Math.Max(0, RemainingTime - (float)(delta * SpeedScale));
        _progressBar.Value = (1 - RemainingTime / RunTimeAt1X);
    }
}
