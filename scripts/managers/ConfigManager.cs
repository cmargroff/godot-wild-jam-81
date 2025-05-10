
using Godot;

namespace ShipOfTheseus2025.Managers;

public partial class ConfigManager : ConfigFile
{
  public ConfigManager()
  {
    Load("res://config.ini");
  }
}