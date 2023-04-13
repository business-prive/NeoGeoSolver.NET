using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Model;

public class ArcDrawer : EntityDrawer
{
  public Arc Arc { get; }
  protected StartPointDrawer Start { get; }
  protected CentrePointDrawer Centre { get; }
  protected EndPointDrawer End { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new PointDrawer[] {Start, Centre, End};
  public override object Entity => Arc;

  public override bool IsNear(System.Drawing.Point pt)
  {
    return pt.IsNear(Arc);
  }

  public ArcDrawer(Arc arc)
  {
    Arc = arc;

    var startPtX = Arc.Centre.X.Value + Arc.Radius.Value * Math.Cos(Arc.StartAngle.Value);
    var startPtY = Arc.Centre.Y.Value + Arc.Radius.Value * Math.Sin(Arc.StartAngle.Value);
    var startPt = new Point(startPtX, startPtY, 0);
    var endPtX = Arc.Centre.X.Value + Arc.Radius.Value * Math.Cos(Arc.EndAngle.Value);
    var endPtY = Arc.Centre.Y.Value + Arc.Radius.Value * Math.Sin(Arc.EndAngle.Value);
    var endPt = new Point(endPtX, endPtY, 0);

    // NOTE:  Start+End are now disconnected from the underlying Arc
    //          ie changes to Start+End will not affect underlying Arc
    Start = new StartPointDrawer(startPt);
    Centre = new CentrePointDrawer(Arc.Centre);
    End = new EndPointDrawer(endPt);
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await Centre.DrawAsync(batch);
    await Start.DrawAsync(batch);

    await batch.BeginPathAsync();
    await Initialise(batch);

    var drawCCW = DrawArcCounterClockwise(Arc.StartAngle.Value, Arc.EndAngle.Value);
    await batch.ArcAsync(Arc.Centre.X.Value, Arc.Centre.Y.Value, Arc.Radius.Value, Arc.StartAngle.Value, Arc.EndAngle.Value, drawCCW);
    await batch.StrokeAsync();

    await End.DrawAsync(batch);
  }

  private static bool DrawArcCounterClockwise(double startAngle, double endAngle)
  {
    // Quadrants are defined here:
    //
    //    https://www.w3schools.com/tags/canvas_arc.asp
    //
    //             |
    //          3  |  4
    //        -----------
    //          2  |  1
    //             |
    //
    // However, Math.atan2() returns angles as:
    //
    //                      |
    //         [-pi/2, -pi] |  [0, -pi/2]
    //       ------------------------------
    //         [+pi/2, +pi] |  [0, +pi/2]
    //                      |
    //
    // rather than in the range [0, 2pi]
    //
    // This leads to chirality issues when the endpoint crosses the X axis
    // and the endAngle flips from -pi to +pi

    // quadrant 1
    if (0 < startAngle && startAngle <= Math.PI / 2)
    {
      if (endAngle + Math.PI < startAngle)
      {
        return false;
      }

      return (endAngle - startAngle) < 0;
    }

    // quadrant 2
    if (Math.PI / 2 < startAngle && startAngle <= Math.PI)
    {
      if (endAngle < Math.PI &&
          endAngle > startAngle)
      {
        return false;
      }

      if (endAngle < startAngle - Math.PI)
      {
        return false;
      }

      return startAngle - endAngle < Math.PI || endAngle < startAngle;
    }

    // quadrant 3
    if (-Math.PI < startAngle && startAngle <= -Math.PI / 2)
    {
      if (endAngle > startAngle + Math.PI)
      {
        return true;
      }

      if (endAngle < startAngle && endAngle < 0)
      {
        return true;
      }

      return false;
    }

    // quadrant 4
    if (-Math.PI / 2 < startAngle && startAngle <= 0)
    {
      if (startAngle + Math.PI > endAngle &&
          endAngle > startAngle)
      {
        return false;
      }

      if (endAngle < startAngle)
      {
        return true;
      }

      return true;
    }

    return default;
  }
}
