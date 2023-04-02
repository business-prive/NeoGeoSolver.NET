using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Line : Entity, ISegmentaryEntity {

	public Point p0;
	public Point p1;

	public Line(Sketch.Sketch sk) : base(sk) {
		p0 = AddChild(new Point(sk));
		p1 = AddChild(new Point(sk));
	}

	public override IEntityType type { get { return IEntityType.Line; } }

	public override IEnumerable<Point> points {
		get {
			yield return p0;
			yield return p1;
		}
	}

	public Point begin { get { return p0; } }
	public Point end { get { return p1; } }
	public IEnumerable<Vector3> segmentPoints {
		get {
			yield return p0.GetPosition();
			yield return p1.GetPosition();
		}
	}

	public override ExpressionVector PointOn(Expression t) {
		var pt0 = p0.exp;
		var pt1 = p1.exp;
		return pt0 + (pt1 - pt0) * t;
	}

	public override ExpressionVector TangentAt(Expression t) {
		return p1.exp - p0.exp;
	}

	public override Expression Length() {
		return (p1.exp - p0.exp).Magnitude();
	}

	public override Expression Radius() {
		return null;
	}
}
