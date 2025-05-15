using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace ShipOfTheseus2025.Components.Game;

public partial class HoverPage : Control
{
  [Export]
  public PackedScene AttributeItem;
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
  private VBoxContainer _attributesWrap;
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
  }
  private void UpdateViewportSize()
  {
    _viewPortSize = _viewport.GetVisibleRect().Size;
  }
  private void UpdateAttributes(List<object> attributes)
  {
    // attribute nodes pooler
    var childCount = _attributesWrap.GetChildCount();
    var i = 0;
    foreach (var attr in attributes)
    {
      if (i < childCount)
      {
        // update existing attribute nodes
        var attrNode = _attributesWrap.GetChild<ItemPageAttribute>(i);
        attrNode.SetAttribute(attr);
        attrNode.Visible = true;
      }
      else
      {
        // create new attribute nodes for longer attribute list
        var newNode = AttributeItem.Instantiate<ItemPageAttribute>();
        newNode.SetAttribute(attr);
        _attributesWrap.AddChild(newNode);
      }
      i++;
    }
    if (childCount > attributes.Count)
    {
      // hide the extra nodes that are not in use
      for (i = childCount - 1; i > attributes.Count; i--)
      {
        _attributesWrap.GetChild<Control>(i).Visible = false;
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
    // UpdateAttributes(inventoryItem.Attributes);
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
