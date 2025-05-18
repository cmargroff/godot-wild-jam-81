using System;
using Godot;
using ShipOfTheseus2025.Services;
using ShipOfTheseus2025.Stores;
using ShipOfTheseus2025.Util;

namespace ShipOfTheseus2025.Managers;

public partial class AudioManager : Node
{
    private SettingsStore _settings;
    private RandomNumberGeneratorService _rng;

    public int MasterBusIndex { get; private set; } = 0;
    public int SfxBusIndex { get; private set; }
    public int BgmBusIndex { get; private set; }
    public int VoiceBusIndex { get; private set; }

    [FromServices]
    public void Inject(SettingsStore settings, RandomNumberGeneratorService rng)
    {
        _settings = settings;
        _rng = rng;
        SfxBusIndex = AudioServer.GetBusIndex("SFX");
        BgmBusIndex = AudioServer.GetBusIndex("BGM");
        VoiceBusIndex = AudioServer.GetBusIndex("Voice");
    }

    public float MainVol
    {
        get => _settings.MainVol;
        set
        {
            _settings.MainVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(MasterBusIndex, _settings.MainVol);
            _settings.Save();
        }
    }

    public float SFXVol
    {
        get => _settings.SFXVol;
        set
        {
            _settings.SFXVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(SfxBusIndex, _settings.SFXVol);
            _settings.Save();
        }
    }

    public float BGMVol
    {
        get => _settings.BGMVol;
        set
        {
            _settings.BGMVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(BgmBusIndex, _settings.BGMVol);
            _settings.Save();
        }
    }

    public float VoiceVol
    {
        get => _settings.VoiceVol;
        set
        {
            _settings.VoiceVol = value;
            AudioServer.Singleton.SetBusVolumeLinear(VoiceBusIndex, _settings.VoiceVol);
            _settings.Save();
        }
    }

    #region global audio
    /// <summary>
    /// Play an audio stream. 
    /// </summary>
    /// <param name="audio">The audio stream to play.</param>
    /// <param name="busName">The name of the bus that this audio should play on. Use "Master", "SFX", "BGM", or "Voice".</param>
    /// <param name="parentNode">The node to attach the audio player and timer to. Use this to make sure the audio stops when the scene ends.</param>
    /// <param name="options">A callback to apply options for the player.</param>
    /// <param name="onFinished">A callback for when the audio finishes playing (if not looped).</param>
    /// <returns>The player that will be playing this audio so that you can stop it before it's finished.</returns>
    public AudioStreamPlayer PlayGlobalAudio(AudioStream audio, string busName, Node parentNode, Action<AudioStreamPlayer> options = null, Action onFinished = null) 
    {
        AudioStreamPlayer player = new()
        {
            Stream = audio,
            Bus = busName
        };
        options?.Invoke(player);
        parentNode.AddChild(player);
        player.Finished += () =>
        {
            player.QueueFree();
            onFinished?.Invoke();
        };
        player.Play();
        return player;
    }

    /// <summary>
    /// Plays an AudioStream (including playlists, randomizers, etc.) on repeat with a given delay using a Timer.
    /// </summary>
    /// <param name="audio">The audio stream to play.</param>
    /// <param name="busName">The name of the bus that this audio should play on. Use "Master", "SFX", "BGM", or "Voice".</param>
    /// <param name="parentNode">The node to attach the audio player and timer to. Use this to make sure the audio stops when the scene ends.</param>
    /// <param name="delay">A range of time in seconds for how long to wait between replays of the audio stream.</param>
    /// <param name="playImmediately">True if the first time should not be delayed, else false.</param>
    /// <param name="options">A callback to apply options just before each play of the audio stream.</param>
    /// <param name="onFinished">A callback after each play of the audio stream is finished.</param>
    /// <returns>The player that will be playing this audio and an action that stops the player as well as the underlying timer.</returns>
    public (AudioStreamPlayer, Action) PlayGlobalAudioOnRepeat(AudioStream audio, string busName, Node parentNode, FloatRange delay, bool playImmediately = false, Action<AudioStreamPlayer> options = null, Action onFinished = null)
    {
        AudioStreamPlayer player = new()
        {
            Stream = audio,
            Bus = busName
        };
        parentNode.AddChild(player);

        Timer delayTimer = null;
        void CreateAndRunTimer(FloatRange delay, Action<AudioStreamPlayer> options, AudioStreamPlayer player)
        {
            if (delayTimer is null)
            {
                delayTimer = CreateNewDelayTimer(delay, player, options);
                player.AddChild(delayTimer);
            }
            delayTimer.Start(_rng.GetFloatRange(delay));
        }

        player.Finished += () =>
        {
            onFinished?.Invoke();
            CreateAndRunTimer(delay, options, player);
        };
        if (playImmediately)
        {
            options?.Invoke(player);
            player.Play();
        }
        else
        {
            CreateAndRunTimer(delay, options, player);
        }
        return (player, () =>
        {
            player.Stop();
            Timer timer = player.GetChild<Timer>(0);
            timer?.Stop();
            player.QueueFree();
        });
    }

