using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public interface IEntity {
  IEnumerable<Expression> equations { get; }
  IEnumerable<Param> parameters { get; }
}