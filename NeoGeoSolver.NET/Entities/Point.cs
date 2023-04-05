using System.Numerics;
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
		return new Vector3((float) x.value, (float) y.value, (float) z.value);
	}

	public void SetPosition(Vector3 pos)
	{
		x.value = pos.X;
		y.value = pos.Y;
    z.value = pos.Z;
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

	private bool IsCoincidentWith(Point point, Point exclude)
	{
		if (IsSameAs(point, this)) return true;
		return false;
	}

	public bool IsCoincidentWith(Point point)
	{
		return IsCoincidentWith(point, null);
	}

	private static bool IsSameAs(IEntity e0, IEntity e1) {
    if(e0 == null) return e1 == null;
    if(e1 == null) return e0 == null;
    return e0 == e1 || e0.type == e1.type;
  }
  
	public override ExpressionVector PointOn(Expression t)
	{
		return exp;
	}
}
