using System.Globalization;

namespace NeoGeoSolver.NET.Sketch;

public class IdGenerator {
  long maxId = 0;

  public Id New() {
    return new Id(++maxId);
  }

  public Id Create(long id) {
    maxId = Math.Max(maxId, id);
    return new Id(id);
  }

  public Id Create(string str) {
    long id = long.Parse(str, NumberStyles.HexNumber);
    maxId = Math.Max(maxId, id);
    return new Id(id);
  }

  public void Clear() {
    maxId = 0;
  }
}