using Godot;
using ShipOfTheseus2025.Components.Game;

public partial class Item : Node3D
{
  const float DROP_ANIMATION_DURATION = 1 / 0.3f;
  const float DROP_ANIMATION_SCALE = 0.5f;
  public InventoryItem InventoryItem { get; set; }
  public IItemMover ItemMover { get; set; }
  public IDragHandler DragHandler { get; set; }
  public IHoverHandler HoverHandler { get; set; }
  public Area3D Area { get; private set; }
  public Node3D Visual { get; private set; }
  private DamagePoint _targetSnapPoint;
  private Vector3 _originalScale;
  private Vector3 _targetScale;
  private bool _isAnimating = false;
  private float _animationProgress = 0f;
  private float _animationSpeed = 1f;
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
      return;
    }
    _targetSnapPoint = damagePoint;
    if (reversed)
    {
      _animationSpeed = -1f;
    }
    else
    {
      _animationSpeed = 1f;
    }
    _isAnimating = true;
  }
  public override void _Process(double delta)
  {
    if (_isAnimating)
    {
      _animationProgress += (float)delta * DROP_ANIMATION_DURATION * _animationSpeed;
      if (_animationProgress >= 1f || _animationProgress <= 0f)
      {
        _animationProgress = Mathf.Clamp(_animationProgress, 0f, 1f);
        _isAnimating = false;
      }
      SnapTween(_animationProgress);
    }
    else if (_animationProgress >= 1f)
    {
      // TODO: this is a really bad band-aid, needs to be updated to make the visual a child of the drop point
      Visual.GlobalPosition = _targetSnapPoint.GlobalPosition;
    }
  }
  private void SnapTween(float t)
  {
    Visual.GlobalPosition = GlobalPosition.Lerp(_targetSnapPoint.GlobalPosition, t);
    Visual.Scale = _originalScale.Lerp(_targetScale, t);
  }
  public new void Hide()
  {
    Visual.Visible = false;
    Area.Monitoring = false;
    Area.Monitorable = false;
  }
  public new void Show()
  {
    Visual.Visible = true;
    Area.Monitoring = true;
    Area.Monitorable = true;
  }
  public void ResetVisual()
  {
    Visual.GlobalPosition = Vector3.Zero;
    Visual.Scale = Vector3.One;
  }
}
