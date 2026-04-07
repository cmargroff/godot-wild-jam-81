
using System;

public interface IPauseManager
{
  void Pause();
  void Unpause();
  void Toggle();
  event Action<bool> GamePauseChanged;
}