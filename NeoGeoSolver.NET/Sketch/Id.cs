namespace NeoGeoSolver.NET.Sketch;

public struct Id {

  public static readonly Id Null = new Id(0);

  long id;
  long secondId;

  internal Id(long v, long s = 0) {
    id = v;
    secondId = s;
  }

  public long value { get { return id; } }
  public long second { get { return secondId; } }

  public Id WithSecond(long s) {
    return new Id(value, s);
  }

  public Id WithoutSecond() {
    return new Id(value, 0);
  }

  public static bool operator==(Id a, Id b) {
    return a.value == b.value && a.second == b.second;
  }

  public static bool operator!=(Id a, Id b) {
    return a.value != b.value || a.second != b.second;
  }

  public override string ToString() {
    if(second == 0) return value.ToString("X");
    return value.ToString("X") + ":" + second.ToString("X");
  }

  public override int GetHashCode() {
    return (int)value;
  }

  public override bool Equals(object obj) {
    var o = (Id)obj;
    if(o == this) return true;
    return value == o.value && second == o.second;
  }
}