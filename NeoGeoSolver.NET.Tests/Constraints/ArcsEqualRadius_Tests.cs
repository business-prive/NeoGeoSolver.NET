namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class ArcsEqualRadius_Tests
{
  [Test]
  public void EqualRadius_works()
  {
    var centre0 = new Point(0, 0, 0);
    var radius0 = new Param("radius", 10);
    var startAngle0 = new Param("startAngle", 0);
    var endAngle0 = new Param("endAngle", Math.PI / 2d);
    var arc0 = new Arc(centre0, radius0, startAngle0, endAngle0);
    var centre1 = new Point(0, 0, 0);
    var radius1 = new Param("radius", 20);
    var startAngle1 = new Param("startAngle", 0);
    var endAngle1 = new Param("endAngle", Math.PI / 2d);
    var arc1 = new Arc(centre1, radius1, startAngle1, endAngle1);
    var constr = new ArcsEqualRadius(arc0, arc1);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(arc0.Radius);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      arc0.Radius.Value.Should().BeApproximately(20, 1e-6);
    }
  }
}
