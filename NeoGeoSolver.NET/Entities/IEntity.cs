using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public interface IEntity {
  ExpressionVector PointOn(Expression t);						// enough for constraining
  ExpressionVector TangentAt(Expression t);
  Expression Length();
  Expression Radius();
  ExpressionVector Center();
  EntityType type { get; }
  IEnumerable<Expression> equations { get; }
  IEnumerable<Param> parameters { get; }
}