using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025;
using ShipOfTheseus2025.DependencyInjection;

public partial class ServiceListManager : Node
{
  private CanvasLayer _canvasLayer;
  private Tree _tree;
  [FromServices]
  public void Inject(ISceneManager SceneManager)
  {
    SceneManager.LoadingHidden += UpdateList;
  }
  public override void _EnterTree()
  {
    _canvasLayer = GetNode<CanvasLayer>("%CanvasLayer");
    _canvasLayer.Visible = false;
    _tree = GetNode<Tree>("%Tree");
  }
  private void UpdateList()
  {
    _tree.Clear();
    var root = _tree.CreateItem();
    root.SetText(0, "Active Services");
    CreateGroup(root, "Global Services", Globals.Instance.GetActiveGlobalServices());
    CreateGroup(root, "Scene Services", Globals.Instance.GetActiveSceneServices());
  }
  private TreeItem CreateGroup(TreeItem parent, string groupName, List<string> services)
  {
    var group = _tree.CreateItem(parent);
    group.SetText(0, groupName);
    GD.Print($"Adding group {groupName} with {services.Count} services.");
    foreach (var service in services)
    {
      GD.Print($"Adding service {service} to group {groupName}.");
      var item = _tree.CreateItem(group);
      item.SetText(0, service);
    }
    return group;
  }
  private void ToggleVisibility()
  {
    _canvasLayer.Visible = !_canvasLayer.Visible;
  }
  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.F2)
    {
      ToggleVisibility();
    }
  }
}