namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointArcDistance_Tests
{
  [TestCase(10, 20)]
  [TestCase(0, 10)]
  public void Distance_works(double dist, double yVal)
  {
    var point = new Point(0, 10, 0);
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var startAngle = new Param("startAngle", 0);
    var endAngle = new Param("endAngle", Math.PI / 2d);
    var arc = new Arc(centre, radius, startAngle, endAngle);
    var constr = new PointArcDistance(point, arc);
    constr.SetValue(dist);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(point.Y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      point.Y.Value.Should().BeApproximately(yVal, 1e-6);
    }
  }
}
