using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class DragHandler : Node, IDragHandler
{
  private IItemDragManager _dragManager;
  [FromServices]
  public void Inject(IItemDragManager dragManager)
  {
    _dragManager = dragManager;

  }

  public override void _EnterTree()
  {
    var item = GetParent<Item>();
    item.DragHandler = this;
    Enable();
  }

  public void Enable()
  {
  }

  public void Disable()
  {
  }

  public override void _ExitTree()
  {
    Disable();
  }
}
