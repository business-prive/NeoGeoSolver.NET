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
    North = new StartPointDrawer(new Point(Circle.Centre.X.Value, Circle.Centre.Y.Value + Circle.Radius.Value, 0));
    South = new StartPointDrawer(new Point(Circle.Centre.X.Value, Circle.Centre.Y.Value - Circle.Radius.Value, 0));
    East = new StartPointDrawer(new Point(Circle.Centre.X.Value + Circle.Radius.Value, Circle.Centre.Y.Value, 0));
    West = new StartPointDrawer(new Point(Circle.Centre.X.Value - Circle.Radius.Value, Circle.Centre.Y.Value, 0));
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    const double Tolerance = 1e-5;
    var nVecX = North.Point.X.Value - Circle.Centre.X.Value;
    var nVecY = North.Point.Y.Value - Circle.Centre.Y.Value;
    var nVec = Math.Sqrt(nVecX * nVecX + nVecY * nVecY);
    var sVecX = South.Point.X.Value - Circle.Centre.X.Value;
    var sVecY = South.Point.Y.Value - Circle.Centre.Y.Value;
    var sVec = Math.Sqrt(sVecX * sVecX + sVecY * sVecY);
    var eVecX = East.Point.X.Value - Circle.Centre.X.Value;
    var eVecY = East.Point.Y.Value - Circle.Centre.Y.Value;
    var eVec = Math.Sqrt(eVecX * eVecX + eVecY * eVecY);
    var wVecX = West.Point.X.Value - Circle.Centre.X.Value;
    var wVecY = West.Point.Y.Value - Circle.Centre.Y.Value;
    var wVec = Math.Sqrt(wVecX * wVecX + wVecY * wVecY);

    // if Center has moved, then *all* of NSEW will be inside/outside a Rad from Center
    if (!(Math.Abs(Circle.Radius.Value - nVec) > Tolerance) ||
        !(Math.Abs(Circle.Radius.Value - sVec) > Tolerance) ||
        !(Math.Abs(Circle.Radius.Value - eVec) > Tolerance) ||
        !(Math.Abs(Circle.Radius.Value - wVec) > Tolerance))
    {
      // update Rad based on new NSEW
      if (Math.Abs(Circle.Radius.Value - nVec) > Tolerance)
      {
        Circle.Radius.Value = nVec;
      }
      else if (Math.Abs(Circle.Radius.Value - sVec) > Tolerance)
      {
        Circle.Radius.Value = sVec;
      }
      else if (Math.Abs(Circle.Radius.Value - eVec) > Tolerance)
      {
        Circle.Radius.Value = eVec;
      }
      else if (Math.Abs(Circle.Radius.Value - wVec) > Tolerance)
      {
        Circle.Radius.Value = wVec;
      }
    }

    // update NSEW to sit on Circle
    North.Point.X.Value = Circle.Centre.X.Value;
    North.Point.Y.Value = Circle.Centre.Y.Value + Circle.Radius.Value;
    South.Point.X.Value = Circle.Centre.X.Value;
    South.Point.Y.Value = Circle.Centre.Y.Value - Circle.Radius.Value;
    East.Point.X.Value = Circle.Centre.X.Value + Circle.Radius.Value;
    East.Point.Y.Value = Circle.Centre.Y.Value;
    West.Point.X.Value = Circle.Centre.X.Value - Circle.Radius.Value;
    West.Point.Y.Value = Circle.Centre.Y.Value;

    await North.DrawAsync(batch);
    await South.DrawAsync(batch);
    await East.DrawAsync(batch);
    await West.DrawAsync(batch);

    await base.DrawAsyncInternal(batch);
  }
}
