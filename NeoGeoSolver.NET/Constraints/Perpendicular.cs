using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Perpendicular : Constraint {
	public enum Option {
		LeftHand,
		RightHand
	}

	private Option option_;

	public Option option { get { return option_; } set { option_ = value; sketch.MarkDirtySketch(topo:true); } }
	protected override Enum optionInternal { get { return option; } set { option = (Option)value; } }

	public Perpendicular(Sketch.Sketch sk) : base(sk) { }

	public Perpendicular(Sketch.Sketch sk, IEntity l0, IEntity l1) : base(sk) {
		AddEntity(l0);
		AddEntity(l1);
		ChooseBestOption();
	}

	public override IEnumerable<Expression> equations {
		get {
			var l0 = GetEntityOfType(IEntityType.Line, 0);
			var l1 = GetEntityOfType(IEntityType.Line, 1);

			ExpressionVector d0 = l0.GetPointAtInPlane(0, sketch.plane) - l0.GetPointAtInPlane(1, sketch.plane);
			ExpressionVector d1 = l1.GetPointAtInPlane(0, sketch.plane) - l1.GetPointAtInPlane(1, sketch.plane);

			Expression angle = sketch.is3d ? ConstraintExp.angle3d(d0, d1) : ConstraintExp.angle2d(d0, d1);
			switch(option) {
				case Option.LeftHand: yield return angle - Math.PI / 2.0; break;
				case Option.RightHand: yield return angle + Math.PI / 2.0; break;
			}
		}
	}
}
