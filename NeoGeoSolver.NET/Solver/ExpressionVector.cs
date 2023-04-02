
using System.Numerics;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionVector {
	public Expression x;
	public Expression y;
	public Expression z;

	public ExpressionVector(Expression x, Expression y, Expression z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static implicit operator ExpressionVector(Vector3 v) {
		return  new ExpressionVector(v.x, v.y, v.z);
	}

	public static ExpressionVector operator+(ExpressionVector a, ExpressionVector b) { return new ExpressionVector(a.x + b.x, a.y + b.y, a.z + b.z); } 
	public static ExpressionVector operator-(ExpressionVector a, ExpressionVector b) { return new ExpressionVector(a.x - b.x, a.y - b.y, a.z - b.z); } 
	public static ExpressionVector operator*(ExpressionVector a, ExpressionVector b) { return new ExpressionVector(a.x * b.x, a.y * b.y, a.z * b.z); } 
	public static ExpressionVector operator/(ExpressionVector a, ExpressionVector b) { return new ExpressionVector(a.x / b.x, a.y / b.y, a.z / b.z); } 

	public static ExpressionVector operator-(ExpressionVector b) { return new ExpressionVector(-b.x, -b.y, -b.z); } 

	public static ExpressionVector operator*(Expression a, ExpressionVector b) { return new ExpressionVector(a * b.x, a * b.y, a * b.z); }
	public static ExpressionVector operator*(ExpressionVector a, Expression b) { return new ExpressionVector(a.x * b, a.y * b, a.z * b); }

	public static ExpressionVector operator/(Expression a, ExpressionVector b) { return new ExpressionVector(a / b.x, a / b.y, a / b.z); }
	public static ExpressionVector operator/(ExpressionVector a, Expression b) { return new ExpressionVector(a.x / b, a.y / b, a.z / b); }

	public static Expression Dot(ExpressionVector a, ExpressionVector b) { return a.x * b.x + a.y * b.y + a.z * b.z; }
	public static ExpressionVector Cross(ExpressionVector a, ExpressionVector b) {
		return new ExpressionVector(
			a.y * b.z - b.y * a.z,
			a.z * b.x - b.z * a.x,
			a.x * b.y - b.x * a.y
		);
	}

	public static Expression PointLineDistance(ExpressionVector point, ExpressionVector l0, ExpressionVector l1) {
		var d = l0 - l1;
		return Cross(d, l0 - point).Magnitude() / d.Magnitude();
	}

	public static float PointLineDistance(Vector3 point, Vector3 l0, Vector3 l1) {
		var d = l0 - l1;
		return Vector3.Cross(d, l0 - point).magnitude / d.magnitude;
	}

	public static ExpressionVector ProjectPointToLine(ExpressionVector p, ExpressionVector l0, ExpressionVector l1) {
		var d = l1 - l0;
		var t = Dot(d, p - l0) / Dot(d, d);
		return l0 + d * t;
	}

	public static Vector3 ProjectPointToLine(Vector3 p, Vector3 l0, Vector3 l1) {
		var d = l1 - l0;
		var t = Vector3.Dot(d, p - l0) / Vector3.Dot(d, d);
		return l0 + d * t;
	}

	public Expression Magnitude() {
		return Expression.Sqrt(Expression.Sqr(x) + Expression.Sqr(y) + Expression.Sqr(z));
	}

	public ExpressionVector Normalized() {
		return this / Magnitude();
	}

	public Vector3 Eval() {
		return new Vector3((float)x.Eval(), (float)y.Eval(), (float)z.Eval());
	}

	public bool ValuesEquals(ExpressionVector o, double eps) {
		return Math.Abs(x.Eval() - o.x.Eval()) < eps &&
		       Math.Abs(y.Eval() - o.y.Eval()) < eps &&
		       Math.Abs(z.Eval() - o.z.Eval()) < eps;
	}

	public static ExpressionVector RotateAround(ExpressionVector point, ExpressionVector axis, ExpressionVector origin, Expression angle) {
		var a = axis.Normalized();
		var c = Expression.Cos(angle);
		var s = Expression.Sin(angle);
		var u = new ExpressionVector(c + (1.0 - c) * a.x * a.x, (1.0 - c) * a.y * a.x + s * a.z, (1 - c) * a.z * a.x - s * a.y);
		var v = new ExpressionVector((1.0 - c) * a.x * a.y - s * a.z, c + (1.0 - c) * a.y * a.y, (1.0 - c) * a.z * a.y + s * a.x);
		var n = new ExpressionVector((1.0 - c) * a.x * a.z + s * a.y, (1.0 - c) * a.y * a.z - s * a.x,	c + (1 - c) * a.z * a.z);
		var p = point - origin;
		return p.x * u + p.y * v + p.z * n + origin;
	}

	public static Vector3 RotateAround(Vector3 point, Vector3 axis, Vector3 origin, float angle) {
		var a = axis.normalized;
		var c = Mathf.Cos(angle);
		var s = Mathf.Sin(angle);
		var u = new Vector3(c + (1 - c) * a.x * a.x, (1 - c) * a.y * a.x + s * a.z, (1 - c) * a.z * a.x - s * a.y);
		var v = new Vector3((1 - c) * a.x * a.y - s * a.z, c + (1 - c) * a.y * a.y, (1 - c) * a.z * a.y + s * a.x);
		var n = new Vector3((1 - c) * a.x * a.z + s * a.y, (1 - c) * a.y * a.z - s * a.x, c + (1 - c) * a.z * a.z);
		var p = point - origin;
		return p.x * u + p.y * v + p.z * n + origin;
	}

}