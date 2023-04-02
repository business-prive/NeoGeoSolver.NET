namespace NeoGeoSolver.NET.Entities;

public interface ISegmentaryEntity {
  PointEntity begin { get; }
  PointEntity end { get; }
  IEnumerable<Vector3> segmentPoints { get; }
}