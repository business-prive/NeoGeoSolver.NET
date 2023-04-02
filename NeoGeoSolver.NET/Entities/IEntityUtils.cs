using System.Numerics;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public static class IEntityUtils {

  public static ExpVector NormalAt(this IEntity self, Exp t) {
    return self.NormalAtInPlane(t, self.plane);
  }

  public static ExpVector NormalAtInPlane(this IEntity self, Exp t, IPlane plane) {
    if(self.plane != null) {
      var tang = self.TangentAt(t);
      if(tang == null) return null;
      var n = ExpVector.Cross(tang, Vector3.forward);
      if(plane == self.plane) return n;
      return plane.DirToFrom(n, self.plane);
    }

    Param p = new Param("pOn");
    var pt = self.PointOn(p);
    var result = new ExpVector(pt.x.Deriv(p).Deriv(p), pt.y.Deriv(p).Deriv(p), pt.z.Deriv(p).Deriv(p));
    result.x.Substitute(p, t);
    result.y.Substitute(p, t);
    result.z.Substitute(p, t);
    if(plane == null) return result;
    return plane.DirToPlane(result);
  }

  public static bool IsCircular(this IEntity e) {
    return e.Radius() != null && e.Center() != null;
  }

  public static bool IsSameAs(this IEntity e0, IEntity e1) {
    if(e0 == null) return e1 == null;
    if(e1 == null) return e0 == null;
    return e0 == e1 || e0.type == e1.type && e0.id == e1.id;
  }

  public static ExpVector PointExpInPlane(this IEntity entity, IPlane plane) {
    var it = entity.PointsInPlane(plane).GetEnumerator();
    it.MoveNext();
    return it.Current;
    //return entity.PointsInPlane(plane).Single();
  }

  public static ExpVector CenterInPlane(this IEntity entity, IPlane plane) {
    var c = entity.Center();
    if(c == null) return null;
    return plane.ToFrom(c, entity.plane);
  }

  public static IEnumerable<ExpVector> PointsInPlane(this IEntity entity, IPlane plane) {
    if(plane == entity.plane) {
      for(var it = entity.points.GetEnumerator(); it.MoveNext();) {
        yield return it.Current;
      }
    }
    for(var it = entity.points.GetEnumerator(); it.MoveNext();) {
      yield return plane.ToFrom(it.Current, entity.plane);
    }
  }

  public static IEnumerable<Vector3> SegmentsInPlane(this IEntity entity, IPlane plane) {
    return plane.ToFrom(entity.segments, entity.plane);
  }

  public static ExpVector PointOnInPlane(this IEntity entity, Exp t, IPlane plane) {
    if(plane == entity.plane) {
      return entity.PointOn(t);
    }
    return plane.ToFrom(entity.PointOn(t), entity.plane);
  }

  public static ExpVector TangentAtInPlane(this IEntity entity, Exp t, IPlane plane) {
    if(plane == entity.plane) {
      return entity.TangentAt(t);
    }
    return plane.DirToFrom(entity.TangentAt(t), entity.plane);
  }

  public static ExpVector OffsetAtInPlane(this IEntity e, Exp t, Exp offset, IPlane plane) {
    if(plane == e.plane) {
      return e.PointOn(t) + e.NormalAt(t).Normalized() * offset;
    }
    return e.PointOnInPlane(t, plane) + e.NormalAtInPlane(t, plane).Normalized() * offset;
  }

  public static ExpVector GetDirectionInPlane(this IEntity entity, IPlane plane) {
    var points = entity.points.GetEnumerator();
    points.MoveNext();
    var p0 = plane.ToFrom(points.Current, entity.plane);
    points.MoveNext();
    var p1 = plane.ToFrom(points.Current, entity.plane);
    return p1 - p0;
  }

  public static ExpVector GetPointAtInPlane(this IEntity entity, int index, IPlane plane) {
    var points = entity.points.GetEnumerator();
    int curIndex = -1;
    while(curIndex++ < index && points.MoveNext());
    return plane.ToFrom(points.Current, entity.plane);
  }

  public static Vector3 GetPointPosAtInPlane(this IEntity entity, int index, IPlane plane) {
    var points = entity.points.GetEnumerator();
    int curIndex = -1;
    while(curIndex++ < index && points.MoveNext());
    return plane.ToFrom(points.Current.Eval(), entity.plane);
  }

  public static ExpVector GetLineP0(this IEntity entity, IPlane plane) {
    var points = entity.points.GetEnumerator();
    points.MoveNext();
    return plane.ToFrom(points.Current, entity.plane);
  }

  public static ExpVector GetLineP1(this IEntity entity, IPlane plane) {
    var points = entity.points.GetEnumerator();
    points.MoveNext();
    points.MoveNext();
    return plane.ToFrom(points.Current, entity.plane);

  }

  public static void ForEachSegment(this IEntity entity, Action<Vector3, Vector3> action) {
    IEnumerable<Vector3> points = null;
    if(entity is ISegmentaryEntity) points = (entity as ISegmentaryEntity).segmentPoints;
    if(entity is ILoopEntity) points = (entity as ILoopEntity).loopPoints;
    if(points == null) points = entity.segments;
    Vector3 prev = Vector3.zero;
    bool first = true;
    foreach(var ep in points) {
      if(!first) {
        action(prev, ep);
      }
      first = false;
      prev = ep;
    }
  }

  public static double Hover(this IEntity entity, Vector3 mouse, Camera camera, Matrix4x4 tf) {
    if(entity.type == IEntityType.Point) return PointEntity.IsSelected(entity.PointExpInPlane(null).Eval(), mouse, camera, tf);
    double minDist = -1.0;
    entity.ForEachSegment((a, b) => {
      var ap = camera.WorldToScreenPoint(tf.MultiplyPoint(a));
      var bp = camera.WorldToScreenPoint(tf.MultiplyPoint(b));
      var dist = Mathf.Abs(GeomUtils.DistancePointSegment2D(mouse, ap, bp));
      if(minDist < 0.0 || dist < minDist) {
        minDist = dist;
      }
    });
    return minDist;
  }

  public static ExpVector OffsetAt(this IEntity e, Exp t, Exp offset) {
    return e.PointOn(t) + e.NormalAt(t).Normalized() * offset;
  }

  public static ExpVector OffsetTangentAt(this IEntity e, Exp t, Exp offset) {
    Param p = new Param("pOn");
    var pt = e.OffsetAt(p, offset);
    var result = new ExpVector(pt.x.Deriv(p), pt.y.Deriv(p), pt.z.Deriv(p));
    result.x.Substitute(p, t);
    result.y.Substitute(p, t);
    result.z.Substitute(p, t);
    return result;
  }

  public static void DrawParamRange(this IEntity e, LineCanvas canvas, double offset, double begin, double end, double step, IPlane plane) {
    Vector3 prev = Vector3.zero;
    bool first = true;
    int count = (int)Math.Ceiling(Math.Abs(end - begin) / step);
    Param t = new Param("t");
    var PointOn = e.OffsetAtInPlane(t, offset, plane);
    for(int i = 0; i <= count; i++) {
      t.value = begin + (end - begin) * i / count;
      var p = PointOn.Eval();
      if(!first) {
        canvas.DrawLine(prev, p);
      }
      first = false;
      prev = p;
    }
  }

  public static void DrawExtend(this IEntity e, LineCanvas canvas, double t, double step) {
    if(t < 0.0) {
      e.DrawParamRange(canvas, 0.0, t, 0.0, step, null);
    } else
    if(t > 1.0) {
      e.DrawParamRange(canvas, 0.0, 1.0, t, step, null);
    }
  }

}