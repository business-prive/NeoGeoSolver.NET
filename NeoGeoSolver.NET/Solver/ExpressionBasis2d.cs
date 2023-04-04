using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionBasis2d {
  private Param _px = new("px", 0.0), _py = new("py", 0.0);
  private Param _ux = new("ux", 1.0), _uy = new("uy", 0.0);
  private Param _vx = new("vx", 0.0), _vy = new("vy", 1.0);

  public ExpressionVector u { get; private set; }
  public ExpressionVector v { get; private set; }
  public ExpressionVector p { get; private set; }

  public ExpressionBasis2d() {
    p = new ExpressionVector(_px, _py, 0.0);
    u = new ExpressionVector(_ux, _uy, 0.0);
    v = new ExpressionVector(_vx, _vy, 0.0);
  }

  public void SetPosParams(Param x, Param y) {
    _px = x;
    _py = y;
    p.x = _px;
    p.y = _py;
  }

  public IEnumerable<Param> parameters {
    get {
      yield return _ux;
      yield return _uy;
      yield return _vx;
      yield return _vy;
      yield return _px;
      yield return _py;
    }
  }

  public override string ToString() {
    var result = "";
    foreach(var p in parameters) {
      result += p.value.ToStr() + " ";
    }
    return result;
  }

  public void FromString(string str) {
    char[] sep = { ' ' };
    var values = str.Split(sep, StringSplitOptions.RemoveEmptyEntries);
    var i = 0;
    foreach(var p in parameters) {
      p.value = values[i].ToDouble();
      i++;
    }
  }

  public ExpressionVector TransformPosition(ExpressionVector pos) {
    return pos.x * u + pos.y * v + p;
  }

  public ExpressionVector TransformDirection(ExpressionVector dir) {
    return dir.x * u + dir.y * v;
  }

  public void GenerateEquations(EquationSystem sys) {
    sys.AddParameters(parameters);
    sys.AddEquations(equations);
  }

  public IEnumerable<Expression> equations {
    get {
      yield return u.Magnitude() - 1.0;
      yield return v.Magnitude() - 1.0;
      var cross = ExpressionVector.Cross(u, v);
      var dot = ExpressionVector.Dot(u, v);
      yield return Expression.Atan2(cross.Magnitude(), dot) - Math.PI / 2;
    }
  }

  public bool changed {
    get {
      return parameters.Any(p => p.changed);
    }
  }

  public void MarkUnchanged() {
    parameters.ForEach(pp => pp.changed = false);
  }
}
