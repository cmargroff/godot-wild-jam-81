using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Stores;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Managers;

public partial class AudioManager : Node
{
    private SettingsStore _settings;

    private int _masterBusIndex = 0;
    private int _sfxBusIndex;
    private int _bgmBusIndex;
    private int _voiceBusIndex;

    [FromServices]
    public void Inject(SettingsStore settings)
    {
        _settings = settings;
        _sfxBusIndex = AudioServer.GetBusIndex("SFX");
        _bgmBusIndex = AudioServer.GetBusIndex("BGM");
        _voiceBusIndex = AudioServer.GetBusIndex("Voice");
    }

    public float MainVol
    {
        get => _settings.MainVol;
        set
        {
            _settings.MainVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(_masterBusIndex, _settings.MainVol);
            _settings.Save();
        }
    }

    public float SFXVol
    {
        get => _settings.SFXVol;
        set
        {
            _settings.SFXVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(_sfxBusIndex, _settings.SFXVol);
            _settings.Save();
        }
    }

    public float BGMVol
    {
        get => _settings.BGMVol;
        set
        {
            _settings.BGMVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(_bgmBusIndex, _settings.BGMVol);
            _settings.Save();
        }
    }

    public float VoiceVol
    {
        get => _settings.VoiceVol;
        set
        {
            _settings.VoiceVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(_voiceBusIndex, _settings.VoiceVol);
            _settings.Save();
        }
    }
}