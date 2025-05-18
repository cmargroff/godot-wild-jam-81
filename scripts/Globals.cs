using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Stores;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025;

public partial class Globals : Node
{
  private static ServiceProvider _serviceProvider;
  private static IServiceScope _currentScope;
  public static IServiceProvider ServiceProvider
  {
    get
    {
      if (_currentScope != null) return _currentScope.ServiceProvider;
      return _serviceProvider;
    }
  }

  public override void _EnterTree()
  {
    var services = new ServiceCollection()
    .AddScoped(InjectNodeClass<GameManager>())
    .AddScoped<PlayerDataStore>()
    .AddSingleton<ConfigStore>()
    .AddSingleton<SettingsStore>()
    .AddSingleton<ConfigManager>()
    .AddScoped(InjectNodeClass<AudioManager>())
    .AddScoped(InjectNodeClass<ScoreManager>())
    .AddSingleton<RandomNumberGeneratorService>()
    .AddSingleton(InjectInstantiatedPackedScene<SceneManager>("res://views/SceneManager.tscn"))
    .AddScoped<StatsManager>()
    .AddScoped(InjectNodeClass<GameEventManager>())
    .AddScoped<InventoryManager>()
    .AddScoped(InjectNodeClass<ItemDragManager>())
    .AddTransient<ItemSpawnManager>()
    .AddScoped(InjectNodeClass<PauseManager>())
    .AddSingleton<ItemFactoryService>()
    .AddScoped(InjectNodeClass<HoverPanelManager>(true))
    ;

    AddScenes(services);
    _serviceProvider = services.BuildServiceProvider();
    CreateSceneScope();
  }

  private Func<IServiceProvider, T> InjectNodeClass<T>(bool autoParent = false) where T : Node, new()
  {
    return (serviceProvider) =>
    {
      var obj = new T();

      InjectAttributedMethods(obj, serviceProvider);

      if (autoParent)
      {
        AddChild(obj);
      }
      return obj;
    };
  }
  private Func<IServiceProvider, T> InjectInstantiatedPackedScene<T>(string path, bool autoParent = true) where T : Node
  {
    return (serviceProvider) =>
    {
      var packed = ResourceLoader.Load<PackedScene>(path);
      var node = packed.Instantiate<T>();

      InjectAttributedMethods(node, serviceProvider);

      if (autoParent)
      {
        GetTree().Root.CallDeferred("add_child", node);
      }
      return node;
    };
  }
  public static void InjectAttributedMethods<T>(T obj, IServiceProvider provider)
  {
    var objType = obj.GetType();
    var methods = objType
      .GetMethods(BindingFlags.Instance | BindingFlags.Public)
      .Where(method => method.GetCustomAttribute<FromServicesAttribute>() != null);

    foreach (var method in methods)
    {
      var args = method
        .GetParameters()
        .Select(param => provider.GetService(param.ParameterType)).ToArray();
      method.Invoke(obj, args);
    }

    var objFields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public)
    .Where(fieldInfo => !fieldInfo.FieldType.IsValueType && fieldInfo.FieldType.IsClass);

    foreach (var fieldInfo in objFields)
    {
      var val = fieldInfo.GetValue(obj);
      if (val != null)
        InjectAttributedMethods(val, provider);
    }
    // inject children in the scene tree
    if (obj is Node node && node.GetChildCount() > 0)
    {
      foreach (var child in node.GetChildren())
      {
        InjectAttributedMethods(child, provider);
      }
    }
  }

  public void AddScenes(IServiceCollection collection)
  {
    var paths = SceneManager.ListAvailableScenes();
    foreach (var path in paths)
    {
      collection.AddKeyedTransient(Path.GetFileNameWithoutExtension(path), InjectAvailableScene(path));
    }
  }

  public Func<IServiceProvider, object?, Node> InjectAvailableScene(string path)
  {
    return (ServiceProvider, serviceKey) => InjectInstantiatedPackedScene<Node>(path, false)(ServiceProvider);
  }

  public static void CreateSceneScope()
  {
    if (_currentScope is not null)
        throw new InvalidOperationException("You must close the service scope before opening a new one. Call " + nameof(CloseSceneScope) + "().");
    _currentScope = _serviceProvider.CreateScope();
  }

  public static void CloseSceneScope()
  {
    _currentScope?.Dispose();
    _currentScope = null;
  }
}