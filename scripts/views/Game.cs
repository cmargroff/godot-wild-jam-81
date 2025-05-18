using System;
using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;
using ShipOfTheseus2025.Components.Game;
using ShipOfTheseus2025.Enum;
using ShipOfTheseus2025.Managers;
using ShipOfTheseus2025.Models;
using ShipOfTheseus2025.Util;
using static Godot.EditorVcsInterface;

namespace ShipOfTheseus2025.Views;

public partial class Game : Node3D
{
  private SceneManager _sceneManager;
  private StatsManager _statsManager;
  private GameEventManager _eventManager;
  private ItemDragManager _dragManager;
  private PauseManager _pauseManager;
  private GameManager _gameManager;
  private AudioManager _audioManager;

  private GameOver _gameOverScreen;

  /// <summary>
  /// The expected time for the game to finish at normal speed, in seconds.
  /// </summary>
  public float RunTimeAt1X { get; set; } = 600f;

  /// <summary>
  /// The time remaining
  /// </summary>
  public float RemainingTime { get; set; } = 600f;

  public float InitialKnots { get; set; } = 7f;
  [Export]
  public float SpeedScale { get; set; } = 1f;

  [FromServices]
  public void Inject(SceneManager sceneManager, StatsManager statsManager, GameEventManager eventManager,
      ItemDragManager dragManager, PauseManager pauseManager, GameManager gameManager, HoverPanelManager hoverPanelManager, AudioManager audioManager)
  {
    _sceneManager = sceneManager;
    _statsManager = statsManager;
    _eventManager = eventManager;
    _dragManager = dragManager;
    AddChild(_eventManager);
    _pauseManager = pauseManager;
    AddChild(_pauseManager);
    AddChild(_dragManager);
    _gameManager = gameManager;
    _audioManager = audioManager;
  }

  public override void _EnterTree()
  {
    _gameOverScreen = GetNode<GameOver>("GameOver");
    //used when the Game scene is loaded directly, otherwise this will be skipped
    if (_sceneManager is null)
    {
      Globals.InjectAttributedMethods(this, Globals.ServiceProvider);
    }
    if (_gameManager.EnabledItems is null || _gameManager.EnabledItems.Count == 0)
      _gameManager.LoadConfig();
#if DEBUG
    if (_gameManager.EnabledItems is not null && _gameManager.EnabledItems.Count > 0)
    {
      _gameManager.LoadItemsDirectly();
    }
#endif
    if (WeatherPresets.Count > 0)
    {
        Weather = WeatherPresets["Calm"].Clone();
        foreach (WeatherType w in WeatherPresets.Values)
        {
            GD.Print(w.color_deep);
        }
    }
  }

  public override void _Ready()
  {
    _dragManager.SetCamera(GetNode<Camera3D>("Camera"));
    _eventManager.Start();
    _sceneManager.GetChild<Control>(0).Visible = false; //hides loading screen without crashing when running the game scene directly
    _audioManager.PlayGlobalAudioOnRepeat(_sceneManager.PreloadedResources["AudioRandomizers"]["waves_audio_stream_randomizer.tres"] as AudioStreamRandomizer,
        "SFX", this, new(0, 2f), true, (AudioStreamPlayer player) => player.VolumeDb = -6f, null);
    _audioManager.PlayGlobalAudioOnRepeat(_sceneManager.PreloadedResources["AudioRandomizers"]["ship_creaking_audio_stream_randomizer.tres"] as AudioStreamRandomizer,
        "SFX", this, new(2, 5f), false, null, null);

    //WeatherParent
    water = GetNode<MeshInstance3D>("Water"); //the other mesh instances copy this ones shader
    environment = GetNode<WorldEnvironment>("WorldEnvironment");
    sun = GetNode< DirectionalLight3D >("DirectionalLight3D");
    rain = GetNode<GpuParticles3D>("rain");
    //postprocessor = GetNode<ColorRect>("Camera/ColorRect");

    ChangeWeather(WeatherPresets["Calm"], 1.0f);
  }

  public override void _PhysicsProcess(double delta)
  {
    RemainingTime = Math.Max(0, RemainingTime - (float)(delta * SpeedScale));
    _statsManager.ChangeStat(new StatChange { Stat = Stat.Progress, Mode = StatChangeMode.Absolute, Amount = (1 - RemainingTime / RunTimeAt1X) * 100f });
    if (_statsManager.GetStats(Stat.WaterLevel) >= 100)
    {
      _gameOverScreen.ShowScreen(false);
    }
    else if (_statsManager.GetStats(Stat.Progress) >= 100)
    {
      _gameOverScreen.ShowScreen(true);
    }
  }

