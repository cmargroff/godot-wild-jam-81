using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.DependencyInjection;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Stores;

namespace ShipOfTheseus2025;

public partial class Globals : DIContainerNode
{
  public static Globals Instance { get; private set; }

  public override void _EnterTree()
  {

    if (Instance != null)
    {
      GD.PrintErr("Multiple instances of Globals detected. This is not allowed. Destroying the new instance.");
      QueueFree();
      return;
    }
    Instance = this;

    CreateServiceCollection();

    _serviceCollection
    .AddSingleton<IGameManager>(InjectNodeClass<GameManager>())
    .AddSingleton<IAudioManager>(InjectNodeClass<AudioManager>())
    .AddSingleton<ISceneManager>(InjectInstantiatedPackedScene<SceneManager>("res://views/SceneManager.tscn"))
    .AddSingleton<ConfigStore>()
    .AddSingleton<SettingsStore>()
    .AddSingleton<ConfigManager>()
    .AddSingleton<RandomNumberGeneratorService>()
    .AddSingleton<ItemFactoryService>()
    .AddSingleton(InjectInstantiatedPackedScene<ServiceListManager>("res://components/debug/ServiceListManager.tscn"))
    .AddScoped<IScoreManager>(InjectNodeClass<ScoreManager>(false))
    .AddScoped<IGameEventManager>(InjectNodeClass<GameEventManager>())
    .AddScoped<IItemDragManager>(InjectNodeClass<ItemDragManager>())
    .AddScoped<IPauseManager>(InjectNodeClass<PauseManager>())
    .AddScoped<IHoverPanelManager>(InjectNodeClass<HoverPanelManager>())
    .AddScoped(InjectNodeClass<EnvironmentManager>(false))
    .AddScoped(InjectNodeClass<ItemSpawnManager>(false))
    .AddScoped<IWaterManager>(InjectInstantiatedPackedScene<WaterManager>("res://components/game/Water.tscn", false))
    .AddScoped<PlayerDataStore>()
    .AddScoped<IStatsManager, StatsManager>()
    .AddScoped<InventoryManager>()
    ;

    AddScenes();
    BuildServiceProvider();
    CreateSceneScope();
  }
  public override void _Ready()
  {
    ServiceProvider.GetRequiredService<IGameManager>().StartGame();
  }

  public void AddScenes()
  {
    var paths = SceneManager.ListAvailableScenes();
    foreach (var path in paths)
    {
      _serviceCollection.AddKeyedScoped(Path.GetFileNameWithoutExtension(path), InjectAvailableScene(path));
    }
  }
#if DEBUG
  private List<string> GetActiveServicesForProvider(IServiceProvider provider)
  {
    Type type = provider.GetType();
    PropertyInfo root = type.GetProperty("Root", BindingFlags.NonPublic | BindingFlags.Instance);
    if (root == null)
    {
      GD.PrintErr("Could not access the root of the service provider.");
      return new List<string>();
    }
    object rootValue = root.GetValue(provider);
    return GetDisposables(rootValue);
  }
  private List<string> GetDisposables(object engine)
  {
    Type ServiceProviderEngine = engine.GetType();
    FieldInfo Disposables = ServiceProviderEngine.GetField("_disposables", BindingFlags.NonPublic | BindingFlags.Instance);
    var disposables = Disposables.GetValue(engine) as List<object>;
    return disposables.Select(d => d.GetType().Name).ToList();
  }
  public List<string> GetActiveGlobalServices()
  {
    return GetActiveServicesForProvider(_serviceProvider);
  }
  public List<string> GetActiveSceneServices()
  {
    return GetDisposables(ServiceProvider);
  }
#endif
}