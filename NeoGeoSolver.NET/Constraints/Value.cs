using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public abstract class Value : Constraint
{
  protected Param value = new("value");
  public bool Reference { get; set; }

  private Vector3 _position;

  public override IEnumerable<Param> Parameters
  {
    get
    {
      if (!Reference)
      {
        yield break;
      }

      yield return value;
    }
  }

  public double GetValue()
  {
    return value.Value;
  }

  public void SetValue(double v)
  {
    value.Value = v;
  }

  public Param GetValueParam()
  {
    return value;
  }

  protected virtual bool OnSatisfy()
  {
    var sys = new EquationSystem();
    sys.revertWhenNotConverged = false;
    sys.AddParameter(value);
    sys.AddEquations(Equations);
    return sys.Solve() == EquationSystem.SolveResult.Okay;
  }

  public bool Satisfy()
  {
    var result = OnSatisfy();
    if (!result)
    {
      // TODO   Debug.LogWarning(GetType() + " satisfy failed!");
    }

    return result;
  }
}
