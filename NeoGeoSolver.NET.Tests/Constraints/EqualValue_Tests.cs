namespace NeoGeoSolver.NET.Tests.Constraints;

[TestFixture]
public sealed class EqualValue_Tests
{
  [Test]
  public void Equal_works()
  {
    var val0 = new DummyValue(10);
    var val1 = new DummyValue(0);
    var constr = new EqualValue(val0, val1);
    constr.SetValue(5);
    var eqnSys = new EquationSystem();
    eqnSys.AddEquations(constr.Equations);
    eqnSys.AddParameter(val1.GetValueParam());

    var result = eqnSys.Solve();

    using (new AssertionScope())
    {
      result.Should().Be(EquationSystem.SolveResult.Okay);
      val1.GetValue().Should().BeApproximately(5, 1e-6);
    }
  }
  
  private sealed class DummyValue : Value
  {
    public DummyValue(double val)
    {
      SetValue(val);
    }

    public override IEnumerable<Entity> Entities
    {
      get
      {
        yield break;
      }
    }
  }
}
