using Godot;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Util;

public partial class GameEventManager : Node
{
  // times in seconds;
  const float MIN_ENVIRONMENT_TIME = 10;
  const float MAX_ENVIRONMENT_TIME = 60;
  const float MIN_ITEM_TIME = 5;
  const float MAX_ITEM_TIME = 25;
  private RandomNumberGeneratorService _rng;
  private Timer _environmentTimer;
  private Timer _itemTimer;
  private ItemSpawnManager _spawnManager;
  [FromServices]
  public void Inject(RandomNumberGeneratorService rng, ItemSpawnManager spawnManager)
  {
    _rng = rng;
    _spawnManager = spawnManager;
    AddChild(_spawnManager);

  }
  public override void _EnterTree()
  {
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
  }
  public void DispatchItemEvent()
  {
    GD.Print("Dispatching item event");
    QueueItemEvent();
    _spawnManager.Spawn("item");
  }
}
