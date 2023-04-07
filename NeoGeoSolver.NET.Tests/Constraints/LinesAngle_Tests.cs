namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class LinesAngle_Tests
{
  [Test]
  public void Angle_works()
  {
    var l0pt0 = new Point(0, 0, 0);
    var l0pt1 = new Point(0, 10, 0);
    var line0 = new Line(l0pt0, l0pt1);
    var l1pt0 = new Point(0, 0, 0);
    var l1pt1 = new Point(10, 0, 0);
    var line1 = new Line(l1pt0, l1pt1);
    var constr = new LinesAngle(line0, line1);
    constr.SetValue(Math.PI / 4d);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(line1.Point1.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      line1.Point1.y.Value.Should().BeApproximately(-10, 1e-6);
    }
  }
}
