using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Diameter : Value {

	public bool showAsRadius = false;

	public Diameter(IEntity c)
	{
		showAsRadius = (c.type == EntityType.Arc);
		AddEntity(c);
		Satisfy();
	}

	private Expression radius { get { return GetEntity(0).Radius(); } }

	public override IEnumerable<Expression> equations {
		get {
			yield return radius * 2.0 - value.exp;
		}
	}
}