    #region Weather stuff
    [Export]
    public Dictionary<string, WeatherType> WeatherPresets { get; set; } = new();

    public WeatherType Weather;

    public MeshInstance3D water; //the other mesh instances copy this ones shader
    public WorldEnvironment environment;
    public DirectionalLight3D sun;
    public GpuParticles3D rain;
    public ColorRect postprocessor;

    private bool _changingWeather = false;
    private float _changeTime = 5.0f;

    private static MethodInfo VariantFromMethod = typeof(Variant).GetMethod("From");
    private Tween tween;

    public void ChangeWeather(WeatherType newWeather, float changeTime)
    {
        _changingWeather = true;
        tween = CreateTween().SetParallel(true);
        foreach (FieldInfo prop in typeof(WeatherType).GetFields().Where(f => f.GetCustomAttribute<TweenableAttribute>() is not null))
        {
            MethodInfo variantFrom = VariantFromMethod.MakeGenericMethod(prop.FieldType);
            //GD.Print($"Changed {prop.Name} from {prop.GetValue(Weather)} to {prop.GetValue(newWeather)}");
            tween.TweenProperty(Weather, prop.Name, (Variant)variantFrom.Invoke(null, [prop.GetValue(newWeather)]), changeTime);
        }
        tween.Finished += () =>
        {
            _changingWeather = false;
        };
        Weather.is_raining = newWeather.is_raining;
        rain.Emitting = newWeather.is_raining;
        //((ShaderMaterial)postprocessor.Material).SetShaderParameter("curve_r", weather.curve_r);
        //((ShaderMaterial)postprocessor.Material).SetShaderParameter("curve_g", weather.curve_g);
        //((ShaderMaterial)postprocessor.Material).SetShaderParameter("curve_b", weather.curve_b);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("weather1"))
        {
            GD.Print("Q pressed");
            ChangeWeather(WeatherPresets["Calm"].Clone(), _changeTime);
        } 
        else if (Input.IsActionJustPressed("weather2"))
        {
            GD.Print("W pressed");
            ChangeWeather(WeatherPresets["Red"].Clone(), _changeTime);
        }
        else if (Input.IsActionJustPressed("weather3"))
        {
            GD.Print("E pressed");
            ChangeWeather(WeatherPresets["Storm"].Clone(), _changeTime);
        }
        if (!_changingWeather)
            return;
        //GD.Print("Changing");
        ShaderMaterial waterShaderMaterial = (water.Mesh.SurfaceGetMaterial(0) as ShaderMaterial);
        foreach (FieldInfo waterField in typeof(WeatherType).GetFields().Where(f => f.GetCustomAttribute<WaterMaterialAttribute>() is not null))
        {
            MethodInfo variantFrom = VariantFromMethod.MakeGenericMethod(waterField.FieldType);
            Variant oldVal = waterShaderMaterial.GetShaderParameter(waterField.Name);
            Variant newVal = (Variant)variantFrom.Invoke(null, [waterField.GetValue(Weather)]);
            waterShaderMaterial?.SetShaderParameter(waterField.Name, newVal);
            if (waterField.Name != "color_deep")
                continue;
            if (!oldVal.Equals(newVal))
                GD.Print($"Changed {waterField.Name} from {oldVal} to {newVal}");
        }
        ShaderMaterial skyShaderMaterial = environment.Environment.Sky.SkyMaterial as ShaderMaterial;
        foreach (FieldInfo skyField in typeof(WeatherType).GetFields().Where(f => f.GetCustomAttribute<SkyMaterialAttribute>() is not null))
        {
            MethodInfo variantFrom = VariantFromMethod.MakeGenericMethod(skyField.FieldType);
            Variant oldVal = skyShaderMaterial.GetShaderParameter(skyField.Name);
            Variant newVal = (Variant)variantFrom.Invoke(null, [skyField.GetValue(Weather)]);
            skyShaderMaterial?.SetShaderParameter(skyField.Name, newVal);
            //if (!oldVal.Equals(newVal))
            //    GD.Print($"Changed {skyField.Name} from {oldVal} to {newVal}");
        }
        sun.LightEnergy = Weather.light_energy;
        sun.LightColor = Weather.light_color;
        rain.Amount = Weather.rain_amount;
    }
    #endregion
}