using System.Diagnostics;
using System.Numerics;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Entities;

public class Function : Entity, ISegmentaryEntity {
	public Point p0;
	public Point p1;
	public Point c;

	private string function_x;
	private string function_y;
	private int subdivision_ = 16;
	public int subdivision {
		get {
			return subdivision_;
		}
		set {
			subdivision_ = value;
		}
	}

	private ExpressionBasis2d basis = new ExpressionBasis2d();

	private bool tBeginFixed_ = false;
	public bool tBeginFixed {
		get {
			return tBeginFixed_;
		}
		set {
			tBeginFixed_ = value;
		}
	}

	private bool tEndFixed_ = false;
	public bool tEndFixed {
		get {
			return tEndFixed_;
		}
		set {
			tEndFixed_ = value;
			//sketch.(entities:true, topo:true);
		}
	}

	public string x {
		get {
			return function_x;
		}
		set {
			if(function_x == value) return;
			function_x = value;
			parser.SetString(function_x);
			var e = parser.Parse();
			if(e != null) {
				exp.x = e;
				Debug.Log("x = " + e.ToString());
				sketch.MarkDirtySketch(entities:true, topo:true);
			}
		}
	}

	public string y {
		get {
			return function_y;
		}
		set {
			if(function_y == value) return;
			function_y = value;
			parser.SetString(function_y);
			var e = parser.Parse();
			if(e != null) {
				exp.y = e;
				// TODO		Debug.Log("y = " + e.ToString());
			}
		}
	}

	private ExpressionParser parser;
	private ExpressionVector exp = new ExpressionVector(0.0, 0.0, 0.0);
	private Param t = new Param("t");
	private Param t0 = new Param("t0", 0.0);
	private Param t1 = new Param("t1", 1.0);

	private void InitParser() {
		parser = new ExpressionParser("0");
		parser.parameters.Add(t);
		x = "t";
		y = "cos(t * pi)";

	}

	public Function(Sketch.Sketch sk) : base(sk) {
		p0 = AddChild(new Point(sk));
		p1 = AddChild(new Point(sk));
		c = AddChild(new Point(sk));
		InitParser();
	}

	public override IEntityType type { get { return IEntityType.Function; } }
	
	public ExpressionVector GetExpClone(Expression t) {
		var e = new ExpressionVector(exp.x.DeepClone(), exp.y.DeepClone(), 0.0);
		if(t != null) {
			e.x.Substitute(this.t, t);
			e.y.Substitute(this.t, t);
			e.z.Substitute(this.t, t);
		}
		return e;
	}

	public override IEnumerable<Expression> equations {
		get {
			ExpressionVector e0 = basis.TransformPosition(GetExpClone(t0));

			var eq0 = e0 - p0.exp;
			yield return eq0.x;
			yield return eq0.y;

			//if(!p0.IsCoincidentWith(p1)) {
			ExpressionVector e1 = basis.TransformPosition(GetExpClone(t1));

			var eq1 = e1 - p1.exp;
			yield return eq1.x;
			yield return eq1.y;
			//}

			var eqc = basis.p - c.exp;
			yield return eqc.x;
			yield return eqc.y;

			foreach(var e in basis.equations) yield return e;
		}
	}

	public override IEnumerable<Point> points {
		get {
			yield return p0;
			yield return p1;
			yield return c;
		}
	}

	public override IEnumerable<Param> parameters {
		get {
			if(!tBeginFixed) yield return t0;
			if(!tEndFixed) yield return t1;
			foreach(var p in basis.parameters) yield return p;
		}
	}

	public Point begin { get { return p0; } }
	public Point end { get { return p1; } }
	public IEnumerable<Vector3> segmentPoints {
		get {
			Param pOn = new Param("pOn");
			var on = PointOn(pOn);
			var subdiv = (int)Math.Ceiling(subdivision * Math.Abs(t1.value - t0.value));
			for(int i = 0; i <= subdiv; i++) {
				pOn.value = (double)i / subdiv;
				yield return on.Eval();
			}
		}
	}	

	public override ExpressionVector PointOn(Expression t) {
		var newt = t0.exp + (t1.exp - t0.exp) * t;
		return basis.TransformPosition(GetExpClone(newt));
	}

	public override Expression Length() {
		return null;
	}

	public override Expression Radius() {
		return null;
	}

	public override ExpressionVector Center() {
		return null;
	}
}
