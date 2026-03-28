using System;

public interface IManagedScene
{
  public event Action<string> SceneClosing;
}