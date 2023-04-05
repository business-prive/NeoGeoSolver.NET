namespace NeoGeoSolver.NET.Solver;

public class Param
{
  public string Name;
  public double Value;

  public Expression Expr { get; private set; }

  public Param(string name)
  {
    this.Name = name;
    Expr = new Expression(this);
  }

  public Param(string name, double value)
  {
    this.Name = name;
    this.Value = value;
    Expr = new Expression(this);
  }
}
