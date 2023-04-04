using System.Numerics;
using NeoGeoSolver.NET.Constraints;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Point : Entity
{
	public Param x = new("x");
	public Param y = new("y");
	public Param z = new("z");

	public override EntityType type
	{
		get
		{
			return EntityType.Point;
		}
	}

	public override IEnumerable<Expression> equations
	{
		get
		{
			yield break;
		}
	}

	public Vector3 GetPosition()
	{
		if (transform != null)
		{
			return exp.Eval();
		}

		return new Vector3((float) x.value, (float) y.value, (float) z.value);
	}

	public void SetPosition(Vector3 pos)
	{
		if (transform != null) return;
		x.value = pos.X;
		y.value = pos.Y;
	}

	public Vector3 pos
	{
		get
		{
			return GetPosition();
		}
		set
		{
			SetPosition(value);
		}
	}

	private ExpressionVector _exp;

	public ExpressionVector exp
	{
		get
		{
			if (_exp == null)
			{
				_exp = new ExpressionVector(x, y, z);
			}

			if (transform != null)
			{
				return transform(_exp);
			}

			return _exp;
		}
	}

	public override IEnumerable<Param> parameters
	{
		get
		{
			yield return x;
			yield return y;
		}
	}

	private bool IsCoincidentWith(IEntity point, IEntity exclude)
	{
		if (point.IsSameAs(this)) return true;
		return false;
	}

	public bool IsCoincidentWith(IEntity point)
	{
		return IsCoincidentWith(point, null);
	}

	public override ExpressionVector PointOn(Expression t)
	{
		return exp;
	}
}
