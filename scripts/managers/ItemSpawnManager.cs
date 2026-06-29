using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Resources;

public partial class ItemSpawnManager : Node3D, IItemSpawnManager
{
    [Export]
    public AudioStream ItemPickupAudio;
    private Vector3 _position = new Vector3(40, 4, 11);
    private ISceneManager _sceneManager;
    private ItemFactoryService _itemFactoryService;
    private IAudioManager _audioManager;
    private AudioStreamPlayer _pickupSFX;

    [FromServices]
    public void Inject(ISceneManager sceneManager, ItemFactoryService itemFactoryService, IAudioManager audioManager)
    {
        _sceneManager = sceneManager;
        _itemFactoryService = itemFactoryService;
        _audioManager = audioManager;
    }

    public override void _EnterTree()
    {
        Name = GetType().Name;
    }

    public override void _Ready()
    {
        _pickupSFX = new AudioStreamPlayer();
        _pickupSFX.Stream = ItemPickupAudio;
        AddChild(_pickupSFX);
    }

    public void Spawn(string identifier)
    {
        ItemResource resource = _sceneManager.PreloadedResources["Items"][identifier] as ItemResource;
        InventoryItem item = _itemFactoryService.GenerateItem(resource);
        // ItemPickUp pickupableItem = Globals.Instance.ServiceProvider.GetRequiredService<ItemPickUp>();
        // pickupableItem.ItemPickupAudioPlayer = _pickupSFX;
        // pickupableItem.InventoryItem = item;
        item.Node.Position = _position;
        AddChild(item.Node);
    }
}
