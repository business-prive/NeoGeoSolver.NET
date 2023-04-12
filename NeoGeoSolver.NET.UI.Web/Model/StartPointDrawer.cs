using System;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Model;

public sealed class StartPointDrawer : PointDrawer
{
  public StartPointDrawer(Point point) :
    base(point)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await batch.ArcAsync(Point.x.Value, Point.x.Value, CircleRadius, 0, 2 * Math.PI);
  }
}
