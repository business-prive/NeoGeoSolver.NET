using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Model;

public sealed class EndPointDrawer : PointDrawer
{
  public EndPointDrawer(Point point) :
    base(point)
  {
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await batch.RectAsync(Point.x.Value - RectSize / 2,  Point.y.Value - RectSize / 2, RectSize, RectSize);
  }
}
