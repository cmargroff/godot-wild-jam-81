using Godot;
using ShipOfTheseus2025.Stores;

namespace ShipOfTheseus2025.Managers;

public class AudioManager
{
  private SettingsStore _settings;
  public AudioManager(SettingsStore settings)
  {
    _settings = settings;
  }
  public float MainVol
  {
    get
    {
      return _settings.MainVol;
    }
    set
    {
      _settings.MainVol = value;
      AudioServer.Singleton.SetBusVolumeLinear(0, _settings.MainVol);
      _settings.Save();
    }
  }
  public float SFXVol
  {
    get
    {
      return _settings.SFXVol;
    }
    set
    {
      AudioServer.Singleton.SetBusVolumeLinear(1, _settings.SFXVol);
      _settings.SFXVol = value;
      _settings.Save();
    }
  }
  public float BGMVol
  {
    get
    {
      return _settings.BGMVol;
    }
    set
    {
      AudioServer.Singleton.SetBusVolumeLinear(1, _settings.BGMVol);
      _settings.BGMVol = value;
      _settings.Save();
    }
  }
  public float VoiceVol
  {
    get
    {
      return _settings.VoiceVol;
    }
    set
    {
      AudioServer.Singleton.SetBusVolumeLinear(1, _settings.VoiceVol);
      _settings.VoiceVol = value;
      _settings.Save();
    }
  }
}