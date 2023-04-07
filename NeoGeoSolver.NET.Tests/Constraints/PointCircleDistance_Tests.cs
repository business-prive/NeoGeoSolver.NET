namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointCircleDistance_Tests
{
  [TestCase(10, 20)]
  [TestCase(0, 10)]
  public void Distance_works(double dist, double yVal)
  {
    var point = new Point(0, 10, 0);
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var circle = new Circle(centre, radius);
    var constr = new PointCircleDistance(point, circle);
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