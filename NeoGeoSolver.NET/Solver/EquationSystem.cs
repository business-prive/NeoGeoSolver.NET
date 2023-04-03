namespace NeoGeoSolver.NET.Solver;

public class EquationSystem  {
	public enum SolveResult {
		Okay,
		DidntConvege,
		Redundant,
		Postpone
	}
	public bool isDirty { get; private set; }
	public int maxSteps = 20;
	public int dragSteps = 3;
	public bool revertWhenNotConverged = true;

	private Expression[,] _j;
	private double[,] _a;
	private double[,] _aat;
	private double[] _b;
	private double[] _x;
	private double[] _z;
	private double[] _oldParamValues;

	private List<Expression> _sourceEquations = new();
	private List<Param> _parameters = new();

	private List<Expression> _equations = new();
	private List<Param> _currentParams = new();

	private Dictionary<Param, Param> _subs;

	public void AddEquation(Expression eq) {
		_sourceEquations.Add(eq);
		isDirty = true;
	}

	public void AddEquation(ExpressionVector v) {
		_sourceEquations.Add(v.x);
		_sourceEquations.Add(v.y);
		_sourceEquations.Add(v.z);
		isDirty = true;
	}

	public void AddEquations(IEnumerable<Expression> eq) {
		_sourceEquations.AddRange(eq);
		isDirty = true;
	}

	public void RemoveEquation(Expression eq) {
		_sourceEquations.Remove(eq);
		isDirty = true;
	}

	public void AddParameter(Param p) {
		_parameters.Add(p);
		isDirty = true;
	}

	public void AddParameters(IEnumerable<Param> p) {
		_parameters.AddRange(p);
		isDirty = true;
	}

	public void RemoveParameter(Param p) {
		_parameters.Remove(p);
		isDirty = true;
	}

	public void Eval(ref double[] b, bool clearDrag) {
		for(var i = 0; i < _equations.Count; i++) {
			if(clearDrag && _equations[i].IsDrag()) {
				b[i] = 0.0;
				continue;
			}
			b[i] = _equations[i].Eval();
		}
	}

	public bool IsConverged(bool checkDrag, bool printNonConverged = false) {
		for(var i = 0; i < _equations.Count; i++) {
			if(!checkDrag && _equations[i].IsDrag()) {
				continue;
			}
			if(Math.Abs(_b[i]) < GaussianMethod.Epsilon) continue;
			if(printNonConverged) {
				//Debug.Log("Not converged: " + equations[i].ToString());
				continue;
			}
			return false;
		}
		return true;
	}

	private void StoreParams() {
		for(var i = 0; i < _parameters.Count; i++) {
			_oldParamValues[i] = _parameters[i].value;
		}
	}

	private void RevertParams() {
		for(var i = 0; i < _parameters.Count; i++) {
			_parameters[i].value = _oldParamValues[i];
		}
	}

	private static Expression[,] WriteJacobian(List<Expression> equations, List<Param> parameters) {
		var j = new Expression[equations.Count, parameters.Count];
		for(var r = 0; r < equations.Count; r++) {
			var eq = equations[r];
			for(var c = 0; c < parameters.Count; c++) {
				var u = parameters[c];
				j[r, c] = eq.Deriv(u);
				/*
			if(!J[r, c].IsZeroConst()) {
				Debug.Log(J[r, c].ToString() + "\n");
			}
			*/
			}
		}
		return j;
	}

	public bool HasDragged() {
		return _equations.Any(e => e.IsDrag());
	}

	public void EvalJacobian(Expression[,] j, ref double[,] a, bool clearDrag) {
		UpdateDirty();
		for(var r = 0; r < j.GetLength(0); r++) {
			if(clearDrag && _equations[r].IsDrag()) {
				for(var c = 0; c < j.GetLength(1); c++) {
					a[r, c] = 0.0;
				}
				continue;
			}
			for(var c = 0; c < j.GetLength(1); c++) {
				a[r, c] = j[r, c].Eval();
			}
		}
	}

	public void SolveLeastSquares(double[,] a, double[] b, ref double[] x) {

		// A^T * A * X = A^T * B
		var rows = a.GetLength(0);
		var cols = a.GetLength(1);

		for(var r = 0; r < rows; r++) {
			for(var c = 0; c < rows; c++) {
				var sum = 0.0;
				for(var i = 0; i < cols; i++) {
					if(a[c, i] == 0 || a[r, i] == 0) continue;
					sum += a[r, i] * a[c, i];
				}
				_aat[r, c] = sum;
			}
		}

		GaussianMethod.Solve(_aat, b, ref _z);

		for(var c = 0; c < cols; c++) {
			var sum = 0.0;
			for(var r = 0; r < rows; r++) {
				sum += _z[r] * a[r, c];
			}
			x[c] = sum;
		}

	}

