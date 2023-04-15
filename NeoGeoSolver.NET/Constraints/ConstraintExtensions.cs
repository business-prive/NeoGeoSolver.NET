using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

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
  public static Constraint IsCoincidentWith(this Point pt0, Line line)
  {
    var cons = new PointLineDistance(pt0, line);
    cons.SetValue(0);
    return cons;
  }
  public static Constraint IsCoincidentWith(this Point pt0, Circle circle)
  {
    var cons = new PointCircleDistance(pt0, circle);
    cons.SetValue(0);
    return cons;
  }
  public static Constraint IsCoincidentWith(this Point pt0, Arc arc)
  {
    var cons = new PointArcDistance(pt0, arc);
    cons.SetValue(0);
    return cons;
  }
  
  public static Constraint IsConcentricWith(this Circle circle0, Circle circle1) => new CirclesConcentric(circle0, circle1);
  public static Constraint IsEqualInRadiusTo(this Circle circle0, Circle circle1) => new CirclesEqualRadius(circle0, circle1);
  public static Constraint IsConcentricWith(this Circle circle, Arc arc1) => new ArcCircleConcentric(arc1, circle);
  public static Constraint IsEqualInRadiusTo(this Circle circle, Arc arc1) => new ArcCircleEqualRadius(arc1, circle);
  public static Constraint IsConcentricWith(this Arc arc0, Arc arc1) => new ArcsConcentric(arc1, arc0);
  public static Constraint IsEqualInRadiusTo(this Arc arc0, Arc arc1) => new ArcsEqualRadius(arc1, arc0);

  public static Constraint HasInternalAngle(this Line line0, Line line1, double angle)
  {
    var cons = new LinesAngle(line0, line1);
    cons.SetValue(angle);
    return cons;
  }
  public static Constraint HasExternalAngle(this Line line0, Line line1, double angle)
  {
    var cons = new LinesAngle(line0, line1)
    {
      Supplementary = true
    };
    cons.SetValue(angle);
    return cons;
  }

  public static Constraint HasDistance(this Point pt0, Point pt1, int dist)
  {
    var cons = new PointsDistance(pt0, pt1);
    cons.SetValue(dist);
    return cons;
  }
  public static Constraint HasDistance(this Point pt, Line line, int dist)
  {
    var cons = new PointLineDistance(pt, line);
    cons.SetValue(dist);
    return cons;
  }

  public static Constraint HasRadius(this Circle circle, int radius)
  {
    var cons = new CircleDiameter(circle);
    cons.SetValue(2 * radius);
    return cons;
  }
  public static Constraint HasRadius(this Arc arc, int radius)
  {
    var cons = new ArcRadius(arc);
    cons.SetValue(radius);
    return cons;
  }
}
