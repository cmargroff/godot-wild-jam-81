using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Resources;

public partial class ItemSpawnManager : Node, IItemSpawnManager
{
    private Vector3 _position = new Vector3(40, 4, 11);
    private PackedScene _itemScene;
    private ISceneManager _sceneManager;
    private ItemFactoryService _itemFactoryService;
    public AudioStreamPlayer3D ItemPickupAudio { get; private set; }

    [FromServices]
    public void Inject(ISceneManager sceneManager, ItemFactoryService itemFactoryService)
    {
        _sceneManager = sceneManager;
        _itemFactoryService = itemFactoryService;
    }

    public override void _EnterTree()
    {
        Name = GetType().Name;
        _itemScene = GD.Load<PackedScene>("res://components/game/ItemPickUp.tscn");
    }

    public override void _Ready()
    {
        // TODO get this from globals
        ItemPickupAudio = GetParent().GetParent().GetNode<AudioStreamPlayer3D>("%AudioStreamPlayer_ItemPickup");
    }

    public void Spawn(string identifier)
    {
        ItemResource resource = _sceneManager.PreloadedResources["Items"][identifier] as ItemResource;
        InventoryItem item = _itemFactoryService.GenerateItem(resource);
        ItemPickUp pickupableItem = _itemScene.Instantiate<ItemPickUp>();
        pickupableItem.ItemPickupAudioPlayer = ItemPickupAudio;
        pickupableItem.InventoryItem = item;
        pickupableItem.Position = _position;
        GetTree().CurrentScene.AddChild(pickupableItem);
    }
}
