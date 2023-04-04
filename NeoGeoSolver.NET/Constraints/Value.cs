using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public abstract class Value : Constraint {

  protected Param value = new("value");
  public bool reference { get; set; }

  private Vector3 _position;

  public override IEnumerable<Param> parameters {
    get {
      if(!reference) yield break;
      yield return value;
    }
  }

  public double GetValue() {
    return value.value;
  }

  public void SetValue(double v) {
    value.value = v;
  }

  public Param GetValueParam() {
    return value;
  }

  protected virtual bool OnSatisfy() {
    var sys = new EquationSystem();
    sys.revertWhenNotConverged = false;
    sys.AddParameter(value);
    sys.AddEquations(equations);
    return sys.Solve() == EquationSystem.SolveResult.Okay;
  }

  public bool Satisfy() {
    var result = OnSatisfy();
    if(!result) {
      // TODO   Debug.LogWarning(GetType() + " satisfy failed!");
    }
    return result;
  }
}
