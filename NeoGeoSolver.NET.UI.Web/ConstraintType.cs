namespace NeoGeoSolver.NET.UI.Web;

public enum ConstraintType
{
  // one or more points
  Fixed,
  Free,

  // one or more lines
  Horizontal,
  Vertical,
  LineLength,

  // exactly two lines
  Parallel,
  Perpendicular,
  Collinear,
  EqualLength,

  // one line + one circle
  // one line + one arc
  Tangent,

  // exactly two points
  // one point + one line
  // one point + one circle
  // one point + one arc
  Coincident,

  // one point + one line
  // one point + one circle
  // one point + one arc
  CoincidentMidPoint,

  // exactly two circles
  // exactly two arcs
  // one circle + one arc
  Concentric,
  EqualRadius,

  // one or more circles
  // one or more arcs
  RadiusValue,
  
  // exactly two points
  // one point + one line
  Distance,
  DistanceHorizontal,
  DistanceVertical,

  // exactly two lines
  InternalAngle,
  ExternalAngle,
  
  // one point + one circle
  OnQuadrant
}
