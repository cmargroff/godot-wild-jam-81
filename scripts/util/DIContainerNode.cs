
using System;
using System.Linq;
using System.Reflection;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace ShipOfTheseus2025.DependencyInjection;

public partial class DIContainerNode : Node
{
  protected IServiceCollection _serviceCollection;
  protected IServiceScope _currentScope;
  public IServiceScope CurrentScope => _currentScope;
  protected IServiceProvider _serviceProvider;
  public IServiceProvider ServiceProvider
  {
    get
    {
      if (_currentScope != null) return _currentScope.ServiceProvider;
      return _serviceProvider;
    }
  }

  protected void CreateServiceCollection()
  {
    if (_serviceCollection == null)
    {
      _serviceCollection = new ServiceCollection();
    }
  }

  protected IServiceProvider BuildServiceProvider()
  {
    if (_serviceProvider != null) return _serviceProvider;
    _serviceProvider = _serviceCollection.BuildServiceProvider();
    InjectDeferredInstances();
    return _serviceProvider;
  }

  protected Func<IServiceProvider, T> InjectNodeClass<T>(bool autoParent = true) where T : Node, new()
  {
    return (serviceProvider) =>
    {
      var node = new T();
      node.Name = typeof(T).Name + "_DI_Managed";

      InjectAttributedMethods(node);

      if (autoParent)
      {
        AddChild(node);
      }
      return node;
    };
  }

  protected Func<IServiceProvider, T> InjectInstantiatedPackedScene<T>(string path, bool autoParent = true) where T : Node
  {
    return (serviceProvider) =>
    {
      var packed = ResourceLoader.Load<PackedScene>(path);
      var node = packed.Instantiate<T>();
      var filename = System.IO.Path.GetFileNameWithoutExtension(path);
      node.Name = filename + "_DI_Managed";
      // node.Owner = this;

      InjectAttributedMethods(node);

      if (autoParent)
      {
        AddChild(node);
      }
      return node;
    };
  }

  protected void InjectAttributedMethods<T>(T obj)
  {
    var objType = obj.GetType();
    var methods = objType
      .GetMethods(BindingFlags.Instance | BindingFlags.Public)
      .Where(method => method.GetCustomAttribute<FromServicesAttribute>() != null);

    foreach (var method in methods)
    {
      var args = method
        .GetParameters()
        .Select(param =>
        {
          if (param.GetCustomAttribute<FromKeyedServicesAttribute>() is FromKeyedServicesAttribute keyedAttr)
          {
            return _currentScope.ServiceProvider.GetRequiredKeyedService(param.ParameterType, keyedAttr.Key);
          }
          return _currentScope.ServiceProvider.GetRequiredService(param.ParameterType);
        }).ToArray();
      method.Invoke(obj, args);
    }

    var objFields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public)
    .Where(fieldInfo => !fieldInfo.FieldType.IsValueType && fieldInfo.FieldType.IsClass);

    foreach (var fieldInfo in objFields)
    {
      var val = fieldInfo.GetValue(obj);
      if (val != null)
        InjectAttributedMethods(val);
    }
    // inject children in the scene tree
    if (obj is Node node && node.GetChildCount() > 0)
    {
      foreach (var child in node.GetChildren())
      {
        InjectAttributedMethods(child);
      }
    }
  }

  protected IServiceCollection AddDeferredInjectedInstance<TService>(TService instance)
  {
    _serviceCollection.AddSingleton(typeof(TService), instance);

    if (instance is Node node)
    {
      node.AddToGroup("deferred_inject", true);
    }

    return _serviceCollection;
  }

  protected IServiceCollection AddDeferredInjectedKeyedInstance<TService>(string key, TService instance)
  {
    _serviceCollection.AddKeyedSingleton(typeof(TService), key, instance);

    if (instance is Node node)
    {
      node.AddToGroup("deferred_inject", true);
    }

    return _serviceCollection;
  }

  private void InjectDeferredInstances()
  {
    var nodes = GetTree().GetNodesInGroup("deferred_inject");
    foreach (var obj in nodes)
    {
      if (obj is Node node)
      {
        if (node.Owner == this)
        {
          InjectAttributedMethods(node);
        }
      }
    }
  }

  protected Func<IServiceProvider, object, Node> InjectAvailableScene(string path)
  {
    return (ServiceProvider, serviceKey) => InjectInstantiatedPackedScene<Node>(path, false)(ServiceProvider);
  }

  public void CreateSceneScope()
  {
    if (_currentScope is not null)
      throw new InvalidOperationException("You must close the service scope before opening a new one. Call " + nameof(CloseSceneScope) + "().");
    _currentScope = _serviceProvider.CreateScope();
  }

  public void CloseSceneScope()
  {
    _currentScope?.Dispose();
    _currentScope = null;
  }
}