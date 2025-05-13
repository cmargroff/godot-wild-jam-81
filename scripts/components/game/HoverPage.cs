using System.Collections.Generic;
using Godot;
using Godot.Collections;

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
  private RichTextLabel _description;
  private VBoxContainer _attributesWrap;
  public override void _EnterTree()
  {
    _viewport = GetViewport();
    _bg = GetNode<TextureRect>("%BG");
    _name = GetNode<Label>("%Name");
    _icon = GetNode<TextureRect>("%Icon");
    _description = GetNode<RichTextLabel>("%Description");
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
  private void AssignData(object inventoryItem)
  {
    // assign a page bg color based on the hash of the item object;
    if (PageTextures.Count > 0)
    {
      var bgIdx = inventoryItem.GetHashCode() % PageTextures.Count;
      _bg.Texture = PageTextures[bgIdx];
    }

    // _name.Text = inventoryItem.ItemName;
    // _icon.Texture = inventoryItem.IconTexture;
    // _description.Text = inventoryItem.Description;
    // UpdateAttributes(inventoryItem.Attributes);
  }
  public void Show(object inventoryItem)
  {
    AssignData(inventoryItem);
    Visible = true;
  }

  public override void _Process(double delta)
  {
    // move to mouse position
    if (Visible)
    {
      Position = _viewport.GetMousePosition();
    }
  }
}
