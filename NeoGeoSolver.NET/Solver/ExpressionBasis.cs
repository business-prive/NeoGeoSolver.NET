using System.Numerics;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionBasis {
	Param px, py, pz;
	Param ux, uy, uz;
	Param vx, vy, vz;
	Param nx, ny, nz;

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

	public Matrix4x4 matrix {
		get {
			return UnityExt.Basis(u.Eval(), v.Eval(), n.Eval(), p.Eval());
		}
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

	public void FromString(string str) {
		char[] sep = { ' ' };
		var values = str.Split(sep, StringSplitOptions.RemoveEmptyEntries);
		int i = 0;
		foreach(var p in parameters) {
			p.value = values[i].ToDouble();
			i++;
		}
	}

	public ExpressionVector TransformPosition(ExpressionVector pos) {
		return pos.x * u + pos.y * v + pos.z * n + p;
	}

	public ExpressionVector TransformDirection(ExpressionVector dir) {
		return dir.x * u + dir.y * v + dir.z * n;
	}

	public void GenerateEquations(EquationSystem sys) {
		sys.AddParameters(parameters);

		sys.AddEquation(u.Magnitude() - 1.0);
		sys.AddEquation(v.Magnitude() - 1.0);

		var cross = ExpressionVector.Cross(u, v);
		var dot = ExpressionVector.Dot(u, v);
		sys.AddEquation(Expression.Atan2(cross.Magnitude(), dot) - Math.PI / 2);
		sys.AddEquation(n - ExpressionVector.Cross(u, v));
	}

	public bool changed {
		get {
			return parameters.Any(p => p.changed);
		}
	}

	public void markUnchanged() {
		parameters.ForEach(pp => pp.changed = false);
	}
}