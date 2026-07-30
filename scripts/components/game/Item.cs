using Godot;
using ShipOfTheseus2025.Components.Game;

public partial class Item : Node3D
{
  const float DROP_ANIMATION_DURATION = 0.3f;
  const float DROP_ANIMATION_SCALE = 0.5f;
  public InventoryItem InventoryItem { get; set; }
  public IItemMover ItemMover { get; set; }
  public IDragHandler DragHandler { get; set; }
  public IHoverHandler HoverHandler { get; set; }
  public Area3D Area { get; private set; }
  public Node3D Visual { get; private set; }
  private Tween _animation;
  private DamagePoint _targetSnapPoint;
  private Vector3 _originalScale;
  private Vector3 _targetScale;
  public override void _EnterTree()
  {
    _originalScale = new Vector3(DROP_ANIMATION_SCALE, DROP_ANIMATION_SCALE, DROP_ANIMATION_SCALE);
    _targetScale = new Vector3(1f, 1f, 1f);
    Visual = GetNode<Node3D>("Visual");
    var area = Visual.GetNode<Area3D>("%Area3D");
    if (area == null)
    {
      GD.PrintErr("Area3D node not found in Item");
      QueueFree();
    }
    Area = area;
    _animation = GetTree().CreateTween();
    _animation.TweenMethod(Callable.From<float>(SnapTween), 0f, 1f, DROP_ANIMATION_DURATION);
    _animation.Pause();
  }
  public void SetAttached(AttachSlotType attachSlotType)
  {
  }
  public void SetDetached()
  {
  }
  public void PlayDropAnimation(DamagePoint damagePoint, bool reversed = false)
  {
    if (damagePoint == null)
    {
      GD.PrintErr("DamagePoint is null in PlayDropAnimation");
      return;
    }
    _targetSnapPoint = damagePoint;
    if (reversed)
    {
      _animation.SetSpeedScale(-1f);
    }
    else
    {
      _animation.SetSpeedScale(1f);
    }
    if (!_animation.IsRunning())
    {
      var t = _animation.GetTotalElapsedTime();
      _animation.Play();
    }
  }
  private void SnapTween(float t)
  {
    Visual.GlobalPosition = GlobalPosition.Lerp(_targetSnapPoint.GlobalPosition, t);
    Visual.Scale = _originalScale.Lerp(_targetScale, t);
  }
  public void Hide()
  {
    Visual.Visible = false;
    Area.Monitoring = false;
    Area.Monitorable = false;
  }
  public void Show()
  {
    Visual.Visible = true;
    Area.Monitoring = true;
    Area.Monitorable = true;
  }
}
