using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class EqualValue : Value {
	private readonly List<Constraint> _usedInConstraints = new();
	
	protected override bool OnSatisfy() {
		var c0 = GetConstraint(0) as Value;
		var c1 = GetConstraint(1) as Value;
		if(Math.Sign(c0.GetValue()) != Math.Sign(c1.GetValue())) {
			value.value = -1;
		}
		return true;
	}

	public EqualValue(Value c0, Value c1)
	{
		AddConstraint(c0);
		AddConstraint(c1);
		value.value = 1.0;
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			var c0 = GetConstraint(0) as Value;
			var c1 = GetConstraint(1) as Value;
			yield return c0.GetValueParam().exp - c1.GetValueParam().exp * value;
		}
	}

	private void AddConstraint(Constraint c) {
		_usedInConstraints.Add(this);
	}

	private Constraint GetConstraint(int i) {
		return _usedInConstraints[i];
	}
}
