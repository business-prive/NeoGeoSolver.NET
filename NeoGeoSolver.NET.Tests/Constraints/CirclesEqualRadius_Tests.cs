namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class CirclesEqualRadius_Tests
{
  [Test]
  public void EqualRadius_works()
  {
    var centre0 = new Point(0, 0, 0);
    var radius0 = new Param("radius", 10);
    var circle0 = new Circle(centre0, radius0);
    var centre1 = new Point(0, 0, 0);
    var radius1 = new Param("radius", 50);
    var circle1 = new Circle(centre1, radius1);
    var constr = new CirclesEqualRadius(circle0, circle1);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(radius0);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      circle0.Radius.Value.Should().BeApproximately(50, 1e-6);
    }
  }
}
