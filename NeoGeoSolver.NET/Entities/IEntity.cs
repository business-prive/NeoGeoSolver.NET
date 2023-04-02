using NeoGeoSolver.NET.Sketch;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public interface IEntity : ICADObject {
  IEnumerable<ExpVector> points { get; }			// enough for dragging
  IEnumerable<Vector3> segments { get; }			// enough for drawing
  ExpVector PointOn(Exp t);						// enough for constraining
  ExpVector TangentAt(Exp t);
  Exp Length();
  Exp Radius();
  ExpVector Center();
  IPlane plane { get; }
  IEntityType type { get; }
}