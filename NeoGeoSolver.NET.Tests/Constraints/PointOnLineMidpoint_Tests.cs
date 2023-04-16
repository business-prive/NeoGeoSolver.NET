namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointOnLineMidpoint_Tests
{
  [Test]
  public void OnMidpoint_works()
  {
    var point = new Point(3, 3, 0);
    var p0 = new Point(0, 0, 0);
    var p1 = new Point(10, 10, 0);
    var line = new Line(p0, p1);
    var constr = new PointOnLineMidpoint(point, line);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(point.X);
    eqnSys.AddParameter(point.Y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      point.X.Value.Should().BeApproximately(5, 1e-6);
      point.Y.Value.Should().BeApproximately(5, 1e-6);
    }
  }
  
}
