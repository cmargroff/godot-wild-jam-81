using Godot;
using JamTemplate.Managers;
using JamTemplate.Resources;
using JamTemplate.Util;

namespace JamTemplate.Components;
public partial class WildCard : TextureRect
{
  [Export]
  public SplashWildcard CardRes;
  private string _jamNumber = "";
  private string _jamTheme = "";
  [FromServices]
  public void Inject(ConfigManager config)
  {
    _jamNumber = (string)config.GetValue("game", "GWJ_NUMBER");
    _jamTheme = (string)config.GetValue("game", "GWJ_THEME");
  }
  public override void _EnterTree()
  {
    GetNode<Label>("%JamNumber").Text = $"Jam {_jamNumber}";
    GetNode<Label>("%JamTheme").Text = $"{_jamTheme}";
    if (CardRes is not null)
    {
      if (!CardRes.Enabled)
      {
        Modulate = new Color(1, 1, 1, 0.3f);
      }
      GetNode<Label>("%Name").Text = CardRes.Title;
      GetNode<Label>("%Description").Text = CardRes.Description;
    }
  }
}
