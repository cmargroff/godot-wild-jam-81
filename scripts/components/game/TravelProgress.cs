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
        _progressBar = GetNode<TextureProgressBar>("%TravelProgressBar");
    }
}
