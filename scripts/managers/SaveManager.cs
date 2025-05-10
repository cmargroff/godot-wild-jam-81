using Godot;
using Godot.Collections;

namespace ShipOfTheseus2025.Managers;

public partial class SaveManager : Node
{
  public const string SaveGroup = "savedata";
  public override void _EnterTree()
  {
    LoadAll();
  }
  public Array<Node> FindSavedNodes()
  {
    return GetTree().GetNodesInGroup(SaveGroup);
  }
  public void Load(Node node)
  {
    var path = $"user://{node.Name}.scn";
    if (!FileAccess.FileExists(path))
    {
      GD.Print($"Skipping loading save for {node.Name}");
    }
    else
    {
      var packed = ResourceLoader.Load<PackedScene>(path);
      var loaded = packed.Instantiate();
      var parent = node.GetParent();
      parent.CallDeferred("remove_child", node);
      parent.CallDeferred("add_child", loaded);
      node.QueueFree();
    }
  }
  public void LoadAll()
  {
    var nodes = FindSavedNodes();
    for (int i = 0; i < nodes.Count; i++)
    {
      Load(nodes[i]);
    }
  }
  public void Save(Node node)
  {
    var packed = new PackedScene();
    packed.Pack(node);
    ResourceSaver.Save(packed, $"user://{node.Name}.scn");
  }
  public void SaveAll()
  {
    var nodes = FindSavedNodes();
    for (int i = 0; i < nodes.Count; i++)
    {
      Save(nodes[i]);
    }
  }
}
