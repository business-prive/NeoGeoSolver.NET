using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionBasis {
	private Param px, py, pz;
	private Param ux, uy, uz;
	private Param vx, vy, vz;
	private Param nx, ny, nz;

	public ExpressionVector u { get; private set; }
	public ExpressionVector v { get; private set; }
	public ExpressionVector n { get; private set; }
	public ExpressionVector p { get; private set; }

	public ExpressionBasis() {
		px = new Param("ux", 0.0);
		py = new Param("uy", 0.0);
		pz = new Param("uz", 0.0);

		ux = new Param("ux", 1.0);
		uy = new Param("uy", 0.0);
		uz = new Param("uz", 0.0);

		vx = new Param("vx", 0.0);
		vy = new Param("vy", 1.0);
		vz = new Param("vz", 0.0);

		nx = new Param("nx", 0.0);
		ny = new Param("ny", 0.0);
		nz = new Param("nz", 1.0);

		p = new ExpressionVector(px, py, pz);
		u = new ExpressionVector(ux, uy, uz);
		v = new ExpressionVector(vx, vy, vz);
		n = new ExpressionVector(nx, ny, nz);
	}

	public IEnumerable<Param> parameters {
		get {
			yield return ux;
			yield return uy;
			yield return uz;
			yield return vx;
			yield return vy;
			yield return vz;
			yield return nx;
			yield return ny;
			yield return nz;
			yield return px;
			yield return py;
			yield return pz;
		}
	}

	public override string ToString() {
		string result = "";
		foreach(var p in parameters) {
			result += p.value.ToStr() + " ";
		}
		return result;
	}
}
