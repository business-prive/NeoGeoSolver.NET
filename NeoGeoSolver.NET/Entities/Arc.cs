using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Arc : Entity
{
  public readonly Point Centre = new();
  public readonly Param Radius = new("Radius");
  public readonly Param StartAngle = new("StartAngle");
  public readonly Param EndAngle = new("EndAngle");

  public Arc(Point centre, Param radius, Param startAngle, Param endAngle)
  {
    Centre = centre;
    Radius = radius;
    StartAngle = startAngle;
    EndAngle = endAngle;
  }
}
