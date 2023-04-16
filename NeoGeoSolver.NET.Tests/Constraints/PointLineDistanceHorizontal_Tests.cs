namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointLineDistanceHorizontal_Tests
{
  [Test]
  public void Distance_works()
  {
    var point = new Point(4, 3, 0);
    var p0 = new Point(0, 0, 0);
    var p1 = new Point(10, 10, 0);
    var line = new Line(p0, p1);
    var constr = new PointLineDistanceHorizontal(point, line);
    constr.SetValue(5);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(point.X);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      point.X.Value.Should().BeApproximately(8, 1e-4);
    }
  }
  
  [Test]
  public void Distance_horizontal_line_fails()
  {
    var point = new Point(4, 3, 0);
    var p0 = new Point(0, 10, 0);
    var p1 = new Point(10, 10, 0);
    var line = new Line(p0, p1);
    var constr = new PointLineDistanceHorizontal(point, line);
    constr.SetValue(5);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(point.X);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.DidntConvege);
    }
  }
}
