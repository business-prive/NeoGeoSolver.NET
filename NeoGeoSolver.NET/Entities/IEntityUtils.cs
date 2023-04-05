using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public static class EntityUtils {
  public static ExpressionVector PointOnInPlane(this IEntity entity, Expression t) {
    if(plane == entity.plane) {
      return entity.PointOn(t);
    }
    return plane.ToFrom(entity.PointOn(t), entity.plane);
  }

  public static ExpressionVector GetPointAtInPlane(this IEntity entity, int index) {
    var points = entity.points.GetEnumerator();
    var curIndex = -1;
    while(curIndex++ < index && points.MoveNext());
    return plane.ToFrom(points.Current, entity.plane);
  }
}
