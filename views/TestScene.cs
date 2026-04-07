using System;
using Godot;
using ShipOfTheseus2025.DependencyInjection;

public partial class TestScene : Node3D, IManagedScene
{
  private IPauseManager _pauseManager;
  private IStatsManager _statsManager;
  private IGameEventManager _eventManager;
  private IItemDragManager _dragManager;
  private IGameManager _gameManager;
  private IHoverPanelManager _hoverPanelManager;
  private IAudioManager _audioManager;
  private IWaterManager _waterManager;

  public event Action<string> SceneClosing;

  [FromServices]
  public void Inject(
    IPauseManager pauseManager, IStatsManager statsManager, IGameEventManager eventManager,
    IItemDragManager dragManager, IGameManager gameManager, IHoverPanelManager hoverPanelManager,
    IAudioManager audioManager, IWaterManager waterManager
  )
  {
    _statsManager = statsManager;
    _eventManager = eventManager;
    _dragManager = dragManager;
    _pauseManager = pauseManager;
    _gameManager = gameManager;
    _hoverPanelManager = hoverPanelManager;
    _audioManager = audioManager;
    _waterManager = waterManager;
  }
  public override void _Ready()
  {
    AddChild(_waterManager as Node3D);
  }
}