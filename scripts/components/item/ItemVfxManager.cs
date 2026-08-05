using System.Collections.Generic;
using Godot;

/// <summary>
/// Identifies and manages visual effect nodes for an Item.
/// </summary>
public partial class ItemVfxManager : Node, IVfxManager
{
  private List<Node3D> _vfxNodes = new List<Node3D>();
  public override void _Ready()
  {
    var parent = GetParent<Item>();
    if (parent is null)
    {
      QueueFree();
      return;
    }
    parent.VfxManager = this;
    IndexVFXNodes(parent);
  }
  private void IndexVFXNodes(Item parent)
  {
    _vfxNodes.Clear();
    var visualNode = parent.Visual;
    foreach (var child in visualNode.GetChildren())
    {
      // for now only index particle systems
      if (child is GpuParticles3D || child is CpuParticles3D)
      {
        _vfxNodes.Add((Node3D)child);
      }
    }
  }
  public void Enable()
  {
    foreach (var vfxNode in _vfxNodes)
    {
      vfxNode.ProcessMode = Node.ProcessModeEnum.Always;
      vfxNode.Visible = true;
    }
  }
  public void Disable()
  {
    foreach (var vfxNode in _vfxNodes)
    {
      vfxNode.ProcessMode = Node.ProcessModeEnum.Disabled;
      vfxNode.Visible = false;
    }
  }
}