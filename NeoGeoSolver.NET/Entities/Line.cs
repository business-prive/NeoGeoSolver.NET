using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Line : Entity {
	public Point p0 = new();
	public Point p1 = new();

	public override IEnumerable<Expression> Equations { get { yield break; } }

	public override ExpressionVector PointOn(Expression t) {
		var pt0 = p0.exp;
		var pt1 = p1.exp;
		return pt0 + (pt1 - pt0) * t;
	}

	public override ExpressionVector TangentAt(Expression t) {
		return p1.exp - p0.exp;
	}

	public Expression LengthExpr() {
		return (p1.exp - p0.exp).Magnitude();
	}
}
