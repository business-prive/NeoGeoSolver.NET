using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Length : Value {
	public Length(Sketch.Sketch sk) : base(sk) { }

	public Length(Sketch.Sketch sk, IEntity e) : base(sk) {
		AddEntity(e);
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			yield return GetEntity(0).Length() - value;
		}
	}
}
