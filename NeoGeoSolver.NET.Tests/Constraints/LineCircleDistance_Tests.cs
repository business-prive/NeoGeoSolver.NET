namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class LineCircleDistance_Tests
{
  [Test]
  public void Distance_tangent_works()
  {
    var p0 = new Point(10, 0, 0);
    var p1 = new Point(15, 10, 0);
    var line = new Line(p0, p1);
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var circle = new Circle(centre, radius);
    var constr = new LineCircleDistance(line, circle);
    constr.SetValue(0);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(p1.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      p1.x.Value.Should().BeApproximately(10, 1e-4);
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
    var circle = new Circle(centre, radius);
    var constr = new LineCircleDistance(line, circle);
    constr.SetValue(10);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(p1.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      p1.x.Value.Should().BeApproximately(20, 1e-4);
    }
  }
}
