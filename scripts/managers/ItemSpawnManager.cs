using System;
using System.Collections.Generic;
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
        Name = GetType().Name;
        foreach(CollisionShape3D child in GetChildren())
        {
            _spawnZones.Add(child.Name, child);
        }

        foreach(var i in _spawnZones)
        {
            GD.Print($"Key: {i.Key}, Value: {i.Value}");
        }
        GD.Print(_spawnZones["Water"]);
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
        GD.Print($"Check spawn {_spawnZones["Water"]}");
        _position = _spawnZones["Water"].GetRandomPoint();
        item.Node.Position = _position;
        GD.Print($"Spawn position: {_position}");
        AddChild(item.Node);
    }  

}
