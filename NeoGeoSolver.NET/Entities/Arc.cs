namespace NeoGeoSolver.NET.Entities;

public class Arc : Entity
{
  public readonly Point Centre = new();
  public readonly Point Point0 = new();
  public readonly Point Point1 = new();

  public Arc(Point centre, Point pt0, Point pt1)
  {
    Centre = centre;
    Point0 = pt0;
    Point1 = pt1;
  }
}
