using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointLineDistance : Value
{
	public Point point
	{
		get;set;
	}

	public Line line
	{
		get;set;
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

	public PointLineDistance(Point pt, Line line)
	{
		point = pt;
		this.line = line;
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
