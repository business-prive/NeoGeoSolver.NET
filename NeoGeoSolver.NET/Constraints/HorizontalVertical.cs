using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class HorizontalVertical : Constraint {
	private ExpressionVector p0Exp {
		get {
			return GetPointInPlane(0);
		}
	}

	private ExpressionVector p1Exp {
		get {
			return GetPointInPlane(1);
		}
	}

	private ExpressionVector GetPointInPlane(int index) {
		if(HasEntitiesOfType(EntityType.Point, 2)) {
			return GetEntityOfType(EntityType.Point, index).PointExpInPlane();
		}
		return GetEntityOfType(EntityType.Line, 0).GetPointAtInPlane(index);
	}

	public HorizontalVerticalOrientation orientation = HorizontalVerticalOrientation.Ox;

	public HorizontalVertical(IEntity p0, IEntity p1)
	{
		AddEntity(p0);
		AddEntity(p1);
	}

	public HorizontalVertical(IEntity line)
	{
		AddEntity(line);
	}

	public override IEnumerable<Expression> equations {
		get {
			switch(orientation) {
				case HorizontalVerticalOrientation.Ox: yield return p0Exp.x - p1Exp.x; break;
				case HorizontalVerticalOrientation.Oy: yield return p0Exp.y - p1Exp.y; break;
				case HorizontalVerticalOrientation.Oz: yield return p0Exp.z - p1Exp.z; break;
			}
		}
	}
}
