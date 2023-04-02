using System.Numerics;

namespace NeoGeoSolver.NET.Entities;

public interface ILoopEntity {
  IEnumerable<Vector3> loopPoints { get; }
}