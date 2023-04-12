namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class LineLength_Tests
{
  [Test]
  public void Length_works()
  {
    var pt0 = new Point(0, 0, 0);
    var pt1 = new Point(10, 10, 0);
    var line = new Line(pt0, pt1);
    var constr = new LineLength(line);
    constr.SetValue(10);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(line.Point1.X);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      line.Point1.X.Value.Should().BeApproximately(0, 1e-4);
    }
  }
}
