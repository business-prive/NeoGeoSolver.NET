using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.Constraints;

public static class ConstraintExtensions
{
  public static LineHorizontalVertical IsHorizontal(this Line line) => new LineHorizontalVertical(line, HorizontalVerticalOrientation.Oy);
  public static LineHorizontalVertical IsVertical(this Line line) => new LineHorizontalVertical(line, HorizontalVerticalOrientation.Ox);
  
  public static LineCircleDistance IsTangentTo(this Line line, Circle circle)
  {
    var constr = new LineCircleDistance(line, circle);
    constr.SetValue(0);
    return constr;
  }
  
  public static LineArcDistance IsTangentTo(this Line line, Arc arc)
  {
    var constr = new LineArcDistance(line, arc);
    constr.SetValue(0);
    return constr;
  }

  public static Perpendicular IsPerpendicularTo(this Line line0, Line line1) => new Perpendicular(line0, line1);

  public static PointsCoincident IsCoincidentWithConstraint(this Point pt0, Point pt1) => new PointsCoincident(pt0, pt1);
}
