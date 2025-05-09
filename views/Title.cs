using System;
using System.IO;
using Godot;
using JamTemplate.Managers;
using JamTemplate.Util;

public partial class Title : Control
{
  private Control _menu;
  private Control _options;
  private bool _optionsShown = false;
  private AudioManager _audio;
  private SceneManager _sceneManager;
  [FromServices]
  public void Inject(AudioManager audio, SceneManager sceneManager)
  {
    _audio = audio;
    _sceneManager = sceneManager;
  }
  public override void _EnterTree()
  {
    _menu = GetNode<Control>("%Menu");
    _options = GetNode<Control>("%Options");
    BindSliders();
  }

  public void BindSliders()
  {
    SliderBinding[] bindings = [
      new (){ NodePath = "%MainVolumeSlider", PropertyName = "MainVol" },
      new (){ NodePath = "%BGMVolumeSlider", PropertyName = "BGMVol" },
      new (){ NodePath = "%SFXVolumeSlider", PropertyName = "SFXVol" },
      new (){ NodePath = "%VoiceVolumeSlider", PropertyName = "VoiceVol" },
    ];

    var audioManagerType = _audio.GetType();

    foreach (var binding in bindings)
    {
      var slider = GetNode<Slider>(binding.NodePath);
      var property = audioManagerType.GetProperty(binding.PropertyName);
      slider.Value = (float)property.GetValue(_audio) * 100;
      slider.Connect(Slider.SignalName.DragEnded, Callable.From<bool>((val_changed) =>
      {
        if (val_changed)
          property.SetValue(_audio, (float)slider.Value / 100);
      }));
    }
  }

  public void Start()
  {
    _sceneManager.ChangeScene("Game");
  }
  public void ToggleOptions()
  {
    _menu.Visible = _optionsShown;
    _optionsShown = !_optionsShown;
    _options.Visible = _optionsShown;
  }
  public void Credits()
  {
    _sceneManager.ChangeScene("Credits");
  }
  public void Quit()
  {
    GetTree().Quit();
  }
  private class SliderBinding
  {
    public string NodePath;
    public string PropertyName;
  }
}
