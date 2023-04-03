using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Length : Value {
	public Length(IEntity e)
	{
		AddEntity(e);
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			yield return GetEntity(0).Length() - value;
		}
	}
}
