namespace NeoGeoSolver.NET.Utils;

public static class SystemExt
{
  public static void Swap<T>(ref T a, ref T b)
  {
    (a, b) = (b, a);
  }
}
