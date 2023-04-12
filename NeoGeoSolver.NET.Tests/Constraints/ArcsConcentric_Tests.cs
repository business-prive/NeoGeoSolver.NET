namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class ArcsConcentric_Tests
{
  [Test]
  public void Concentric_works()
  {
    var centre0 = new Point(0, 0, 0);
    var c0p0 = new Point(10, 0, 0);
    var c0p1 = new Point(0, 10, 0);
    var arc0 = new Arc(centre0, c0p0, c0p1);
    var centre1 = new Point(10, 0, 0);
    var c1p0 = new Point(10, 0, 0);
    var c1p1 = new Point(20, 10, 0);
    var arc1 = new Arc(centre1, c1p0, c1p1);
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
