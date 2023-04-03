using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Value : Constraint {

  protected Param value = new("value");
  private bool reference_;
  public bool reference {
    get {
      return reference_;
    }
    set {
      reference_ = value;
    }
  }

  private Vector3 position_;

  public override IEnumerable<Param> parameters {
    get {
      if(!reference) yield break;
      yield return value;
    }
  }

  public double GetValue() {
    return ValueToLabel(value.value);
  }

  public void SetValue(double v) {
    value.value = LabelToValue(v);
  }

  public Param GetValueParam() {
    return value;
  }

  public virtual double ValueToLabel(double value) {
    return value;
  }

  public virtual double LabelToValue(double label) {
    return label;
  }

  protected virtual bool OnSatisfy() {
    EquationSystem sys = new EquationSystem();
    sys.revertWhenNotConverged = false;
    sys.AddParameter(value);
    sys.AddEquations(equations);
    return sys.Solve() == EquationSystem.SolveResult.OKAY;
  }

  public bool Satisfy() {
    var result = OnSatisfy();
    if(!result) {
      // TODO   Debug.LogWarning(GetType() + " satisfy failed!");
    }
    return result;
  }
}
