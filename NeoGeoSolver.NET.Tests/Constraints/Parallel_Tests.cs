namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class Parallel_Tests
{
  [Test]
  public void Codirected_works()
  {
    var l0pt0 = new Point(0, 0, 0);
    var l0pt1 = new Point(0, 10, 0);
    var line0 = new Line(l0pt0, l0pt1);
    var l1pt0 = new Point(0, 0, 0);
    var l1pt1 = new Point(10, 10, 0);
    var line1 = new Line(l1pt0, l1pt1);
    var constr = new NeoGeoSolver.NET.Constraints.Parallel(line0, line1)
    {
      option = NeoGeoSolver.NET.Constraints.Parallel.Option.Codirected
    };
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(line1.Point1.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      line1.Point1.x.Value.Should().BeApproximately(0, 1e-6);
    }
  }

  [Test]
  public void Antidirected_works()
  {
    var l0pt0 = new Point(0, 0, 0);
    var l0pt1 = new Point(0, 10, 0);
    var line0 = new Line(l0pt1, l0pt0);  // NOTE:   reversed line direction for anti-directed
    var l1pt0 = new Point(0, 0, 0);
    var l1pt1 = new Point(10, 10, 0);
    var line1 = new Line(l1pt0, l1pt1);
    var constr = new NeoGeoSolver.NET.Constraints.Parallel(line0, line1)
    {
      option = NeoGeoSolver.NET.Constraints.Parallel.Option.Antidirected
    };
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(line1.Point0.x);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      line1.Point0.x.Value.Should().BeApproximately(10, 1e-6); 
    }
  }
}
