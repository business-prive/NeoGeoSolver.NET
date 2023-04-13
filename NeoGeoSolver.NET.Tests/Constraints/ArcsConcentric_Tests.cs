namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class ArcsConcentric_Tests
{
  [Test]
  public void Concentric_works()
  {
    var centre0 = new Point(0, 0, 0);
    var radius0 = new Param("radius0", 10);
    var startAngle0 = new Param("startAngle0", 0);
    var endAngle0 = new Param("endAngle0", Math.PI);
    var arc0 = new Arc(centre0, radius0, startAngle0, endAngle0);
    var centre1 = new Point(10, 0, 0);
    var radius1 = new Param("radius1", 10);
    var startAngle1 = new Param("startAngle1", 0);
    var endAngle1 = new Param("endAngle1", 0);
    var arc1 = new Arc(centre1, radius1, startAngle1, endAngle1);
    var constr = new ArcsConcentric(arc0, arc1);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(centre1.X);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      centre1.X.Value.Should().BeApproximately(0, 1e-4);
    }
  }
}
