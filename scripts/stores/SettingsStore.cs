using Godot;

namespace ShipOfTheseus2025.Stores;

public partial class SettingsStore : BaseSavedStore
{
    public float MainVol
    {
        get => GetValue("MainVol");
        set => SetValue("MainVol", value);
    }

    public float SFXVol
    {
        get => GetValue("SFXVol"); 
        set => SetValue("SFXVol", value);
    }

    public float BGMVol
    {
        get => GetValue("BGMVol"); 
        set => SetValue("BGMVol", value);
    }

    public float VoiceVol
    {
        get => GetValue("VoiceVol");
        set => SetValue("VoiceVol", value);
    }
    
    private float GetValue(string key) => Mathf.Clamp((float)_configFile.GetValue("volume", key, 1f), 0f, 1f);
    private void SetValue(string key, float value) => _configFile.SetValue("volume", key, value);
}
