using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsCoincident : Constraint {
	public Point p0 { get;}
	public Point p1 { get;}

  public PointsCoincident(Point pt0, Point pt1)
  {
    p0 = pt0;
    p1 = pt1;
  }
  
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
