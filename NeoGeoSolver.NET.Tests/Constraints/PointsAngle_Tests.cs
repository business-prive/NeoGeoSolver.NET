namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointsAngle_Tests
{
  [Test]
  public void Angle_works()
  {
    var l0pt0 = new Point(0, 0, 0);
    var l0pt1 = new Point(0, 10, 0);
    var l1pt0 = new Point(0, 0, 0);
    var l1pt1 = new Point(10, 0, 0);
    var pts = new[] {l0pt0, l0pt1, l1pt0, l1pt1};
    var constr = new PointsAngle(pts);
    constr.SetValue(Math.PI / 4d);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(l1pt1.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      l1pt1.y.Value.Should().BeApproximately(-10, 1e-6);
    }
  }
}
