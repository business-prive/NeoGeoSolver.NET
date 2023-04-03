using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointLineDistance : Value
{
	public IEntity point
	{
		get
		{
			return GetEntity(0);
		}
		set
		{
			SetEntity(0, value);
		}
	}

	public IEntity line
	{
		get
		{
			return GetEntity(1);
		}
		set
		{
			SetEntity(1, value);
		}
	}

	public ExpressionVector pointExp
	{
		get
		{
			return point.PointExpInPlane(sketch.plane);
		}
	}

	public ExpressionVector lineP0Exp
	{
		get
		{
			return line.PointsInPlane(sketch.plane).ToArray()[0];
		}
	}

	public ExpressionVector lineP1Exp
	{
		get
		{
			return line.PointsInPlane(sketch.plane).ToArray()[1];
		}
	}

	public PointLineDistance(IEntity p0, IEntity p1)
	{
		AddEntity(p0);
		AddEntity(p1);
		SetValue(1.0);
		Satisfy();
	}

	public override IEnumerable<Expression> equations
	{
		get
		{
			yield return ConstraintExp.pointLineDistance(pointExp, lineP0Exp, lineP1Exp, sketch.is3d) - value;
		}
	}
}
