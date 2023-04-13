namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class LineArcDistance_Tests
{
  [Test]
  public void Distance_tangent_works()
  {
    var p0 = new Point(10, 0, 0);
    var p1 = new Point(15, 10, 0);
    var line = new Line(p0, p1);
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var startAngle = new Param("startAngle", 0);
    var endAngle = new Param("endAngle", Math.PI / 2d);
    var arc = new Arc(centre, radius, startAngle, endAngle);
    var constr = new LineArcDistance(line, arc);
    constr.SetValue(0);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(p1.X);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      p1.X.Value.Should().BeApproximately(10, 1e-4);
    }
  }
  
  [Test]
  public void Distance_works()
  {
    var p0 = new Point(20, 0, 0);
    var p1 = new Point(10, 10, 0);
    var line = new Line(p0, p1);
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var startAngle = new Param("startAngle", 0);
    var endAngle = new Param("endAngle", Math.PI / 2d);
    var arc = new Arc(centre, radius, startAngle, endAngle);
    var constr = new LineArcDistance(line, arc);
    constr.SetValue(10);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(p1.X);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      p1.X.Value.Should().BeApproximately(20, 1e-4);
    }
  }
}
