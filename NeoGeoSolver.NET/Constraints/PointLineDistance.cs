using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointLineDistance : Value
{
	public Point point
	{
		get
		{
			return (Point) GetEntity(0);
		}
		set
		{
			SetEntity(0, value);
		}
	}

	public Line line
	{
		get
		{
			return (Line) GetEntity(1);
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
			return point.exp;
		}
	}

	public ExpressionVector lineP0Exp
	{
		get
		{
			return line.p0.exp;
		}
	}

	public ExpressionVector lineP1Exp
	{
		get
		{
			return line.p1.exp;
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
			yield return ConstraintExp.PointLineDistance(pointExp, lineP0Exp, lineP1Exp) - value;
		}
	}
}
