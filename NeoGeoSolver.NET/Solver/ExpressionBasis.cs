using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionBasis {
	private Param _px, _py, _pz;
	private Param _ux, _uy, _uz;
	private Param _vx, _vy, _vz;
	private Param _nx, _ny, _nz;

	public ExpressionVector u { get; private set; }
	public ExpressionVector v { get; private set; }
	public ExpressionVector n { get; private set; }
	public ExpressionVector p { get; private set; }

	public ExpressionBasis() {
		_px = new Param("ux", 0.0);
		_py = new Param("uy", 0.0);
		_pz = new Param("uz", 0.0);

		_ux = new Param("ux", 1.0);
		_uy = new Param("uy", 0.0);
		_uz = new Param("uz", 0.0);

		_vx = new Param("vx", 0.0);
		_vy = new Param("vy", 1.0);
		_vz = new Param("vz", 0.0);

		_nx = new Param("nx", 0.0);
		_ny = new Param("ny", 0.0);
		_nz = new Param("nz", 1.0);

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
