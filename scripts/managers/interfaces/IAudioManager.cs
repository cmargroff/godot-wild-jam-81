using System;
using Godot;

public interface IAudioManager
{
  float MainVol { get; set; }
  float SFXVol { get; set; }
  float BGMVol { get; set; }
  float VoiceVol { get; set; }
  AudioStreamPlayer PlayGlobalAudio(AudioStream audio, string busName, Node parentNode, Action<AudioStreamPlayer> options = null, Action onFinished = null);
  (AudioStreamPlayer, Action) PlayGlobalAudioOnRepeat(AudioStream audio, string busName, Node parentNode, FloatRange delay, bool playImmediately = false, Action<AudioStreamPlayer> options = null, Action onFinished = null);
  AudioStreamPlayer2D PlayAudio2D(AudioStream audio, string busName, Node parentNode, Action<AudioStreamPlayer2D> options = null, Action onFinished = null);
  (AudioStreamPlayer2D, Action) Play2DAudioOnRepeat(AudioStream audio, string busName, Node parentNode, FloatRange delay, bool playImmediately = false, Action<AudioStreamPlayer2D> options = null, Action onFinished = null);
  AudioStreamPlayer3D PlayAudio3D(AudioStream audio, string busName, Node parentNode, Action<AudioStreamPlayer3D> options = null, Action onFinished = null);
  (AudioStreamPlayer3D, Action) Play3DAudioOnRepeat(AudioStream audio, string busName, Node parentNode, FloatRange delay, bool playImmediately = false, Action<AudioStreamPlayer3D> options = null, Action onFinished = null);
}