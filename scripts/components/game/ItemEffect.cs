using System;

public class ItemEffect
{
  public string Description { get; set; }
  public Action ItemStored { get; set; }
  public Action ItemRemovedFromStorage { get; set; }
  public Action ItemAttached { get; set; }
  public Action ItemDetached { get; set; }
  public Action ItemDropped { get; set; }
  public Action ItemPickedUp { get; set; }
  public Action EventStarted { get; set; }
  public Action EnemyAttacking { get; set; }
}