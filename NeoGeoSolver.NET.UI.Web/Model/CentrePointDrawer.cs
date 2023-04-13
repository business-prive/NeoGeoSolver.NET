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
    await batch.MoveToAsync(Point.X.Value, Point.Y.Value + RectSize / 2);
    await batch.LineToAsync(Point.X.Value, Point.Y.Value - RectSize / 2);
      
    await batch.MoveToAsync(Point.X.Value + RectSize / 2, Point.Y.Value);
    await batch.LineToAsync(Point.X.Value - RectSize / 2, Point.Y.Value);
    await batch.StrokeAsync();
  }
}
