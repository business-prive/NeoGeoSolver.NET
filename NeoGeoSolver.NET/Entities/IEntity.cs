
using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public interface IEntity {
  IEnumerable<ExpressionVector> points { get; }			// enough for dragging
  IEnumerable<Vector3> segments { get; }			// enough for drawing
  ExpressionVector PointOn(Expression t);						// enough for constraining
  ExpressionVector TangentAt(Expression t);
  Expression Length();
  Expression Radius();
  ExpressionVector Center();
  IEntityType type { get; }
}