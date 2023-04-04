namespace NeoGeoSolver.NET.Solver;

public class Param {
	public string name;
	public bool reduceable = true;
	public double value;

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
