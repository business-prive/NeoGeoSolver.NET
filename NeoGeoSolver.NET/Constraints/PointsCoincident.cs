using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsCoincident : Constraint {
	public Point p0 { get { return (Point) GetEntity(0); } set { SetEntity(0, value); } }
	public Point p1 { get { return (Point) GetEntity(1); } set { SetEntity(1, value); } }

	public override IEnumerable<Expression> equations {
		get {
			var pe0 = p0.exp;
			var pe1 = p1.exp;
			yield return pe0.x - pe1.x;
			yield return pe0.y - pe1.y;
		}
	}

	public IEntity GetOtherPoint(IEntity p) {
		if(p0 == p) return p1;
		return p0;
	}
}
