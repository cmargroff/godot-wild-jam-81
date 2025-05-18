using System;
using System.Collections.Generic;
using Godot;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Util;

public partial class GameEventManager : Node
{
  // times in seconds;
  const float MIN_ENVIRONMENT_TIME = 10;
  const float MAX_ENVIRONMENT_TIME = 60;
  const float MIN_ITEM_TIME = 1;
  const float MAX_ITEM_TIME = 15;
  private RandomNumberGeneratorService _rng;
  private Timer _environmentTimer;
  private Timer _itemTimer;
  private GameManager _gameManager;
  private ItemSpawnManager _spawnManager;
  private SceneManager _sceneManager;

  public event Action EnvironmentEvent;

  [FromServices]
  public void Inject(RandomNumberGeneratorService rng, ItemSpawnManager spawnManager, GameManager gameManager, SceneManager sceneManager)
  {
    _rng = rng;
    _gameManager = gameManager;
    _spawnManager = spawnManager;
    _sceneManager = sceneManager;
    AddChild(_spawnManager);

  }
  public override void _EnterTree()
  {
    Name = GetType().Name;
    InitTimers();
  }
  private void InitTimers()
  {
    _environmentTimer = new();
    _environmentTimer.OneShot = true;
    _environmentTimer.Autostart = false;
    _environmentTimer.Connect(Timer.SignalName.Timeout, Callable.From(DispatchEnvironmentEvent));
    AddChild(_environmentTimer);

    _itemTimer = new();
    _itemTimer.OneShot = true;
    _itemTimer.Autostart = false;
    _itemTimer.Connect(Timer.SignalName.Timeout, Callable.From(DispatchItemEvent));
    AddChild(_itemTimer);
  }
  public void Start()
  {
    // code that the game calls when its ready to start the events
    // this should only be called once
    QueueEnvironmentEvent();
    QueueItemEvent();
  }
  public void QueueEnvironmentEvent()
  {
    _environmentTimer.Start(
      _rng.GetFloatRange(MIN_ENVIRONMENT_TIME, MAX_ENVIRONMENT_TIME)
    );
    GD.Print($"Queueing environment event for {_environmentTimer.TimeLeft} seconds");
  }
  public void QueueItemEvent()
  {
    _itemTimer.Start(
      _rng.GetFloatRange(MIN_ITEM_TIME, MAX_ITEM_TIME)
    );
    GD.Print($"Queueing item event for {_itemTimer.TimeLeft} seconds");
  }
  public void End()
  {
    // code called when run ends to stop and clean up events
    _environmentTimer.Stop();
    _itemTimer.Stop();
  }

  public void DispatchEnvironmentEvent()
  {
    GD.Print("Dispatching environment event");
    QueueEnvironmentEvent();
    // _evironmentManager.Start(identifier);
    EnvironmentEvent?.Invoke();
  }
  public void DispatchItemEvent()
  {
    GD.Print("Dispatching item event");
    QueueItemEvent();
    string item = null;
    if (_gameManager.EnabledItems.Count > 0)
    {
      do
      {
        item = _gameManager.EnabledItems[_rng.GetIntRange(0, _gameManager.EnabledItems.Count - 1)];
        if (_sceneManager.PreloadedResources is null ||
            !_sceneManager.PreloadedResources.TryGetValue("Items", out Dictionary<string, Resource> itemDict) ||
            !itemDict.TryGetValue(item, out Resource itemResource) || itemResource is null)
        {
          _gameManager.EnabledItems.Remove(item);
          GD.PrintErr($"The item {item} has not been preloaded.");
          item = null;
        }
      } while (item is null && _gameManager.EnabledItems.Count > 0);
    }
    if (item is not null)
      _spawnManager.Spawn(item);
  }
}
