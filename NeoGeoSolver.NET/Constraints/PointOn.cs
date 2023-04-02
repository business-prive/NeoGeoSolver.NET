using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointOn : Value {
	public IEntity point { get { return GetEntity(0); } set { SetEntity(0, value); } }
	public IEntity on { get { return GetEntity(1); } set { SetEntity(1, value); } }

	public ExpressionVector pointExp { get { return point.PointExpInPlane(sketch.plane); } }

	protected override bool OnSatisfy() {
		EquationSystem sys = new EquationSystem();
		sys.AddParameters(parameters);
		var exprs = equations.ToList();
		sys.AddEquations(equations);

		double bestI = 0.0;
		double min = -1.0;
		for(double i = 0.0; i < 1.0; i += 0.25 / 2.0) {
			value.value = i;
			sys.Solve();
			double cur_value = exprs.Sum(e => Math.Abs(e.Eval()));
			if(min >= 0.0 && min < cur_value) continue;
			bestI = value.value;
			min = cur_value;
		}
		value.value = bestI;
		return true;
	}

	public override IEnumerable<Expression> equations {
		get {
			var p = pointExp;
			var eq = on.PointOnInPlane(value, sketch.plane) - p;

			yield return eq.x;
			yield return eq.y;
			if(sketch.is3d) yield return eq.z;
		}
	}

	public override double LabelToValue(double label) {
		switch(on.type) {
			//case IEntityType.Arc:
			//case IEntityType.Circle:
			case IEntityType.Helix:
				return label / 180.0 * Math.PI;
		}
		return base.LabelToValue(label);
	}

	public override double ValueToLabel(double value) {
		switch(on.type) {
			//case IEntityType.Arc:
			//case IEntityType.Circle:
			case IEntityType.Helix:
				return value * 180.0 / Math.PI;
		}
		return base.ValueToLabel(value);
	}
}
