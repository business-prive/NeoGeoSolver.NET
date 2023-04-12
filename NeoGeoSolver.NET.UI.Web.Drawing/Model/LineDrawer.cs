using System.Collections.Generic;
using System.Threading.Tasks;
using Excubo.Blazor.Canvas.Contexts;
using NeoGeoSolver.NET.Entities;

namespace NeoGeoSolver.NET.UI.Web.Drawing.Model;

public sealed class LineDrawer : EntityDrawer
{
  public Line Line { get; }
  private StartPointDrawer Start { get; }
  private EndPointDrawer End { get; }

  public override IEnumerable<PointDrawer> SelectionPoints => new PointDrawer[] {Start, End};
  public override object Entity => Line;

  public override bool IsNear(System.Drawing.Point pt)
  {
    return pt.IsNear(Line);
  }

  public LineDrawer(Line line)
  {
    Line = line;
    Start = new StartPointDrawer(Line.Point0);
    End = new EndPointDrawer(Line.Point1);
  }

  protected override async Task DrawAsyncInternal(Batch2D batch)
  {
    await Start.DrawAsync(batch);

    await batch.BeginPathAsync();
    await Initialise(batch);

    await batch.MoveToAsync(Line.Point0.x.Value, Line.Point0.y.Value);
    await batch.LineToAsync(Line.Point1.x.Value, Line.Point1.y.Value);
    await batch.StrokeAsync();

    await End.DrawAsync(batch);
  }
}
