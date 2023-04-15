namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointOnCircleQuadConstraint_Tests
{
  [TestCase(0)] // east
  [TestCase(1)] // north
  [TestCase(2)] // west
  [TestCase(3)] // south
  public void PointCircleQuad_works(int quadIndex)
  {
    var centre = new Point(0, 0, 0);
    var radius = new Param("radius", 10);
    var circle = new Circle(centre, radius);
    var pt = new Point(5, 0, 0);
    var constr = new PointOnCircleQuadConstraint(pt, circle, quadIndex);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(pt.X);
    eqnSys.AddParameter(pt.Y);
    var expX = quadIndex switch
    {
      0 => 10,
      1 => 0,
      2 => -10,
      3 => 0
    };
    var expY = quadIndex switch
    {
      0 => 0,
      1 => 10,
      2 => 0,
      3 => -10,
    };

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      pt.X.Value.Should().BeApproximately(expX, 1e-4);
      pt.Y.Value.Should().BeApproximately(expY, 1e-4);
    }
  }
}
