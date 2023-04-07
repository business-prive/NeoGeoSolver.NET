namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointsCoincident_Tests
{
  [Test]
  public void Coincident_work()
  {
    var p0 = new Point(0, 0, 0);
    var p1 = new Point(5, 10, 0);
    var constr = new PointsCoincident(p0, p1);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(p1.x);
    eqnSys.AddParameter(p1.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      p1.x.Value.Should().BeApproximately(0, 1e-6);
      p1.y.Value.Should().BeApproximately(0, 1e-6);
    }
  }
  
}
