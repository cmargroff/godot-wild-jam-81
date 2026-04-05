using System.IO;
using System.Linq;
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
    .AddSingleton(InjectNodeClass<AudioManager>())
    .AddSingleton(InjectInstantiatedPackedScene<SceneManager>("res://views/SceneManager.tscn"))
    .AddSingleton<ConfigStore>()
    .AddSingleton<SettingsStore>()
    .AddSingleton<ConfigManager>()
    .AddSingleton<RandomNumberGeneratorService>()
    .AddSingleton<ItemFactoryService>()
    .AddScoped(InjectNodeClass<ScoreManager>(false))
    .AddScoped(InjectNodeClass<GameEventManager>(false))
    .AddScoped(InjectNodeClass<ItemDragManager>())
    .AddScoped<IPauseManager>(InjectNodeClass<PauseManager>())
    .AddScoped(InjectNodeClass<HoverPanelManager>())
    .AddScoped(InjectNodeClass<EnvironmentManager>(false))
    .AddScoped(InjectNodeClass<ItemSpawnManager>(false))
    .AddScoped<IWaterManager>(InjectInstantiatedPackedScene<WaterManager>("res://components/game/Water.tscn", false))
    .AddScoped<PlayerDataStore>()
    .AddScoped<StatsManager>()
    .AddScoped<InventoryManager>()
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