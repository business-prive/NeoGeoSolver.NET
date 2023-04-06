using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Tests;

[TestFixture]
public sealed class ExpressionParser_Tests
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public void Constructor_completes()
  {
    var sut = new ExpressionParser("");
  }

  [TestCase("a + b")]
  [TestCase("  a  - -b")]
  [TestCase("43 + d * c")]
  [TestCase("2.3 * d + c ")]
  [TestCase("(a * b) + c ")]
  [TestCase("a * (b + c)")]
  [TestCase(" a * b + c * (d + e) * f - 1 ")]
  [TestCase(" a * (b + c) * (d + e) * (f - 1) ")]
  [TestCase(" (a * ((b + c) + (d + e)) * 3 + (f - 1) * 5)) ")]
  public static void Parse_completes(string exprStr)
  {
    var parser = new ExpressionParser(exprStr);

    Action act = () => _ = parser.Parse();

    act.Should().NotThrow();
  }

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
