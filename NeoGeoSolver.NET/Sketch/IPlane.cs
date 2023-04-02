namespace NeoGeoSolver.NET.Sketch;

public interface IPlane {
  Vector3 u { get; }
  Vector3 v { get; }
  Vector3 n { get; }
  Vector3 o { get; }
}