namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointsDistanceVertical_Tests
{
  [TestCase(10)]
  [TestCase(0)]
  public void PointsDistance_works(double dist)
  {
    var p0 = new Point(0, 0, 0);
    var p1 = new Point(0, 5, 0);
    var constr = new PointsDistanceVertical(p0, p1);
    constr.SetValue(dist);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(p1.Y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      p1.Y.Value.Should().BeApproximately(dist, 1e-6);
    }
  }
}
