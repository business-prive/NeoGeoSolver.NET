namespace NeoGeoSolver.NET.Solver;

public class Param {
	public string name;
	public bool reduceable = true;
	private double _v;
	public bool changed;

	public double value {
		get { return _v; }
		set {
			if(_v == value) return;
			changed = true;
			_v = value;
		}
	}

	public Expression exp { get; private set; }

	public Param(string name, bool reduceable = true) {
		this.name = name;
		this.reduceable = reduceable;
		exp = new Expression(this);
	}

	public Param(string name, double value) {
		this.name = name;
		this.value = value;
		exp = new Expression(this);
	}
}
