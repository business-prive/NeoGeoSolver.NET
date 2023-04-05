using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Entities;

public class Function : Entity {
	public Point p0 = new();
	public Point p1 = new();
	public Point c = new();

	private string _functionX;
	private string _functionY;
	public int subdivision { get; set; } = 16;

	private ExpressionBasis2d _basis = new();

	public bool tBeginFixed { get; set; } = false;

	public bool tEndFixed { get; set; } = false;

	public string x {
		get {
			return _functionX;
		}
		set {
			if(_functionX == value) return;
			_functionX = value;
			_parser.SetString(_functionX);
			var e = _parser.Parse();
			if(e != null) {
				_exp.x = e;
				// TODO		Debug.Log("x = " + e.ToString());
				
			}
		}
	}

	public string y {
		get {
			return _functionY;
		}
		set {
			if(_functionY == value) return;
			_functionY = value;
			_parser.SetString(_functionY);
			var e = _parser.Parse();
			if(e != null) {
				_exp.y = e;
				// TODO		Debug.Log("y = " + e.ToString());
			}
		}
	}

	private ExpressionParser _parser;
	private ExpressionVector _exp = new(0.0, 0.0, 0.0);
	private Param _t = new("t");
	private Param _t0 = new("t0", 0.0);
	private Param _t1 = new("t1", 1.0);

	private void InitParser() {
		_parser = new ExpressionParser("0");
		_parser.parameters.Add(_t);
		x = "t";
		y = "cos(t * pi)";
	}

	public Function()
	{
		InitParser();
	}

	private ExpressionVector GetExpClone(Expression t) {
		var e = new ExpressionVector(_exp.x.DeepClone(), _exp.y.DeepClone(), 0.0);
		if(t != null) {
			e.x.Substitute(this._t, t);
			e.y.Substitute(this._t, t);
			e.z.Substitute(this._t, t);
		}
		return e;
	}

	public override IEnumerable<Expression> Equations {
		get {
			var e0 = _basis.TransformPosition(GetExpClone(_t0));

			var eq0 = e0 - p0.exp;
			yield return eq0.x;
			yield return eq0.y;

			var e1 = _basis.TransformPosition(GetExpClone(_t1));
			var eq1 = e1 - p1.exp;
			yield return eq1.x;
			yield return eq1.y;

			var eqc = _basis.p - c.exp;
			yield return eqc.x;
			yield return eqc.y;

			foreach(var e in _basis.equations) yield return e;
		}
	}

	public override IEnumerable<Param> Parameters {
		get {
			if(!tBeginFixed) yield return _t0;
			if(!tEndFixed) yield return _t1;
			foreach(var p in _basis.parameters) yield return p;
		}
	}

	public override ExpressionVector PointOn(Expression t) {
		var newt = _t0.exp + (_t1.exp - _t0.exp) * t;
		return _basis.TransformPosition(GetExpClone(newt));
	}
}
