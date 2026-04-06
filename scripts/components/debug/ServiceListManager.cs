using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Managers;

public partial class ServiceListManager : Node
{
  private Tree _tree;
  [FromServices]
  public void Inject(SceneManager SceneManager)
  {
    SceneManager.LoadingHidden += UpdateList;
  }
  public override void _EnterTree()
  {
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
}