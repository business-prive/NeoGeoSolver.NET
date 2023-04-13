namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class ArcRadius_Tests
{
  [Test]
  public void Radius_works()
  {
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var startAngle = new Param("startAngle", 0);
    var endAngle = new Param("endAngle", Math.PI / 2d);
    var arc = new Arc(centre, radius, startAngle, endAngle);
    var constr = new ArcRadius(arc);
    constr.SetValue(20);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(arc.Radius);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      arc.Radius.Value.Should().BeApproximately(20, 1e-6);
    }
  }
}
