namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class ArcAngle_Tests
{
  [Test]
  public void Angle_works()
  {
    var center = new Point(0, 0, 0);
    var pt0 = new Point(10, 0, 0);
    var pt1 = new Point(0, 10, 0);
    var arc = new Arc(pt0, pt1, center);
    var constr = new ArcAngle(arc);
    constr.SetValue(Math.PI / 4d);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(pt1.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      pt1.x.Value.Should().BeApproximately(10, 1e-6);
    }
  }
}
