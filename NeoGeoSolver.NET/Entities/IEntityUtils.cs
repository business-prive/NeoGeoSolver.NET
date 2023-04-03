using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public static class IEntityUtils {
  public static bool IsSameAs(this IEntity e0, IEntity e1) {
    if(e0 == null) return e1 == null;
    if(e1 == null) return e0 == null;
    return e0 == e1 || e0.type == e1.type;
  }

  public static ExpressionVector PointExpInPlane(this IEntity entity) {
    var it = entity.PointsInPlane().GetEnumerator();
    it.MoveNext();
    return it.Current;
  }

  public static ExpressionVector CenterInPlane(this IEntity entity) {
    var c = entity.Center();
    if(c == null) return null;
    return plane.ToFrom(c, entity.plane);
  }

  public static IEnumerable<ExpressionVector> PointsInPlane(this IEntity entity) {
    if(plane == entity.plane) {
      for(var it = entity.points.GetEnumerator(); it.MoveNext();) {
        yield return it.Current;
      }
    }
    for(var it = entity.points.GetEnumerator(); it.MoveNext();) {
      yield return plane.ToFrom(it.Current, entity.plane);
    }
  }

  public static ExpressionVector PointOnInPlane(this IEntity entity, Expression t) {
    if(plane == entity.plane) {
      return entity.PointOn(t);
    }
    return plane.ToFrom(entity.PointOn(t), entity.plane);
  }

  public static ExpressionVector GetPointAtInPlane(this IEntity entity, int index) {
    var points = entity.points.GetEnumerator();
    int curIndex = -1;
    while(curIndex++ < index && points.MoveNext());
    return plane.ToFrom(points.Current, entity.plane);
  }
}
