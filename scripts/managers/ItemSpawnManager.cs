using Godot;

public partial class ItemSpawnManager : Node
{
    private Vector3 _position = new Vector3(20, 0, -6);
    private PackedScene _itemScene;
    public override void _EnterTree()
    {
        _itemScene = GD.Load<PackedScene>("res://components/game/ItemPickUp.tscn");
    }
    public void Spawn(string identifier)
    {
        Node3D itemSceneInstance = _itemScene.Instantiate<Node3D>();
        itemSceneInstance.Position = _position;
        GetTree().CurrentScene.AddChild(itemSceneInstance);
    }
}