	public void Clear() {
		_parameters.Clear();
		_currentParams.Clear();
		_equations.Clear();
		_sourceEquations.Clear();
		isDirty = true;
		UpdateDirty();
	}

	public bool TestRank(out int dof) {
		EvalJacobian(_j, ref _a, clearDrag:false);
		var rank = GaussianMethod.Rank(_a);
		dof = _a.GetLength(1) - rank;
		return rank == _a.GetLength(0);
	}

	private void UpdateDirty() {
		if(isDirty) {
			_equations = _sourceEquations.Select(e => e.DeepClone()).ToList();
			_currentParams = _parameters.ToList();
			/*
		foreach(var e in equations) {
			e.ReduceParams(currentParams);
		}*/
			//currentParams = parameters.Where(p => equations.Any(e => e.IsDependOn(p))).ToList();
			_subs = SolveBySubstitution();

			_j = WriteJacobian(_equations, _currentParams);
			_a = new double[_j.GetLength(0), _j.GetLength(1)];
			_b = new double[_equations.Count];
			_x = new double[_currentParams.Count];
			_z = new double[_a.GetLength(0)];
			_aat = new double[_a.GetLength(0), _a.GetLength(0)];
			_oldParamValues = new double[_parameters.Count];
			isDirty = false;
			dofChanged = true;
		}
	}

	private void BackSubstitution(Dictionary<Param, Param> subs) {
		if(subs == null) return;
		for(var i = 0; i < _parameters.Count; i++) {
			var p = _parameters[i];
			if(!subs.ContainsKey(p)) continue;
			p.value = subs[p].value;
		}
	}

	private Dictionary<Param, Param> SolveBySubstitution() {
		var subs = new Dictionary<Param, Param>();

		for(var i = 0; i < _equations.Count; i++) {
			var eq = _equations[i];
			if(!eq.IsSubstitionForm()) continue;
			var a = eq.GetSubstitutionParamA();
			var b = eq.GetSubstitutionParamB();
			if(Math.Abs(a.value - b.value) > GaussianMethod.Epsilon) continue;
			if(!_currentParams.Contains(b)) {
				var t = a;
				a = b;
				b = t;
			}
			// TODO: Check errors
			//if(!parameters.Contains(b)) {
			//	continue;
			//}

			foreach(var k in subs.Keys.ToList()) {
				if(subs[k] == b) {
					subs[k] = a;
				}
			}
			subs[b] = a;
			_equations.RemoveAt(i--);
			_currentParams.Remove(b);

			for(var j = 0; j < _equations.Count; j++) {
				_equations[j].Substitute(b, a);
			}
		}
		return subs;
	}

	public string stats { get; private set; }
	public bool dofChanged { get; private set; }

	public SolveResult Solve() {
		dofChanged = false;
		UpdateDirty();
		StoreParams();
		var steps = 0;
		do {
			var isDragStep = steps <= dragSteps;
			Eval(ref _b, clearDrag: !isDragStep);
			/*
		if(steps > 0) {
			BackSubstitution(subs);
			return SolveResult.POSTPONE;
		}
		*/
			if(IsConverged(checkDrag: isDragStep)) {
				if(steps > 0) {
					dofChanged = true;
					// TODO		Debug.Log(String.Format("solved {0} equations with {1} unknowns in {2} steps", equations.Count, currentParams.Count, steps));
				}
				stats = String.Format("eqs:{0}\nunkn: {1}", _equations.Count, _currentParams.Count);
				BackSubstitution(_subs);
				return SolveResult.Okay;
			}
			EvalJacobian(_j, ref _a, clearDrag: !isDragStep);
			SolveLeastSquares(_a, _b, ref _x);
			for(var i = 0; i < _currentParams.Count; i++) {
				_currentParams[i].value -= _x[i];
			}
		} while(steps++ <= maxSteps);
		IsConverged(checkDrag: false, printNonConverged: true);
		if(revertWhenNotConverged) {
			RevertParams();
			dofChanged = false;
		}
		return SolveResult.DidntConvege;
	}
}
