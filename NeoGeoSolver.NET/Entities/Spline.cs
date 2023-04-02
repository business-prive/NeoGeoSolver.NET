using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Spline : Entity, ISegmentaryEntity {

	public Point[] p = new Point[4];

	public Spline(Sketch.Sketch sk) : base(sk) {
		for(int i = 0; i < p.Length; i++) {
			p[i] = AddChild(new Point(sk));
		}
	}

	public override IEntityType type { get { return IEntityType.Spline; } }

	public override IEnumerable<Point> points {
		get {
			for(int i = 0; i < p.Length; i++) {
				yield return p[i];
			}
		}
	}

	public Point begin { get { return p[0]; } }
	public Point end { get { return p[3]; } }
	public IEnumerable<Vector3> segmentPoints {
		get {
			return getSegments(32, t => PointOn(t));
		}
	}

	public override ExpressionVector PointOn(Expression t) {
		var p0 = p[0].exp;
		var p1 = p[1].exp;
		var p2 = p[2].exp;
		var p3 = p[3].exp;
		var t2 = t * t;
		var t3 = t2 * t;
		return p1 * (3.0 * t3 - 6.0 * t2 + 3.0 * t) + p3 * t3 + p2 * (3.0 * t2 - 3.0 * t3) - p0 * (t3 - 3.0 * t2 + 3.0 * t - 1.0);
	}

	public Vector3 PointOn(double t) {
		var p0 = p[0].pos;
		var p1 = p[1].pos;
		var p2 = p[2].pos;
		var p3 = p[3].pos;
		var t2 = t * t;
		var t3 = t2 * t;
		return p1 * (float)(3.0 * t3 - 6.0 * t2 + 3.0 * t) + p3 * (float)t3 + p2 * (float)(3.0 * t2 - 3.0 * t3) - p0 * (float)(t3 - 3.0 * t2 + 3.0 * t - 1.0);
	}

	public override Expression Length() {
		return null;
	}

	public override Expression Radius() {
		return null;
	}

	public override ExpressionVector Center() {
		return null;
	}
}
