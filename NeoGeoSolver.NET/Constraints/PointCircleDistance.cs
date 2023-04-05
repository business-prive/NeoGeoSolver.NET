using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointCircleDistance : Value {
  private readonly Point _pt;
  private readonly Circle _circle;
	public PointCircleDistance(Point pt, Circle c)
	{
		_pt = pt;
		_circle = c;
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			var pPos = _pt.exp;
			var cCen = _circle.Center();
			var cRad = _circle.Radius();

			yield return (pPos - cCen).Magnitude() - cRad - value.exp;
		}
	}
}
