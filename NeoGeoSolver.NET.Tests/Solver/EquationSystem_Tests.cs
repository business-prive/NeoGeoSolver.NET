namespace NeoGeoSolver.NET.Tests.Solver;

[TestFixture]
public sealed class EquationSystem_Tests
{
  [Test]
  public void Square_around_circle()
  {
    const double Tolerance = 1e-6;
    
    // circle on origin with radius 10
    var circle = new Circle(new Point(0, 0, 0), new Param("radius", 10));

    // We want a box around the circle where all lines touch the 
    // circle and line0 is vertical. We arrange the lines roughly
    // in the correct placement to get the search off to a good
    // start
    var line0 = new Line(new Point(-11, 0, 0), new Point(0, 13, 0));
    var line1 = new Line(new Point(0, 12, 0), new Point(12, 0, 0));
    var line2 = new Line(new Point(11, 0, 0), new Point(0, -12, 0));
    var line3 = new Line(new Point(0, -12, 0), new Point(-12, 0, 0));

    #region Constraints

    var line0_tan_circle = line0.IsTangentTo(circle);
    var line1_tan_circle = line1.IsTangentTo(circle);
    var line2_tan_circle = line2.IsTangentTo(circle);
    var line3_tan_circle = line3.IsTangentTo(circle);

    var line0_perp_line1 = line0.IsPerpendicularTo(line1);
    var line1_perp_line2 = line1.IsPerpendicularTo(line2);
    var line2_perp_line3 = line2.IsPerpendicularTo(line3);
    var line3_perp_line0 = line3.IsPerpendicularTo(line0);

    var l0p1_coinc_l1p0 = line0.Point1.IsCoincidentWithConstraint(line1.Point0);
    var l1p1_coinc_l2p0 = line1.Point1.IsCoincidentWithConstraint(line2.Point0);
    var l2p1_coinc_l3p0 = line2.Point1.IsCoincidentWithConstraint(line3.Point0);
    var l3p1_coinc_l0p0 = line3.Point1.IsCoincidentWithConstraint(line0.Point0);

    var line0_hor = line0.IsHorizontal();

    var constrs = new Constraint[]
    {
      line0_tan_circle,
      line1_tan_circle,
      line2_tan_circle,
      line3_tan_circle,

      line0_perp_line1,
      line1_perp_line2,
      line2_perp_line3,
      line3_perp_line0,

      l0p1_coinc_l1p0,
      l1p1_coinc_l2p0,
      l2p1_coinc_l3p0,
      l3p1_coinc_l0p0,

      line0_hor
    };

    #endregion
    
    var eqns = constrs.SelectMany(constr => constr.Equations);
    
    // all points free
    var pointParams = new[]
    {
      line0.Point0.x, line0.Point0.y, line0.Point1.x, line0.Point1.y,
      line1.Point0.x, line1.Point0.y, line1.Point1.x, line1.Point1.y,
      line2.Point0.x, line2.Point0.y, line2.Point1.x, line2.Point1.y,
      line3.Point0.x, line3.Point0.y, line3.Point1.x, line3.Point1.y
    };
    
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(eqns);
    eqnSys.AddParameters(pointParams);

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);

      line0.Point0.x.Value.Should().BeApproximately(-10, Tolerance);
      line0.Point0.y.Value.Should().BeApproximately(10, Tolerance);
      line0.Point1.x.Value.Should().BeApproximately(10, Tolerance);
      line0.Point1.y.Value.Should().BeApproximately(10, Tolerance);

      line1.Point0.x.Value.Should().BeApproximately(10, Tolerance);
      line1.Point0.y.Value.Should().BeApproximately(10, Tolerance);
      line1.Point1.x.Value.Should().BeApproximately(10, Tolerance);
      line1.Point1.y.Value.Should().BeApproximately(-10, Tolerance);

      line2.Point0.x.Value.Should().BeApproximately(10, Tolerance);
      line2.Point0.y.Value.Should().BeApproximately(-10, Tolerance);
      line2.Point1.x.Value.Should().BeApproximately(-10, Tolerance);
      line2.Point1.y.Value.Should().BeApproximately(-10, Tolerance);
    }
  }
}
