namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class LineHorizontalVertical_Tests
{
  [Test]
  public void Vertical_works()
  {
    var pt0 = new Point(0, 0, 0);
    var pt1 = new Point(10, 10, 0);
    var line = new Line(pt0, pt1);
    var constr = new LineHorizontalVertical(line, HorizontalVerticalOrientation.Ox);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(line.Point1.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      line.Point1.x.Value.Should().Be(0);
    }
  }

  [Test]
  public void Horizontal_works()
  {
    var pt0 = new Point(0, 0, 0);
    var pt1 = new Point(10, 10, 0);
    var line = new Line(pt0, pt1);
    var constr = new LineHorizontalVertical(line, HorizontalVerticalOrientation.Oy);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(line.Point1.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      line.Point1.y.Value.Should().Be(0); 
    }
  }
}
