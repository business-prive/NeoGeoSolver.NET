using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Model;

public sealed class CentrePointDrawer : PointDrawer
{
  public CentrePointDrawer(Point point) :
    base(point)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await batch.BeginPathAsync();
    await Initialise(batch);
    await batch.MoveToAsync(Point.x.Value, Point.y.Value + RectSize / 2);
    await batch.LineToAsync(Point.x.Value, Point.y.Value - RectSize / 2);
      
    await batch.MoveToAsync(Point.x.Value + RectSize / 2, Point.y.Value);
    await batch.LineToAsync(Point.x.Value - RectSize / 2, Point.y.Value);
    await batch.StrokeAsync();
  }
}
