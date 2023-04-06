namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class CircleDiameter_Tests
{
  [Test]
  public void Diameter_works()
  {
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var circle = new Circle(centre, radius);
    var constr = new CircleDiameter(circle);
    constr.SetValue(10);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(radius);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      circle.Radius.Value.Should().BeApproximately(5, 1e-4);
    }
  }
}
