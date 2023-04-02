using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class HorizontalVertical : Constraint {

	public ExpressionVector p0exp {
		get {
			return GetPointInPlane(0, sketch.plane);
		}
	}

	public ExpressionVector p1exp {
		get {
			return GetPointInPlane(1, sketch.plane);
		}
	}

	private ExpressionVector GetPointInPlane(int index, IPlane plane) {
		if(HasEntitiesOfType(IEntityType.Point, 2)) {
			return GetEntityOfType(IEntityType.Point, index).PointExpInPlane(plane);
		}
		return GetEntityOfType(IEntityType.Line, 0).GetPointAtInPlane(index, plane);
	}

	public HorizontalVerticalOrientation orientation = HorizontalVerticalOrientation.OX;

	public HorizontalVertical(Sketch.Sketch sk) : base(sk) { }

	public HorizontalVertical(Sketch.Sketch sk, IEntity p0, IEntity p1) : base(sk) {
		AddEntity(p0);
		AddEntity(p1);
	}

	public HorizontalVertical(Sketch.Sketch sk, IEntity line) : base(sk) {
		AddEntity(line);
	}

	public override IEnumerable<Expression> equations {
		get {
			switch(orientation) {
				case HorizontalVerticalOrientation.OX: yield return p0exp.x - p1exp.x; break;
				case HorizontalVerticalOrientation.OY: yield return p0exp.y - p1exp.y; break;
				case HorizontalVerticalOrientation.OZ: yield return p0exp.z - p1exp.z; break;
			}
		}
	}
}
