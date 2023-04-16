namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointOnArcMidpoint_Tests
{
  [Test]
  public void OnMidpoint_quadrant_works()
  {
    var point = new Point(0, 10, 0);
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var startAngle = new Param("startAngle", 0);
    var endAngle = new Param("endAngle", Math.PI / 2d);
    var arc = new Arc(centre, radius, startAngle, endAngle);
    var constr = new PointOnArcMidpoint(point, arc);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(point.X);
    eqnSys.AddParameter(point.Y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      point.X.Value.Should().BeApproximately(7.071067811, 1e-6);
      point.Y.Value.Should().BeApproximately(7.071067811, 1e-6);
    }
  }

  [Test]
  public void OnMidpoint_semi_circle_works()
  {
    var point = new Point(5, 5, 0);
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var startAngle = new Param("startAngle", 0);
    var endAngle = new Param("endAngle", Math.PI);
    var arc = new Arc(centre, radius, startAngle, endAngle);
    var constr = new PointOnArcMidpoint(point, arc);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(point.X);
    eqnSys.AddParameter(point.Y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      point.X.Value.Should().BeApproximately(0, 1e-6);
      point.Y.Value.Should().BeApproximately(10, 1e-6);
    }
  }
}
