using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Diameter : Value {
  private readonly Circle _circle;

	public Diameter(Circle circle)
	{
		_circle = circle;
		Satisfy();
	}

	private Expression radius { get { return _circle.Radius(); } }

	public override IEnumerable<Expression> equations {
		get {
			yield return radius * 2.0 - value.exp;
		}
	}
}
