using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class PointsDistance : Value {

	public ExpressionVector p0Exp { get { return p0.exp; } }
	public ExpressionVector p1Exp { get { return p1.exp; } }

	public Point p0 { get;}
	public Point p1 { get;}

	public PointsDistance(Point pt0, Point pt1)
	{
		p0 = pt0;
		p1 = pt1;
		Satisfy();
	}

  public Line line {get;set;}
	public PointsDistance(Line line)
	{
		this.line = line;
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			yield return (p1Exp - p0Exp).Magnitude() - value.exp;
		}
	}
}
