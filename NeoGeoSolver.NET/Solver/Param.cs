namespace NeoGeoSolver.NET.Solver;

public class Param {
	public string name;
	public bool reduceable = true;
	private double v;
	public bool changed;

	public double value {
		get { return v; }
		set {
			if(v == value) return;
			changed = true;
			v = value;
		}
	}

	public Exp exp { get; private set; }

	public Param(string name, bool reduceable = true) {
		this.name = name;
		this.reduceable = reduceable;
		exp = new Exp(this);
	}

	public Param(string name, double value) {
		this.name = name;
		this.value = value;
		exp = new Exp(this);
	}

}