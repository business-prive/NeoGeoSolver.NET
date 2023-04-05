using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public interface IEntity {
  IEnumerable<Expression> Equations { get; }
  IEnumerable<Param> Parameters { get; }
}