    private Timer CreateNewDelayTimer(FloatRange delay, AudioStreamPlayer player, Action<AudioStreamPlayer> options = null)
    {
        Timer delayTimer = new()
        {
            OneShot = true,
            Autostart = false,
        };
        delayTimer.Connect(Timer.SignalName.Timeout, Callable.From(() =>
        {
            options?.Invoke(player);
            player.Play();
        }));
        return delayTimer;
    }
    #endregion

    #region 2D Audio
    /// <summary>
    /// Play an audio stream in a 2D context. 
    /// </summary>
    /// <param name="audio">The audio stream to play.</param>
    /// <param name="busName">The name of the bus that this audio should play on. Use "Master", "SFX", "BGM", or "Voice".</param>
    /// <param name="parentNode">The node to attach the audio player and timer to. Use this to make sure the audio stops when the scene ends.</param>
    /// <param name="options">A callback to apply options for the player.</param>
    /// <param name="onFinished">A callback for when the audio finishes playing (if not looped).</param>
    /// <returns>The player that will be playing this audio so that you can stop it before it's finished.</returns>
    public AudioStreamPlayer2D PlayAudio2D(AudioStream audio, string busName, Node parentNode, Action<AudioStreamPlayer2D> options = null, Action onFinished = null)
    {
        AudioStreamPlayer2D player = new()
        {
            Stream = audio,
            Bus = busName
        };
        options?.Invoke(player);
        parentNode.AddChild(player);
        player.Finished += () =>
        {
            player.QueueFree();
            onFinished?.Invoke();
        };
        player.Play();
        return player;
    }

    /// <summary>
    /// Plays an AudioStream (including playlists, randomizers, etc.) on repeat with a given delay using a Timer.
    /// </summary>
    /// <param name="audio">The audio stream to play.</param>
    /// <param name="busName">The name of the bus that this audio should play on. Use "Master", "SFX", "BGM", or "Voice".</param>
    /// <param name="parentNode">The node to attach the audio player and timer to. Use this to make sure the audio stops when the scene ends.</param>
    /// <param name="delay">A range of time in seconds for how long to wait between replays of the audio stream.</param>
    /// <param name="playImmediately">True if the first time should not be delayed, else false.</param>
    /// <param name="options">A callback to apply options just before each play of the audio stream.</param>
    /// <param name="onFinished">A callback after each play of the audio stream is finished.</param>
    /// <returns>The player that will be playing this audio, and an action that stops the player as well as the underlying timer.</returns>
    public (AudioStreamPlayer2D, Action) Play2DAudioOnRepeat(AudioStream audio, string busName, Node parentNode, FloatRange delay, bool playImmediately = false, Action<AudioStreamPlayer2D> options = null, Action onFinished = null)
    {
        AudioStreamPlayer2D player = new()
        {
            Stream = audio,
            Bus = busName
        };
        parentNode.AddChild(player);

        Timer delayTimer = null;
        void CreateAndRunTimer2D(FloatRange delay, Action<AudioStreamPlayer2D> options, AudioStreamPlayer2D player)
        {
            if (delayTimer is null)
            {
                delayTimer = CreateNewDelayTimer2D(delay, player, options);
                player.AddChild(delayTimer);
            }
            delayTimer.Start(_rng.GetFloatRange(delay));
        }

        player.Finished += () =>
        {
            onFinished?.Invoke();
            CreateAndRunTimer2D(delay, options, player);
        };
        if (playImmediately)
        {
            options?.Invoke(player);
            player.Play();
        }
        else
        {
            CreateAndRunTimer2D(delay, options, player);
        }
        return (player, () =>
        {
            player.Stop();
            Timer timer = player.GetChild<Timer>(0);
            timer?.Stop();
            player.QueueFree();
        });
    }

