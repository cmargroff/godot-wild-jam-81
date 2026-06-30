using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Resources;

public partial class ItemSpawnManager : Node3D, IItemSpawnManager
{
    [Export]
    public AudioStream ItemPickupAudio;
    private Vector3 _position;
    private ISceneManager _sceneManager;
    private ItemFactoryService _itemFactoryService;
    private IAudioManager _audioManager;
    private AudioStreamPlayer _pickupSFX;
    private Dictionary<StringName, CollisionShape3D> _spawnZones = [];

    [FromServices]
    public void Inject(ISceneManager sceneManager, ItemFactoryService itemFactoryService, IAudioManager audioManager)
    {
        _sceneManager = sceneManager;
        _itemFactoryService = itemFactoryService;
        _audioManager = audioManager;
    }

    public override void _EnterTree()
    {
        foreach (CollisionShape3D child in GetChildren())
        {
            _spawnZones.Add(child.Name, child);
        }
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
        item.Node.Position = GetSpawnPosition(resource);
        AddChild(item.Node);
    }

    public Vector3 GetSpawnPosition(ItemResource resource)
    {
        var spawnZone = System.Enum.GetName(typeof(ItemSpawnZone), resource.SpawnZone);
        if (_spawnZones.ContainsKey(spawnZone))
        {
            return _spawnZones[spawnZone].GetRandomPoint();
        }
        else
        {
            GD.PrintErr($"Spawn zone '{spawnZone}' not found.");
            return Vector3.Zero;
        }
    }

}
