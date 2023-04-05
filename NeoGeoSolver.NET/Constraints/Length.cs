using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Length : Value {
  private readonly Line _line;
	public Length(Line line)
	{
		_line = line;
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			yield return _line.Length() - value;
		}
	}
}
