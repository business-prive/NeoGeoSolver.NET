using System;
using System.Drawing;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web;

using Point = System.Drawing.Point;

public static class PointExtensions
{
  private const int ToleranceRadius = 16;

  public static bool IsNear(this Point pt, Point other)
  {
    var diff = pt - new Size(other);
    var distSq = diff.X * diff.X + diff.Y * diff.Y;
    return distSq < ToleranceRadius;
  }

  public static bool IsNear(this Entities.Point pt, Point other)
  {
    var dX = pt.X.Value - other.X;
    var dY = pt.Y.Value - other.Y;
    var distSq = dX * dX + dY * dY;
    return distSq < ToleranceRadius;
  }

  public static Point ToDrawing(this Entities.Point pt)
  {
    return new Point((int) pt.X.Value, (int) pt.Y.Value);
  }

  public static Entities.Point ToModel(this Entities.Point pt)
  {
    return new Entities.Point((int) pt.X.Value, (int) pt.Y.Value, 0);
  }

  public static Entities.Point ToModel(this Point pt)
  {
    return new Entities.Point(pt.X, pt.Y, 0);
  }

  public static bool IsNear(this Point pt, Line line)
  {
    var startPt = line.Point0.ToDrawing();
    var endPt = line.Point1.ToDrawing();
    var dist = LineToPointDistance2D(startPt, endPt, pt);
    var distSq = dist * dist;
    return distSq < ToleranceRadius;
  }

  public static bool IsNear(this Point pt, Arc arc)
  {
    //see what the current radius to the point is
    var a1CenterX = arc.Centre.X.Value;
    var a1CenterY = arc.Centre.Y.Value;
    var a1Radius = arc.Radius.Value;
    var a1StartA = arc.StartAngle.Value;
    var a1StartY = a1CenterY + a1Radius * Math.Sin(a1StartA);
    var a1StartX = a1CenterX + a1Radius * Math.Cos(a1StartA);
    var p1X = pt.X;
    var p1Y = pt.Y;
    var rad1 = Hypot(a1CenterX - p1X, a1CenterY - p1Y);
    var rad2 = Hypot(a1CenterX - a1StartX, a1CenterY - a1StartY);
    //Compare this radius to the radius of the circle, return the error squared
    var dist = rad1 - rad2;
    var distSq = dist * dist;
    return distSq < ToleranceRadius;
  }

  private static double Hypot(double a, double b)
  {
    return Math.Sqrt(a * a + b * b);
  }

  #region Point to Line distance

  // stolen from:
  //    https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
  private static double DotProduct(Point pointA, Point pointB, Point pointC)
  {
    var AB = new Point();
    var BC = new Point();
    AB.X = pointB.X - pointA.X;
    AB.Y = pointB.Y - pointA.Y;
    BC.X = pointC.X - pointB.X;
    BC.Y = pointC.Y - pointB.Y;
    var dot = AB.X * BC.X + AB.Y * BC.Y;

    return dot;
  }

  //Compute the cross product AB x AC
  private static double CrossProduct(Point pointA, Point pointB, Point pointC)
  {
    var AB = new Point();
    var AC = new Point();
    AB.X = pointB.X - pointA.X;
    AB.Y = pointB.Y - pointA.Y;
    AC.X = pointC.X - pointA.X;
    AC.Y = pointC.Y - pointA.Y;
    var cross = AB.X * AC.Y - AB.Y * AC.X;

    return cross;
  }

  //Compute the distance from A to B
  private static double Distance(Point pointA, Point pointB)
  {
    var dX = pointA.X - pointB.X;
    var dY = pointA.Y - pointB.Y;

    return Math.Sqrt(dX * dX + dY * dY);
  }

  //Compute the distance from AB to C
  //if isSegment is true, AB is a segment, not a line.
  private static double LineToPointDistance2D(Point pointA, Point pointB, Point pointC)
  {
    var dist = CrossProduct(pointA, pointB, pointC) / Distance(pointA, pointB);
    var dot1 = DotProduct(pointA, pointB, pointC);
    if (dot1 > 0)
    {
      return Distance(pointB, pointC);
    }

    var dot2 = DotProduct(pointB, pointA, pointC);
    if (dot2 > 0)
    {
      return Distance(pointA, pointC);
    }

    return Math.Abs(dist);
  }

  #endregion
}
