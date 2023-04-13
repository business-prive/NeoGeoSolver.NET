using System;
using System.Numerics;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Model;

public sealed class UpdatableArcDrawer : ArcDrawer
{
  public UpdatableArcDrawer(Arc arc) :
    base(arc)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    // update StartAngle+EndAngle based on new Start+End
    var startAngle = Math.Atan2(Start.Point.Y.Value - Centre.Point.Y.Value, Start.Point.X.Value - Centre.Point.X.Value);
    var endAngle = Math.Atan2(End.Point.Y.Value - Centre.Point.Y.Value, End.Point.X.Value - Centre.Point.X.Value);
    Arc.StartAngle.Value = startAngle;
    Arc.EndAngle.Value = endAngle;

    // update Rad as Start or End might have changed
    const double Tolerance = 1e-5;
    var startRadVecX = Start.Point.X.Value - Centre.Point.X.Value;
    var startRadVecY = Start.Point.Y.Value - Centre.Point.Y.Value;
    var startRad = Math.Sqrt(startRadVecX * startRadVecX + startRadVecY * startRadVecY);
    var endRadVecX = End.Point.X.Value - Centre.Point.X.Value;
    var endRadVecY = End.Point.Y.Value - Centre.Point.Y.Value;
    var endRad = Math.Sqrt(endRadVecX * endRadVecX + endRadVecY * endRadVecY);
    if (Math.Abs(Arc.Radius.Value - startRad) > Tolerance)
    {
      Arc.Radius.Value = startRad;
    }
    else if (Math.Abs(Arc.Radius.Value - endRad) > Tolerance)
    {
      Arc.Radius.Value = endRad;
    }

    // update Start+End to sit on Arc
    var startPtX = Arc.Centre.X.Value + Arc.Radius.Value * Math.Cos(Arc.StartAngle.Value);
    var startPtY = Arc.Centre.Y.Value + Arc.Radius.Value * Math.Sin(Arc.StartAngle.Value);
    Start.Point.X.Value = startPtX;
    Start.Point.Y.Value = startPtY;

    var endPtX = Arc.Centre.X.Value + Arc.Radius.Value * Math.Cos(Arc.EndAngle.Value);
    var endPtY = Arc.Centre.Y.Value + Arc.Radius.Value * Math.Sin(Arc.EndAngle.Value);
    End.Point.X.Value = endPtX;
    End.Point.Y.Value = endPtY;

    await base.DrawAsyncInternal(batch);
  }
}
