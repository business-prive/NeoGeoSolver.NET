using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsDistance : Value {

	public ExpressionVector p0Exp { get { return GetPointInPlane(0); } }
	public ExpressionVector p1Exp { get { return GetPointInPlane(1); } }

	public PointsDistance(IEntity p0, IEntity p1)
	{
		AddEntity(p0);
		AddEntity(p1);
		Satisfy();
	}

	public PointsDistance(IEntity line)
	{
		AddEntity(line);
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			yield return (p1Exp - p0Exp).Magnitude() - value.exp;
		}
	}

	private ExpressionVector GetPointInPlane(int i) {
		if(HasEntitiesOfType(EntityType.Line, 1)) {
			return GetEntityOfType(EntityType.Line, 0).GetPointAtInPlane(i);
		} else 
		if(HasEntitiesOfType(EntityType.Point, 2)) {
			return GetEntityOfType(EntityType.Point, i).GetPointAtInPlane(0);
		}
		return null;
	}
}
