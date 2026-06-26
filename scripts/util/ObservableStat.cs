using System;
using ShipOfTheseus2025.Enum;

public class ObservableStat
{
  public Stat Type { get; private set; }
  private float _value;
  public float Value
  {
    get => _value;
    set
    {
      if (_value != value)
      {
        _value = value;
        OnChanged?.Invoke(_value);
      }
    }
  }
  public ObservableStat(Stat type, float initialValue)
  {
    Type = type;
    _value = initialValue;
  }
  public event Action<float> OnChanged;
  public static implicit operator float(ObservableStat stat) => stat.Value;
  public override string ToString() => Value.ToString();
}
