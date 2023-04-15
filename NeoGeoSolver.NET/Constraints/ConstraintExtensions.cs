using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.Constraints;

public static class ConstraintExtensions
{
  public static Constraint IsHorizontal(this Line line) => new LineHorizontalVertical(line, HorizontalVerticalOrientation.Oy);
  public static Constraint IsVertical(this Line line) => new LineHorizontalVertical(line, HorizontalVerticalOrientation.Ox);
  
  public static Constraint IsTangentTo(this Line line, Circle circle)
  {
    var constr = new LineCircleDistance(line, circle);
    constr.SetValue(0);
    return constr;
  }
  
  public static Constraint IsTangentTo(this Line line, Arc arc)
  {
    var constr = new LineArcDistance(line, arc);
    constr.SetValue(0);
    return constr;
  }

  public static Constraint IsPerpendicularTo(this Line line0, Line line1) => new Perpendicular(line0, line1);
  public static Constraint IsParallelTo(this Line line0, Line line1) => new Parallel(line0, line1);
  public static Constraint IsCollinearTo(this Line line0, Line line1) => new LinesCollinear(line0, line1);
  public static Constraint IsEqualInLengthTo(this Line line0, Line line1) => new LinesEqualLength(line0, line1);

  public static Constraint IsCoincidentWithConstraint(this Point pt0, Point pt1) => new PointsCoincident(pt0, pt1);
}
