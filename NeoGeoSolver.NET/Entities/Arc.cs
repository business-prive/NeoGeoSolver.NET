using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Arc : Entity
{
  public readonly Point Centre = new();
  public readonly Param StartAngle = new("StartAngle");
  public readonly Param EndAngle = new("EndAngle");

  public Arc(Point centre, Param startAngle, Param endAngle)
  {
    Centre = centre;
    StartAngle = startAngle;
    EndAngle = endAngle;
  }
}
