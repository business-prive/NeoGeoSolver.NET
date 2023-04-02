using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Diameter : Value {

	public Diameter(Sketch.Sketch sk) : base(sk) { }

	public bool showAsRadius = false;

	public Diameter(Sketch.Sketch sk, IEntity c) : base(sk) {
		showAsRadius = (c.type == IEntityType.Arc);
		AddEntity(c);
		Satisfy();
	}

	private Expression radius { get { return GetEntity(0).Radius(); } }

	public override IEnumerable<Expression> equations {
		get {
			yield return radius * 2.0 - value.exp;
		}
	}

	public override double LabelToValue(double label) {
		return showAsRadius ? label / 2.0 : label;
	}

	public override double ValueToLabel(double value) {
		return showAsRadius ? value * 2.0 : value;
	}
}
