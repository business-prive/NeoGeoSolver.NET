using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionBasis {
	private Param _px = new("ux", 0.0), _py = new("uy", 0.0), _pz = new("uz", 0.0);
	private Param _ux = new("ux", 1.0), _uy = new("uy", 0.0), _uz = new("uz", 0.0);
	private Param _vx = new("vx", 0.0), _vy = new("vy", 1.0), _vz = new("vz", 0.0);
	private Param _nx = new("nx", 0.0), _ny = new("ny", 0.0), _nz = new("nz", 1.0);

	public ExpressionVector u { get; private set; }
	public ExpressionVector v { get; private set; }
	public ExpressionVector n { get; private set; }
	public ExpressionVector p { get; private set; }

	public ExpressionBasis() {
		p = new ExpressionVector(_px, _py, _pz);
		u = new ExpressionVector(_ux, _uy, _uz);
		v = new ExpressionVector(_vx, _vy, _vz);
		n = new ExpressionVector(_nx, _ny, _nz);
	}

	public IEnumerable<Param> parameters {
		get {
			yield return _ux;
			yield return _uy;
			yield return _uz;
			yield return _vx;
			yield return _vy;
			yield return _vz;
			yield return _nx;
			yield return _ny;
			yield return _nz;
			yield return _px;
			yield return _py;
			yield return _pz;
		}
	}

	public override string ToString() {
		var result = "";
		foreach(var p in parameters) {
			result += p.value.ToStr() + " ";
		}
		return result;
	}
}
