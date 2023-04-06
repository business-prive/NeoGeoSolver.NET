namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class PointsHorizontalVertical_Tests
{
  [Test]
  public void Vertical_works()
  {
    var pt0 = new Point(0, 0, 0);
    var pt1 = new Point(10, 10, 0);
    var constr = new PointsHorizontalVertical(pt0, pt1, HorizontalVerticalOrientation.Ox);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(pt1.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      pt1.x.Value.Should().Be(0);
    }
  }

  [Test]
  public void Horizontal_works()
  {
    var pt0 = new Point(0, 0, 0);
    var pt1 = new Point(10, 10, 0);
    var constr = new PointsHorizontalVertical(pt0, pt1, HorizontalVerticalOrientation.Oy);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(pt1.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      pt1.y.Value.Should().Be(0);
    }
  }
}
