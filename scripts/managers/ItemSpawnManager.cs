using Godot;
using System;

public partial class ItemSpawnManager : Node
{
    [Export]
    private Vector3 _position = new Vector3(0, 0, -6);
    public void Spawn()
    {
        var itemScene = GD.Load<PackedScene>("res://components/game/Item.tscn");
        Node3D itemSceneInstance = itemScene.Instantiate<Node3D>();
        itemSceneInstance.Position = _position;
        GetTree().CurrentScene.AddChild(itemSceneInstance);

    }


}
