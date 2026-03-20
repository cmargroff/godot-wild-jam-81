using System.IO;
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
    .AddSingleton(InjectNodeClass<GameManager>())
    .AddScoped<PlayerDataStore>()
    .AddSingleton<ConfigStore>()
    .AddSingleton<SettingsStore>()
    .AddSingleton<ConfigManager>()
    .AddSingleton(InjectNodeClass<AudioManager>())
    .AddScoped(InjectNodeClass<ScoreManager>())
    .AddSingleton<RandomNumberGeneratorService>()
    .AddSingleton(InjectInstantiatedPackedScene<SceneManager>("res://views/SceneManager.tscn"))
    .AddScoped<StatsManager>()
    .AddScoped(InjectNodeClass<GameEventManager>())
    .AddScoped<InventoryManager>()
    .AddScoped(InjectNodeClass<ItemDragManager>())
    .AddScoped<ItemSpawnManager>()
    .AddScoped(InjectNodeClass<PauseManager>())
    .AddSingleton<ItemFactoryService>()
    .AddScoped(InjectNodeClass<HoverPanelManager>(true))
    .AddScoped(InjectNodeClass<EnvironmentManager>(true))
    ;

    AddScenes();
    BuildServiceProvider();
    CreateSceneScope();
    ServiceProvider.GetRequiredService<GameManager>();
  }

  public void AddScenes()
  {
    var paths = SceneManager.ListAvailableScenes();
    foreach (var path in paths)
    {
      _serviceCollection.AddKeyedScoped(Path.GetFileNameWithoutExtension(path), InjectAvailableScene(path));
    }
  }

}