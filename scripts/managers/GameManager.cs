using System;
using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025.Resources;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Managers;

public partial class GameManager : Node
{
    private SceneManager _sceneManager;
    private ConfigManager _configManager;

    public Godot.Collections.Array<string> EnabledItems { get; set; }

    [FromServices]
    public void Inject(SceneManager sceneManager, ConfigManager configManager)
    {
        GD.Print(GetType().Name, " Constructed");
        _sceneManager = sceneManager;
        _configManager = configManager;
    }

    public override void _Ready()
    {
        Name = GetType().Name;
        GD.Print(GetType().Name, " Started");
        var initialScenePath = (string)_configManager.GetValue("game", "INITIAL_SCENE_NAME");
        if (initialScenePath != "")
        {
            _sceneManager.ChangeScene(initialScenePath);
        }
    }

    public void LoadConfig()
    {
        EnabledItems = (Godot.Collections.Array<string>)_configManager.GetValue("game", "ENABLED_ITEMS");
    }

    public void StartGame()
    {
        LoadConfig();
        Dictionary<string, Dictionary<string, string>> preloads = CreatePreloadsFromIni();
        _sceneManager.ChangeScene("Game", preloads);
    }

    private Dictionary<string, Dictionary<string, string>> CreatePreloadsFromIni()
    {
        Dictionary<string, Dictionary<string, string>> preloads = new() { { "Items", new() }, { "AudioRandomizers", new() } };
        foreach (string itemResourceFileName in EnabledItems)
        {
            preloads["Items"].Add(itemResourceFileName, $"res://resources/Items/{itemResourceFileName}");
        }
        preloads["AudioRandomizers"].Add("ship_creaking_audio_stream_randomizer.tres", "res://resources/audio/ship_creaking_audio_stream_randomizer.tres");
        preloads["AudioRandomizers"].Add("waves_audio_stream_randomizer.tres", "res://resources/audio/waves_audio_stream_randomizer.tres");
        return preloads;
    }
#if DEBUG
    public void LoadItemsDirectly()
    {
        Dictionary<string, Dictionary<string, string>> preloads = CreatePreloadsFromIni();
        if (_sceneManager.PreloadedResources is null)
            _sceneManager.PreloadedResources = [];
        if (!_sceneManager.PreloadedResources.TryGetValue("Items", out Dictionary<string, Resource> itemDict))
        {
            itemDict = new();
            _sceneManager.PreloadedResources.Add("Items", itemDict);
            foreach (KeyValuePair<string, string> itemKey in preloads["Items"])
            {
                ItemResource itemRes = ResourceLoader.Load<ItemResource>(itemKey.Value);
                itemDict.TryAdd(itemKey.Key, itemRes);
            }
        }
        if (!_sceneManager.PreloadedResources.TryGetValue("AudioRandomizers", out Dictionary<string, Resource> audioDict))
        {
            audioDict = new();
            _sceneManager.PreloadedResources.Add("AudioRandomizers", itemDict);
            foreach (KeyValuePair<string, string> audioEntry in preloads["AudioRandomizers"])
            {
                AudioStreamRandomizer audioStream = ResourceLoader.Load<AudioStreamRandomizer>(audioEntry.Value);
                audioDict.TryAdd(audioEntry.Key, audioStream);
            }
        }
    }
#endif
}
