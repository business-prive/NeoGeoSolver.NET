using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class LinesAngle : Value {
	private bool _supplementary;
	public bool supplementary {
		get {
			return _supplementary;
		}
		set {
			if(value == _supplementary) return;
			supplementary = value;
			this.value.value = -(Math.Sign(this.value.value) * Math.PI - this.value.value);
		}
	}

  private readonly Line _l0;
  private readonly Line _l1;

	public LinesAngle(Line l0, Line l1)
	{
		_l0 = l0;
		_l1 = l1;
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			var p = GetPointsExp();
			var d0 = p[0] - p[1];
			var d1 = p[3] - p[2];
			var angle = ConstraintExp.Angle2d(d0, d1);
			yield return angle - value;
		}
	}

	private ExpressionVector[] GetPointsExp() {
		var p = new ExpressionVector[4];
			var l0 = _l0;
			p[0] = l0.Point0.exp;
			p[1] = l0.Point1.exp;
			var l1 = _l1;
			p[2] = l1.Point0.exp;
			p[3] = l1.Point1.exp;
			if(supplementary) {
				SystemExt.Swap(ref p[2], ref p[3]);
			}

		return p;
	}
}
