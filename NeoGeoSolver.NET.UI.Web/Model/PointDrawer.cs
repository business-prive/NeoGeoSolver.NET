using System.Collections.Generic;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Drawing.Model;

public abstract class PointDrawer : EntityDrawer
{
  public Point Point { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new [] { this };
  public override object Entity => Point;

  public override bool IsNear(System.Drawing.Point pt)
  {
    return Point.IsNear(pt);
  }

  public PointDrawer(Point point)
  {
    Point = point;
  }
}
