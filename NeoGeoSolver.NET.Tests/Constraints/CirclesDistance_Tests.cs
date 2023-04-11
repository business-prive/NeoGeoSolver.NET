namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class CirclesDistance_Tests
{
  [Ignore("not working, maybe not needed")]
  [Test]
  public void Distance_works()
  {
    var centre0 = new Point(0, 0, 0);
    var radius0 = new Param("radius0", 10);
    var circle0 = new Circle(centre0, radius0);
    var centre1 = new Point(1, 0, 0);
    var radius1 = new Param("radius1", 10);
    var circle1 = new Circle(centre1, radius1);
    var constr = new CirclesDistance(circle0, circle1);
    constr.SetValue(10);
    constr.option = CirclesDistance.Option.FirstInside;
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(centre1.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      centre1.x.Value.Should().BeApproximately(0, 1e-4);
    }
  }
}
