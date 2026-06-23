using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Resources;

public partial class ItemSpawnManager : Node, IItemSpawnManager
{
    [Export]
    public PackedScene ItemScene;
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
        if (ItemScene == null)
        {
            GD.PrintErr("ItemScene is not assigned in the inspector.");
            return;
        }
        ItemResource resource = _sceneManager.PreloadedResources["Items"][identifier] as ItemResource;
        InventoryItem item = _itemFactoryService.GenerateItem(resource);
        ItemPickUp pickupableItem = ItemScene.Instantiate<ItemPickUp>();
        pickupableItem.ItemPickupAudioPlayer = _pickupSFX;
        pickupableItem.InventoryItem = item;
        pickupableItem.Position = _position;
        GetTree().CurrentScene.AddChild(pickupableItem);
    }
}
