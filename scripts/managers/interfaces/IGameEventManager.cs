using System;

public interface IGameEventManager
{
  public event Action EnvironmentEvent;
  public void QueueEnvironmentEvent();
  public void End();
  public void DispatchEnvironmentEvent();
  public void DispatchItemEvent();
  public void Start();
}