using System.Numerics;
using NeoGeoSolver.NET.Constraints;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class EllipticArc : Entity, ISegmentaryEntity {

	public Point p0;
	public Point p1;
	public Point c;

	public EllipticArc(Sketch.Sketch sk) : base(sk) {
		p0 = AddChild(new Point(sk));
		p1 = AddChild(new Point(sk));
		c = AddChild(new Point(sk));
	}

	public override IEntityType type { get { return IEntityType.Arc; } }

	public override IEnumerable<Expression> equations {
		get {
			if(!p0.IsCoincidentWith(p1)) {
				yield return (p0.exp - c.exp).Magnitude() - (p1.exp - c.exp).Magnitude();
			}
		}
	}

	public override IEnumerable<Point> points {
		get {
			yield return p0;
			yield return p1;
			yield return c;
		}
	}

	public Expression GetAngleExp() {
		if(!p0.IsCoincidentWith(p1)) {
			var d0 = p0.exp - c.exp;
			var d1 = p1.exp - c.exp;
			return ConstraintExp.angle2d(d0, d1, angle360: true);
		}
		return Math.PI * 2.0;
	}

	public Point begin { get { return p0; } }
	public Point end { get { return p1; } }
	public Point center { get { return c; } }
	public IEnumerable<Vector3> segmentPoints {
		get {
			return getSegmentsUsingPointOn(36);
		}
	}	

	public override ExpressionVector PointOn(Expression t) {
		var angle = GetAngleExp();
		var cos = Expression.Cos(angle * t);
		var sin = Expression.Sin(angle * t);
		var rv = p0.exp - c.exp;

		return c.exp + new ExpressionVector(
			cos * rv.x - sin * rv.y, 
			sin * rv.x + cos * rv.y, 
			0.0
		);
	}

	public override ExpressionVector TangentAt(Expression t) {
		var angle = GetAngleExp();
		var cos = Expression.Cos(angle * t + Math.PI / 2);
		var sin = Expression.Sin(angle * t + Math.PI / 2);
		var rv = p0.exp - c.exp;

		return new ExpressionVector(
			cos * rv.x - sin * rv.y, 
			sin * rv.x + cos * rv.y, 
			0.0
		);
	}
	
	public override Expression Length() {
		return GetAngleExp() * Radius();
	}

	public override Expression Radius() {
		return (p0.exp - c.exp).Magnitude();
	}

	public override ExpressionVector Center() {
		return c.exp;
	}
}
