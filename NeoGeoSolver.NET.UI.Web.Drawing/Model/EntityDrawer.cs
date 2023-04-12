using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;

namespace NeoGeoSolver.NET.UI.Web.Drawing.Model;

public abstract class EntityDrawer : IDrawable
{
  public const string DefaultClr = "black";
  public const string PreviewClr = "blue";
  public const string SelectedClr = "orange";

  public const double CircleRadius = 8d;
  public const double RectSize = 2 * CircleRadius;

  public bool IsSelected { get; set; }
  public bool ShowPreview { get; set; }
  public abstract object Entity { get; }

  public abstract IEnumerable<PointDrawer> SelectionPoints { get; }

  public abstract bool IsNear(Point pt);

  public async Task DrawAsync(Batch2D batch)
  {
    await batch.BeginPathAsync();
    await Initialise(batch);
    await DrawAsyncInternal(batch);
    await batch.StrokeAsync();
  }

  protected async Task Initialise(Batch2D batch)
  {
    await batch.GlobalCompositeOperationAsync(CompositeOperation.Source_Over);
    await SetLine(batch);
    await SetColour(batch);
  }

  protected virtual async Task SetLine(Batch2D batch)
  {
    if (IsSelected)
    {
      await batch.LineWidthAsync(3);
    }
    else if (ShowPreview)
    {
      await batch.LineWidthAsync(3);
    }
    else
    {
      await batch.LineWidthAsync(1);
    }

    await batch.LineJoinAsync(LineJoin.Round);
    await batch.LineCapAsync(LineCap.Round);
  }

  protected virtual async Task SetColour(Batch2D batch)
  {
    if (IsSelected)
    {
      await batch.StrokeStyleAsync(SelectedClr);
    }
    else if (ShowPreview)
    {
      await batch.StrokeStyleAsync(PreviewClr);
    }
    else
    {
      await batch.StrokeStyleAsync(DefaultClr);
    }
  }

  protected abstract Task DrawAsyncInternal(Batch2D batch);
}