    private Timer CreateNewDelayTimer2D(FloatRange delay, AudioStreamPlayer2D player, Action<AudioStreamPlayer2D> options = null)
    {
        Timer delayTimer = new()
        {
            OneShot = true,
            Autostart = false,
        };
        delayTimer.Connect(Timer.SignalName.Timeout, Callable.From(() =>
        {
            options?.Invoke(player);
            player.Play();
        }));
        return delayTimer;
    }
    #endregion

    #region 3D Audio
    /// <summary>
    /// Play an audio stream in a 2D context. 
    /// </summary>
    /// <param name="audio">The audio stream to play.</param>
    /// <param name="busName">The name of the bus that this audio should play on. Use "Master", "SFX", "BGM", or "Voice".</param>
    /// <param name="parentNode">The node to attach the audio player and timer to. Use this to make sure the audio stops when the scene ends.</param>
    /// <param name="options">A callback to apply options for the player.</param>
    /// <param name="onFinished">A callback for when the audio finishes playing (if not looped).</param>
    /// <returns>The player that will be playing this audio so that you can stop it before it's finished.</returns>
    public AudioStreamPlayer3D PlayAudio3D(AudioStream audio, string busName, Node parentNode, Action<AudioStreamPlayer3D> options = null, Action onFinished = null)
    {
        AudioStreamPlayer3D player = new()
        {
            Stream = audio,
            Bus = busName
        };
        options?.Invoke(player);
        parentNode.AddChild(player);
        player.Finished += () =>
        {
            player.QueueFree();
            onFinished?.Invoke();
        };
        player.Play();
        return player;
    }

    /// <summary>
    /// Plays an AudioStream (including playlists, randomizers, etc.) on repeat with a given delay using a Timer.
    /// </summary>
    /// <param name="audio">The audio stream to play.</param>
    /// <param name="busName">The name of the bus that this audio should play on. Use "Master", "SFX", "BGM", or "Voice".</param>
    /// <param name="parentNode">The node to attach the audio player and timer to. Use this to make sure the audio stops when the scene ends.</param>
    /// <param name="delay">A range of time in seconds for how long to wait between replays of the audio stream.</param>
    /// <param name="playImmediately">True if the first time should not be delayed, else false.</param>
    /// <param name="options">A callback to apply options just before each play of the audio stream.</param>
    /// <param name="onFinished">A callback after each play of the audio stream is finished.</param>
    /// <returns>The player that will be playing this audio, and an action that stops the player as well as the underlying timer.</returns>
    public (AudioStreamPlayer3D, Action) Play3DAudioOnRepeat(AudioStream audio, string busName, Node parentNode, FloatRange delay, bool playImmediately = false, Action<AudioStreamPlayer3D> options = null, Action onFinished = null)
    {
        AudioStreamPlayer3D player = new()
        {
            Stream = audio,
            Bus = busName
        };
        parentNode.AddChild(player);

        Timer delayTimer = null;
        void CreateAndRunTimer3D(FloatRange delay, Action<AudioStreamPlayer3D> options, AudioStreamPlayer3D player)
        {
            if (delayTimer is null)
            {
                delayTimer = CreateNewDelayTimer3D(delay, player, options);
                player.AddChild(delayTimer);
            }
            delayTimer.Start(_rng.GetFloatRange(delay));
        }

        player.Finished += () =>
        {
            onFinished?.Invoke();
            CreateAndRunTimer3D(delay, options, player);
        };
        if (playImmediately)
        {
            options?.Invoke(player);
            player.Play();
        }
        else
        {
            CreateAndRunTimer3D(delay, options, player);
        }
        return (player, () =>
        {
            player.Stop();
            Timer timer = player.GetChild<Timer>(0);
            timer?.Stop();
            player.QueueFree();
        });
    }

    private Timer CreateNewDelayTimer3D(FloatRange delay, AudioStreamPlayer3D player, Action<AudioStreamPlayer3D> options = null)
    {
        Timer delayTimer = new()
        {
            OneShot = true,
            Autostart = false,
        };
        delayTimer.Connect(Timer.SignalName.Timeout, Callable.From(() =>
        {
            options?.Invoke(player);
            player.Play();
        }));
        return delayTimer;
    }
    #endregion
}