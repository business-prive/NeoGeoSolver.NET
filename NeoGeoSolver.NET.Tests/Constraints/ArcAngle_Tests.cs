namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class ArcAngle_Tests
{
  [Test]
  public void Angle_works()
  {
    var center = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var startAngle = new Param("startAngle", 0);
    var endAngle = new Param("endAngle", Math.PI / 2d);
    var arc = new Arc(center, radius, startAngle, endAngle);
    var constr = new ArcAngle(arc);
    constr.SetValue(Math.PI / 4d);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(endAngle);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      endAngle.Value.Should().BeApproximately(Math.PI / 4d, 1e-6);
    }
  }
}
