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

	public Angle(IEntity[] points)
	{
		foreach(var p in points) {
			AddEntity(p);
		}
		Satisfy();
	}

	public Angle(IEntity arc)
	{
		AddEntity(arc);
		value.value = Math.PI / 4;
		Satisfy();
	}

	public Angle(IEntity l0, IEntity l1)
	{
		AddEntity(l0);
		AddEntity(l1);
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
				p[i] = GetEntityOfType(EntityType.Point, i).GetPointAtInPlane(0);
			}
			if(supplementary) {
				SystemExt.Swap(ref p[2], ref p[3]);
			}
		} else 
		if(HasEntitiesOfType(EntityType.Line, 2)) {
			var l0 = GetEntityOfType(EntityType.Line, 0);
			p[0] = l0.GetPointAtInPlane(0);
			p[1] = l0.GetPointAtInPlane(1);
			var l1 = GetEntityOfType(EntityType.Line, 1);
			p[2] = l1.GetPointAtInPlane(0);
			p[3] = l1.GetPointAtInPlane(1);
			if(supplementary) {
				SystemExt.Swap(ref p[2], ref p[3]);
			}
		} else 
		if(HasEntitiesOfType(EntityType.Arc, 1)) {
			var arc = GetEntityOfType(EntityType.Arc, 0);
			p[0] = arc.GetPointAtInPlane(0);
			p[1] = arc.GetPointAtInPlane(2);
			p[2] = arc.GetPointAtInPlane(2);
			p[3] = arc.GetPointAtInPlane(1);
			if(supplementary) {
				SystemExt.Swap(ref p[0], ref p[3]);
				SystemExt.Swap(ref p[1], ref p[2]);
			}
		}
		return p;
	}
}
