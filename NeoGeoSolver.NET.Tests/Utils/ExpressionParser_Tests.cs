using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Tests.Utils;

[TestFixture]
public sealed class ExpressionParser_Tests
{
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
}
