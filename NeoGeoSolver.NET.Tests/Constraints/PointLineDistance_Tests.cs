namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public class PointLineDistance_Tests
{
  [TestCase(10, 10)]
  [TestCase(0, 0)]
  public void Distance_works(double dist, double yVal)
  {
    var point = new Point(0, 5, 0);
    var p0 = new Point(0, 0, 0);
    var p1 = new Point(10, 0, 0);
    var line = new Line(p0, p1);
    var constr = new PointLineDistance(point, line);
    constr.SetValue(dist);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(point.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      point.y.Value.Should().BeApproximately(yVal, 1e-6);
    }
  }
}
