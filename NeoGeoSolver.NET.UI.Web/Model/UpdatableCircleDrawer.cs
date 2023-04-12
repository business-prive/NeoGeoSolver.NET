using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Model;

public sealed class UpdatableCircleDrawer : CircleDrawer
{
  public StartPointDrawer North { get; }
  public StartPointDrawer South { get; }
  public StartPointDrawer East { get; }
  public StartPointDrawer West { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new PointDrawer[] {Centre, North, South, East, West};

  public UpdatableCircleDrawer(Circle circle) :
    base(circle)
  {
    North = new StartPointDrawer(new Point(Circle.Centre.x.Value, Circle.Centre.y.Value + Circle.Radius.Value, 0));
    South = new StartPointDrawer(new Point(Circle.Centre.x.Value, Circle.Centre.y.Value - Circle.Radius.Value, 0));
    East = new StartPointDrawer(new Point(Circle.Centre.x.Value + Circle.Radius.Value, Circle.Centre.y.Value, 0));
    West = new StartPointDrawer(new Point(Circle.Centre.x.Value - Circle.Radius.Value, Circle.Centre.y.Value, 0));
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    const double Tolerance = 1e-5;
    var nVec = North.Point - Circle.Centre;
    var sVec = South.Point - Circle.Centre;
    var eVec = East.Point - Circle.Centre;
    var wVec = West.Point - Circle.Centre;

    // if Center has moved, then *all* of NSEW will be inside/outside a Rad from Center
    if (!(Math.Abs(Circle.Radius.Value - nVec.Length) > Tolerance) ||
        !(Math.Abs(Circle.Radius.Value - sVec.Length) > Tolerance) ||
        !(Math.Abs(Circle.Radius.Value - eVec.Length) > Tolerance) ||
        !(Math.Abs(Circle.Radius.Value - wVec.Length) > Tolerance))
    {
      // update Rad based on new NSEW
      if (Math.Abs(Circle.Radius.Value - nVec.Length) > Tolerance)
      {
        Circle.Radius.Value = nVec.Length;
      }
      else if (Math.Abs(Circle.Radius.Value - sVec.Length) > Tolerance)
      {
        Circle.Radius.Value = sVec.Length;
      }
      else if (Math.Abs(Circle.Radius.Value - eVec.Length) > Tolerance)
      {
        Circle.Radius.Value = eVec.Length;
      }
      else if (Math.Abs(Circle.Radius.Value - wVec.Length) > Tolerance)
      {
        Circle.Radius.Value = wVec.Length;
      }
    }

    // update NSEW to sit on Circle
    North.Point.x.Value = Circle.Centre.x.Value;
    North.Point.y.Value = Circle.Centre.y.Value + Circle.Radius.Value;
    South.Point.x.Value = Circle.Centre.x.Value;
    South.Point.y.Value = Circle.Centre.y.Value - Circle.Radius.Value;
    East.Point.x.Value = Circle.Centre.x.Value + Circle.Radius.Value;
    East.Point.y.Value = Circle.Centre.y.Value;
    West.Point.x.Value = Circle.Centre.x.Value - Circle.Radius.Value;
    West.Point.y.Value = Circle.Centre.y.Value;

    await North.DrawAsync(batch);
    await South.DrawAsync(batch);
    await East.DrawAsync(batch);
    await West.DrawAsync(batch);

    await base.DrawAsyncInternal(batch);
  }
}
