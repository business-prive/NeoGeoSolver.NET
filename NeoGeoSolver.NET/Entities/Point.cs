using System.Numerics;
using NeoGeoSolver.NET.Constraints;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Point : Entity {
	public Param x = new("x");
	public Param y = new("y");
	public Param z = new("z");

	public override IEntityType type { get { return IEntityType.Point; } }

	public Vector3 GetPosition() {
		if(transform != null) {
			return exp.Eval();
		}
		return new Vector3((float)x.value, (float)y.value, (float)z.value);
	}

	public void SetPosition(Vector3 pos) {
		if(transform != null) return;
		x.value = pos.X;
		y.value = pos.Y;
	}

	public Vector3 pos {
		get {
			return GetPosition();
		}
		set {
			SetPosition(value);
		}
	}

	private ExpressionVector exp_;
	public ExpressionVector exp {
		get {
			if(exp_ == null) {
				exp_ = new ExpressionVector(x, y, z);
			}
			if(transform != null) {
				return transform(exp_);
			}
			return exp_;
		}
	}

	public override IEnumerable<Param> parameters {
		get {
			yield return x;
			yield return y;
		}
	}

	public override IEnumerable<Point> points {
		get {
			yield return this;
		}
	}

	private bool IsCoincidentWith(IEntity point, IEntity exclude) {
		if(point.IsSameAs(this)) return true;
		for(int i = 0; i < usedInConstraints.Count; i++) {
			var c = usedInConstraints[i] as PointsCoincident;
			if(c == null) continue;
			var p = c.GetOtherPoint(this);
			if(p.IsSameAs(point) || !p.IsSameAs(exclude) && p is Point && (p as Point).IsCoincidentWith(point, this)) {
				return true;
			}
		}
		return false;
	}

	public bool IsCoincidentWith(IEntity point) {
		return IsCoincidentWith(point, null);
	}

	public override ExpressionVector PointOn(Expression t) {
		return exp;
	}

	public override ExpressionVector TangentAt(Expression t) {
		return null;
	}

	public override Expression Length() {
		return null;
	}

	public override Expression Radius() {
		return null;
	}
}
