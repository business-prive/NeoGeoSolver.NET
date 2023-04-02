using System.Numerics;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Sketch;

public static class IPlaneUtils {

  public static Quaternion GetRotation(this IPlane plane) {
    return Quaternion.LookRotation(plane.n, plane.v);
  }

  public static Matrix4x4 GetTransform(this IPlane plane) {
    if(plane == null) return Matrix4x4.identity;
    return UnityExt.Basis(plane.u, plane.v, plane.n, plane.o);
  }

  public static ExpVector FromPlane(this IPlane plane, ExpVector pt) {
    if(plane == null) return pt;
    return plane.o + (ExpVector)plane.u * pt.x + (ExpVector)plane.v * pt.y + (ExpVector)plane.n * pt.z;
  }

  public static Vector3 FromPlane(this IPlane plane, Vector3 pt) {
    if(plane == null) return pt;
    return plane.o + plane.u * pt.x + plane.v * pt.y + plane.n * pt.z;
  }

  public static ExpVector DirFromPlane(this IPlane plane, ExpVector dir) {
    if(plane == null) return dir;
    return (ExpVector)plane.u * dir.x + (ExpVector)plane.v * dir.y + (ExpVector)plane.n * dir.z;
  }

  public static Vector3 DirFromPlane(this IPlane plane, Vector3 dir) {
    if(plane == null) return dir;
    return plane.u * dir.x + plane.v * dir.y + plane.n * dir.z;
  }

  public static IEnumerable<Vector3> FromPlane(this IPlane plane, IEnumerable<Vector3> points) {
    if(plane == null) return points;

    var pu = plane.u;
    var pv = plane.v;
    var pn = plane.n;
    var po = plane.o;
    return points.Select(pt => po + pu * pt.x + pv * pt.y + pn * pt.z);
  }

  public static ExpVector ToPlane(this IPlane plane, ExpVector pt) {
    if(plane == null) return pt;
    ExpVector result = new ExpVector(0, 0, 0);
    var dir = pt - plane.o;
    result.x = ExpVector.Dot(dir, plane.u);
    result.y = ExpVector.Dot(dir, plane.v);
    result.z = ExpVector.Dot(dir, plane.n);
    return result;
  }

  public static Vector3 ToPlane(this IPlane plane, Vector3 pt) {
    if(plane == null) return pt;
    Vector3 result = new Vector3(0, 0, 0);
    var dir = pt - plane.o;
    result.x = Vector3.Dot(dir, plane.u);
    result.y = Vector3.Dot(dir, plane.v);
    result.z = Vector3.Dot(dir, plane.n);
    return result;
  }

  public static ExpVector DirToPlane(this IPlane plane, ExpVector dir) {
    if(plane == null) return dir;
    ExpVector result = new ExpVector(0, 0, 0);
    result.x = ExpVector.Dot(dir, plane.u);
    result.y = ExpVector.Dot(dir, plane.v);
    result.z = ExpVector.Dot(dir, plane.n);
    return result;
  }

  public static Vector3 DirToPlane(this IPlane plane, Vector3 dir) {
    if(plane == null) return dir;
    Vector3 result = new Vector3(0, 0, 0);
    result.x = Vector3.Dot(dir, plane.u);
    result.y = Vector3.Dot(dir, plane.v);
    result.z = Vector3.Dot(dir, plane.n);
    return result;
  }

  public static Vector3 projectVectorInto(this IPlane plane, Vector3 val) {
    if(plane == null) return val;
    Vector3 r = val - plane.o;
    float up = Vector3.Dot(r, plane.u);
    float vp = Vector3.Dot(r, plane.v);
    return plane.u * up + plane.v * vp + plane.o;
  }

  public static IEnumerable<Vector3> ToPlane(this IPlane plane, IEnumerable<Vector3> points) {
    if(plane == null) return points;

    var pu = plane.u;
    var pv = plane.v;
    var pn = plane.n;
    var po = plane.o;

    return points.Select(pt => {
      var dir = pt - po;
      return new Vector3(
        Vector3.Dot(dir, pu),
        Vector3.Dot(dir, pv),
        Vector3.Dot(dir, pn)
      );
    });
  }

  public static ExpVector ToFrom(this IPlane to, ExpVector pt, IPlane from) {
    return to.ToPlane(from.FromPlane(pt));
  }

  public static Vector3 ToFrom(this IPlane to, Vector3 pt, IPlane from) {
    return to.ToPlane(from.FromPlane(pt));
  }

  public static IEnumerable<Vector3> ToFrom(this IPlane to, IEnumerable<Vector3> points, IPlane from) {
    return to.ToPlane(from.FromPlane(points));
  }

  public static ExpVector DirToFrom(this IPlane to, ExpVector pt, IPlane from) {
    return to.DirToPlane(from.DirFromPlane(pt));
  }

}