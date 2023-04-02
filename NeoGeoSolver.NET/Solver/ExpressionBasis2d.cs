using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Solver;

public class ExpressionBasis2d {
  private Param px, py;
  private Param ux, uy;
  private Param vx, vy;

  public ExpressionVector u { get; private set; }
  public ExpressionVector v { get; private set; }
  public ExpressionVector p { get; private set; }

  public ExpressionBasis2d() {
    px = new Param("ux", 0.0);
    py = new Param("uy", 0.0);

    ux = new Param("ux", 1.0);
    uy = new Param("uy", 0.0);

    vx = new Param("vx", 0.0);
    vy = new Param("vy", 1.0);

    p = new ExpressionVector(px, py, 0.0);
    u = new ExpressionVector(ux, uy, 0.0);
    v = new ExpressionVector(vx, vy, 0.0);
  }

  public void SetPosParams(Param x, Param y) {
    px = x;
    py = y;
    p.x = px;
    p.y = py;
  }

  public IEnumerable<Param> parameters {
    get {
      yield return ux;
      yield return uy;
      yield return vx;
      yield return vy;
      yield return px;
      yield return py;
    }
  }

  public override string ToString() {
    string result = "";
    foreach(var p in parameters) {
      result += p.value.ToStr() + " ";
    }
    return result;
  }

  public void FromString(string str) {
    char[] sep = { ' ' };
    var values = str.Split(sep, StringSplitOptions.RemoveEmptyEntries);
    int i = 0;
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

  public void markUnchanged() {
    parameters.ForEach(pp => pp.changed = false);
  }
}
