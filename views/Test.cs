using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class Test : Node3D
{
  private IWaterManager _waterManager;
  [FromServices]
  public void Inject(IWaterManager waterManager)
  {
    _waterManager = waterManager;
  }
  public override void _Ready()
  {
    AddChild(_waterManager as Node3D);
  }
}