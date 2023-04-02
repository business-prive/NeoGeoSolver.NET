using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public static class ConstraintExp {

	public static ExpressionVector rotateDir2d(ExpressionVector rv, Expression angle) {
		var cos = Expression.Cos(angle);
		var sin = Expression.Sin(angle);
		return new ExpressionVector(
			cos * rv.x - sin * rv.y, 
			sin * rv.x + cos * rv.y, 
			rv.z
		);
	}

	public static Expression angle2d(ExpressionVector d0, ExpressionVector d1, bool angle360 = false) {
		Expression nu = d1.x * d0.x + d1.y * d0.y;
		Expression nv = d0.x * d1.y - d0.y * d1.x;
		if(angle360) return Math.PI - Expression.Atan2(nv, -nu);
		return Expression.Atan2(nv, nu);
	}
	
	public static Expression angle3d(ExpressionVector d0, ExpressionVector d1) {
		return Expression.Atan2(ExpressionVector.Cross(d0, d1).Magnitude(), ExpressionVector.Dot(d0, d1));
	}

	public static double angle2d(Vector3 d0, Vector3 d1, bool angle360 = false) {
		var nu = d1.x * d0.x + d1.y * d0.y;
		var nv = d0.x * d1.y - d0.y * d1.x;
		if(angle360) return Math.PI - Math.Atan2(nv, -nu);
		return Math.Atan2(nv, nu);
	}
	
	public static double angle3d(Vector3 d0, Vector3 d1) {
		return Math.Atan2(Vector3.Cross(d0, d1).magnitude, Vector3.Dot(d0, d1));
	}

	public static Expression pointLineDistance(ExpressionVector p, ExpressionVector p0, ExpressionVector p1, bool is3d) {
		if(is3d) {
			var d = p0 - p1;
			return ExpressionVector.Cross(d, p0 - p).Magnitude() / d.Magnitude();
		}
		return ((p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y + p0.x * p1.y - p1.x * p0.y) / Expression.Sqrt(Expression.Sqr(p1.x - p0.x) + Expression.Sqr(p1.y - p0.y));
	}

}