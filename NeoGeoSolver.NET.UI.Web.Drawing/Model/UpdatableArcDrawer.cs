using System;
using System.Numerics;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Drawing.Model;

public sealed class UpdatableArcDrawer : ArcDrawer
{
  public UpdatableArcDrawer(Arc arc) :
    base(arc)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    // update StartAngle+EndAngle based on new Start+End
    var startAngle = Math.Atan2(Start.Point.y.Value - Centre.Point.y.Value, Start.Point.x.Value - Centre.Point.x.Value);
    var endAngle = Math.Atan2(End.Point.y.Value - Centre.Point.y.Value, End.Point.x.Value - Centre.Point.x.Value);
    Arc.StartAngle.Value = startAngle;
    Arc.EndAngle.Value = endAngle;

    // update Rad as Start or End might have changed
    const double Tolerance = 1e-5;
    var startRadVec = Start.Point - Centre.Point;
    var startRad = startRadVec.Length;
    var endRadVec = End.Point - Centre.Point;
    var endRad = endRadVec.Length;
    if (Math.Abs(Arc.Rad.Value - startRad) > Tolerance)
    {
      Arc.Rad.Value = startRad;
    }
    else if (Math.Abs(Arc.Rad.Value - endRad) > Tolerance)
    {
      Arc.Rad.Value = endRad;
    }

    // update Start+End to sit on Arc
    var startVec = new Vector(Math.Cos(Arc.StartAngle.Value), Math.Sin(Arc.StartAngle.Value));
    var startPt = Arc.Center + Arc.Rad.Value * startVec;
    Start.Point.x.Value = startPt.x.Value;
    Start.Point.y.Value = startPt.y.Value;

    var endVec = new Vector(Math.Cos(Arc.EndAngle.Value), Math.Sin(Arc.EndAngle.Value));
    var endPt = Arc.Center + Arc.Rad.Value * endVec;
    End.Point.x.Value = endPt.x.Value;
    End.Point.y.Value = endPt.y.Value;

    await base.DrawAsyncInternal(batch);
  }
}
