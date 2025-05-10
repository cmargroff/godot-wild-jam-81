using Godot;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ShipOfTheseus2025.Stores;

public partial class BaseSavedStore
{
  public const string SK = "CHANGEME";
  protected bool Encrypted = false;
  protected ConfigFile _configFile;
  public BaseSavedStore()
  {
    _configFile = new ConfigFile();
    Load();
  }
  private string Filename
  {
    get
    {
      var basename = Regex.Replace(GetType().Name, "[Ss]tore$", "");
      return $"user://{basename}.ini";
    }
  }
  public void Load()
  {
    Error err;
    if (Encrypted)
    {
      err = _configFile.LoadEncryptedPass(Filename, SK);
    }
    else
    {
      err = _configFile.Load(Filename);
    }
    if (err == Error.Ok)
    {
      // bind values from file
    }
    else
    {
      Initialize();
    }
  }
  public void Save()
  {
    if (Encrypted)
    {
      _configFile.SaveEncryptedPass(Filename, SK);
      return;
    }
    _configFile.Save(Filename);
  }
  private void Initialize()
  {
    var thisType = GetType();
    var fields = thisType.GetFields(BindingFlags.Public | BindingFlags.Instance);

    foreach (var field in fields)
    {
      switch (field.FieldType.Name)
      {
        case "Int32":
          _configFile.SetValue("", field.Name, (int)field.GetValue(this));
          break;
        case "String":
          _configFile.SetValue("", field.Name, (string)field.GetValue(this));
          break;
        default:
          break;
      }
    }
    Save();
  }
}
