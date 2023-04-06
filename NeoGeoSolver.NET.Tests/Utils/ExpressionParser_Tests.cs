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

  public static void Test()
  {
    List<string> exps = new()
    {
      "a + b",
      "  a  - -b",
      "43 + d * c",
      "2.3 * d + c ",
      "(a * b) + c ",
      "a * (b + c)",
      " a * b + c * (d + e) * f - 1 ",
      " a * (b + c) * (d + e) * (f - 1) ",
      " (a * ((b + c) + (d + e)) * 3 + (f - 1) * 5)) ",
    };

    foreach (var e in exps)
    {
      var parser = new ExpressionParser(e);
      var exp = parser.Parse();
      // TODO		Debug.Log("src: \"" + e + "\" -> \"" + exp.ToString() + "\"");
    }

    Dictionary<string, double> results = new()
    {
      {"2 * 3", 6.0},
      {"2 + 1", 3.0},
      {"-2 + 2", 0.0},
      {"+2 - -2", 4.0},
      {"-2 * -2", 4.0},
      {"+1 * +2", 2.0},
      {"2 + 3 * 6", 20.0},
      {"2 + (3 * 6)", 20.0},
      {"(2 + 3) * 6", 30.0},
      {"((2 + 3) * (6))", 30.0},
      {"cos(0)", 1.0},
      {"sqr(cos(2)) + sqr(sin(2))", 1.0},
      {"pi", Math.PI},
      {"e", Math.E},
    };

    foreach (var e in results)
    {
      var parser = new ExpressionParser(e.Key);
      var exp = parser.Parse();
      //TODO		Debug.Log("src: \"" + e + "\" -> \"" + exp.ToString() + "\" = " + exp.Eval().ToStr());
      if (exp.Eval() != e.Value)
      {
        {
          // TODO		Debug.Log("result fail: get \"" + exp.Eval() + "\" excepted: \"" + e.Value + "\"");
        }
      }
    }
  }
}
