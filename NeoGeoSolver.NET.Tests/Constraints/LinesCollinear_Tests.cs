namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class LinesCollinear_Tests
{
  [Test]
  public void Collinear_line1_pt1_works()
  {
    var l0pt0 = new Point(0, 0, 0);
    var l0pt1 = new Point(10, 10, 0);
    var line0 = new Line(l0pt0, l0pt1);
    var l1pt0 = new Point(10, 10, 0);
    var l1pt1 = new Point(20, 10, 0);
    var line1 = new Line(l1pt0, l1pt1);
    var constr = new LinesCollinear(line0, line1);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(l1pt1.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      l1pt1.y.Value.Should().BeApproximately(20, 1e-4);
    }
  }

  [Test]
  public void Collinear_line0_pt1_works()
  {
    var l0pt0 = new Point(0, 0, 0);
    var l0pt1 = new Point(10, 5, 0);
    var line0 = new Line(l0pt0, l0pt1);
    var l1pt0 = new Point(10, 10, 0);
    var l1pt1 = new Point(20, 20, 0);
    var line1 = new Line(l1pt0, l1pt1);
    var constr = new LinesCollinear(line0, line1);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(l0pt1.y);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      l0pt1.y.Value.Should().BeApproximately(10, 1e-4);
    }
  }
}
