using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace ShipOfTheseus2025.Components.Game;

public partial class HoverPage : Control
{
  [Export]
  public PackedScene TraitItem;
  [Export]
  public Array<Texture2D> PageTextures = new();
  private Viewport _viewport;
  private TextureRect _bg;
  private Label _name;
  private TextureRect _icon;
  private Label _weightLabel;
  private string _weightFmt;
  private Label _valueLabel;
  private string _valueFmt;
  private RichTextLabel _description;
  private VBoxContainer _traitsWrap;
  private Vector2 _pageSize;
  private Vector2 _viewPortSize;
  public override void _EnterTree()
  {
    _viewport = GetViewport();
    UpdateViewportSize();
    _viewport.Connect(Viewport.SignalName.SizeChanged, Callable.From(UpdateViewportSize));
    _bg = GetNode<TextureRect>("%BG");
    _pageSize = _bg.Size * _bg.Scale;
    _name = GetNode<Label>("%Name");
    _icon = GetNode<TextureRect>("%Icon");
    _weightLabel = GetNode<Label>("%WeightLabel");
    _weightFmt = _weightLabel.Text;
    _valueLabel = GetNode<Label>("%ValueLabel");
    _valueFmt = _valueLabel.Text;
    _description = GetNode<RichTextLabel>("%Description");
    _traitsWrap = GetNode<VBoxContainer>("%TraitsWrap");
  }
  private void UpdateViewportSize()
  {
    _viewPortSize = _viewport.GetVisibleRect().Size;
  }
  private void UpdateTraits(List<ItemTrait> traits)
  {
    // attribute nodes pooler
    var childCount = _traitsWrap.GetChildCount();
    var i = 0;
    foreach (var trait in traits)
    {
      if (i < childCount)
      {
        // update existing attribute nodes
        var traitNode = _traitsWrap.GetChild<ItemPageTrait>(i);
        traitNode.SetTrait(trait);
        traitNode.Visible = true;
      }
      else
      {
        // create new attribute nodes for longer attribute list
        var newNode = TraitItem.Instantiate<ItemPageTrait>();
        _traitsWrap.AddChild(newNode);
        newNode.SetTrait(trait);
      }
      i++;
    }
    if (childCount > traits.Count)
    {
      // hide the extra nodes that are not in use
      for (i = childCount - 1; i > traits.Count; i--)
      {
        _traitsWrap.GetChild<Control>(i).Visible = false;
      }
    }
  }
  private void AssignData(InventoryItem inventoryItem)
  {
    // assign a page bg color based on the hash of the item object;
    if (PageTextures.Count > 0)
    {
      var bgIdx = inventoryItem.GetHashCode() % PageTextures.Count; //here bug
      _bg.Texture = PageTextures[bgIdx];
    }

    _name.Text = inventoryItem.Name;
    _icon.Texture = inventoryItem.IconTexture;
    _description.Text = inventoryItem.Description;
    _weightLabel.Text = string.Format(_weightFmt, inventoryItem.Weight);
    _valueLabel.Text = string.Format(_valueFmt, inventoryItem.GoldValue);
    UpdateTraits(inventoryItem.Traits);
  }
  public void Show(InventoryItem inventoryItem)
  {
    AssignData(inventoryItem);
    Visible = true;
  }

  public override void _Process(double delta)
  {
    // move to mouse position
    if (Visible)
    {
      var nextPos = _viewport.GetMousePosition();
      if (nextPos.Y + _pageSize.Y > _viewPortSize.Y)
      {
        nextPos.Y -= _pageSize.Y;
      }
      if (nextPos.X + _pageSize.X > _viewPortSize.X)
      {
        nextPos.X -= _pageSize.X;
      }
      Position = nextPos;
    }
  }
}
