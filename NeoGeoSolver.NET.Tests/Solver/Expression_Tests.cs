using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Tests.Solver;

[TestFixture]
public sealed class Expression_Tests
{
  [TestCase("2 * 3", 6.0)]
  [TestCase("2 + 1", 3.0)]
  [TestCase("-2 + 2", 0.0)]
  [TestCase("+2 - -2", 4.0)]
  [TestCase("-2 * -2", 4.0)]
  [TestCase("+1 * +2", 2.0)]
  [TestCase("2 + 3 * 6", 20.0)]
  [TestCase("2 + (3 * 6)", 20.0)]
  [TestCase("(2 + 3) * 6", 30.0)]
  [TestCase("((2 + 3) * (6))", 30.0)]
  [TestCase("cos(0)", 1.0)]
  [TestCase("sqr(cos(2)) + sqr(sin(2))", 1.0)]
  [TestCase("pi", Math.PI)]
  [TestCase("e", Math.E)]
  public static void Eval_returns_expected(string exprStr, double expResult)
  {
    var parser = new ExpressionParser(exprStr);
    var exp = parser.Parse();

    var result = exp.Eval();

    result.Should().Be(expResult);
  }
}
