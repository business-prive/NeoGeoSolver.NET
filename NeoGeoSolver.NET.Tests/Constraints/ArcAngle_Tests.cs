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
    var arc = new Arc(center, pt0, pt1);
    var constr = new ArcAngle(arc);
    constr.SetValue(Math.PI / 4d);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(pt1.X);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      pt1.X.Value.Should().BeApproximately(10, 1e-6);
    }
  }
}
