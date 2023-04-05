using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class Angle : Value {
	private bool _supplementary;
	public bool supplementary {
		get {
			return _supplementary;
		}
		set {
			if(value == _supplementary) return;
			_supplementary = value;
			if(HasEntitiesOfType(EntityType.Arc, 1)) {
				this.value.value = 2.0 * Math.PI - this.value.value;
			} else {
				this.value.value = -(Math.Sign(this.value.value) * Math.PI - this.value.value);
			}
		}
	}

  private readonly Point[] _points = new Point[4];
	public Angle(Point[] points)
	{
    if (points.Length != 4)
    {
      throw new ArgumentOutOfRangeException();
    }

		_points[0] = points[0];
		_points[1] = points[1];
		_points[2] = points[2];
		_points[3] = points[3];
		Satisfy();
	}

  private readonly Arc _arc;

	public Angle(Arc arc)
	{
		_arc = arc;
		value.value = Math.PI / 4;
		Satisfy();
	}

  private readonly Line _l0;
  private readonly Line _l1;

	public Angle(Line l0, Line l1)
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
			var angle360 = HasEntitiesOfType(EntityType.Arc, 1);
			var angle = ConstraintExp.Angle2d(d0, d1, angle360);
			yield return angle - value;
		}
	}

	private ExpressionVector[] GetPointsExp() {
		var p = new ExpressionVector[4];
		if(HasEntitiesOfType(EntityType.Point, 4)) {
			for(var i = 0; i < 4; i++) {
				p[i] = _points[i].exp;
			}
			if(supplementary) {
				SystemExt.Swap(ref p[2], ref p[3]);
			}
		} else 

		if(HasEntitiesOfType(EntityType.Line, 2)) {
			var l0 = _l0;
			p[0] = l0.p0.exp;
			p[1] = l0.p1.exp;
			var l1 = _l1;
			p[2] = l1.p0.exp;
			p[3] = l1.p1.exp;
			if(supplementary) {
				SystemExt.Swap(ref p[2], ref p[3]);
			}
		} else 

		if(HasEntitiesOfType(EntityType.Arc, 1)) {
			var arc = _arc;
			p[0] = arc.p0.exp;
			p[1] = arc.center.exp;
			p[2] = arc.center.exp;
			p[3] = arc.p1.exp;
			if(supplementary) {
				SystemExt.Swap(ref p[0], ref p[3]);
				SystemExt.Swap(ref p[1], ref p[2]);
			}
		}
		return p;
	}
}
