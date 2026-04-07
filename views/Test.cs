using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class Test : Node3D
{
  private IWaterManager _waterManager;
  private IPauseManager _pauseManager;
  [FromServices]
  public void Inject(
    IPauseManager pauseManager,
    IWaterManager waterManager)
  {
    _pauseManager = pauseManager;
    _waterManager = waterManager;
  }
  public override void _Ready()
  {
    AddChild(_waterManager as Node3D);
  }
}