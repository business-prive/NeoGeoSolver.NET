namespace NeoGeoSolver.NET.Solver;

public class Param
{
  public readonly string Name;
  public double Value;

  public Expression Expr { get; private set; }

  public Param(string name)
  {
    Name = name;
    Expr = new Expression(this);
  }

  public Param(string name, double value)
  {
    Name = name;
    Value = value;
    Expr = new Expression(this);
  }
}
