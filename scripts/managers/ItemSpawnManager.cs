using Godot;
using System;

public partial class ItemSpawnManager : Node
{
    private Vector3 _position = new Vector3(20, 0, -6);
    public void Spawn()
    {
        var itemScene = GD.Load<PackedScene>("res://components/game/ItemPickUp.tscn");
        Node3D itemSceneInstance = itemScene.Instantiate<Node3D>();
        itemSceneInstance.Position = _position;
        GetTree().CurrentScene.AddChild(itemSceneInstance);

    }


}
