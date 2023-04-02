using System.Numerics;

namespace NeoGeoSolver.NET.Entities;

public interface ISegmentaryEntity {
  Point begin { get; }
  Point end { get; }
  IEnumerable<Vector3> segmentPoints { get; }
}