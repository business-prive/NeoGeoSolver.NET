using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class Angle : Value {
	private bool supplementary_;
	public bool supplementary {
		get {
			return supplementary_;
		}
		set {
			if(value == supplementary_) return;
			supplementary_ = value;
			if(HasEntitiesOfType(IEntityType.Arc, 1)) {
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
			var p = GetPointsExp(sketch.plane);
			ExpressionVector d0 = p[0] - p[1];
			ExpressionVector d1 = p[3] - p[2];
			bool angle360 = HasEntitiesOfType(IEntityType.Arc, 1);
			Expression angle = ConstraintExp.angle2d(d0, d1, angle360);
			yield return angle - value;
		}
	}

	private ExpressionVector[] GetPointsExp(IPlane plane) {
		var p = new ExpressionVector[4];
		if(HasEntitiesOfType(IEntityType.Point, 4)) {
			for(int i = 0; i < 4; i++) {
				p[i] = GetEntityOfType(IEntityType.Point, i).GetPointAtInPlane(0, plane);
			}
			if(supplementary) {
				SystemExt.Swap(ref p[2], ref p[3]);
			}
		} else 
		if(HasEntitiesOfType(IEntityType.Line, 2)) {
			var l0 = GetEntityOfType(IEntityType.Line, 0);
			p[0] = l0.GetPointAtInPlane(0, plane);
			p[1] = l0.GetPointAtInPlane(1, plane);
			var l1 = GetEntityOfType(IEntityType.Line, 1);
			p[2] = l1.GetPointAtInPlane(0, plane);
			p[3] = l1.GetPointAtInPlane(1, plane);
			if(supplementary) {
				SystemExt.Swap(ref p[2], ref p[3]);
			}
		} else 
		if(HasEntitiesOfType(IEntityType.Arc, 1)) {
			var arc = GetEntityOfType(IEntityType.Arc, 0);
			p[0] = arc.GetPointAtInPlane(0, plane);
			p[1] = arc.GetPointAtInPlane(2, plane);
			p[2] = arc.GetPointAtInPlane(2, plane);
			p[3] = arc.GetPointAtInPlane(1, plane);
			if(supplementary) {
				SystemExt.Swap(ref p[0], ref p[3]);
				SystemExt.Swap(ref p[1], ref p[2]);
			}
		}
		return p;
	}

	public override double LabelToValue(double label) {
		return label * Math.PI / 180.0;
	}

	public override double ValueToLabel(double value) {
		return value / Math.PI * 180.0;
	}
}
