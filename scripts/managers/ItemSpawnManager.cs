using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Resources;
using ShipOfTheseus2025.Util;

public partial class ItemSpawnManager : Node
{
    private Vector3 _position = new Vector3(20, 0, -6);
    private PackedScene _itemScene;
    private SceneManager _sceneManager;
    private ItemFactoryService _itemFactoryService;
    public AudioStreamPlayer3D ItemPickupAudio { get; private set; }

    [FromServices]
    public void Inject(SceneManager sceneManager, ItemFactoryService itemFactoryService)
    {
        _sceneManager = sceneManager;
        _itemFactoryService = itemFactoryService;
    }

    public override void _EnterTree()
    {
        _itemScene = GD.Load<PackedScene>("res://components/game/ItemPickUp.tscn");
    }

    public override void _Ready()
    {
        ItemPickupAudio = GetParent().GetParent().GetNode<AudioStreamPlayer3D>("%AudioStreamPlayer_ItemPickup");
    }

    public void Spawn(string identifier)
    {
        ItemResource resource = _sceneManager.PreloadedResources["Items"][identifier] as ItemResource;
        ShipOfTheseus2025.Components.Game.InventoryItem item = _itemFactoryService.GenerateItem(resource);
        ItemPickUp pickupableItem = _itemScene.Instantiate<ItemPickUp>();
        pickupableItem.ItemPickupAudioPlayer = ItemPickupAudio;
        pickupableItem.InventoryItem = item;
        pickupableItem.Position = _position;
        GetTree().CurrentScene.AddChild(pickupableItem);
    }
}
