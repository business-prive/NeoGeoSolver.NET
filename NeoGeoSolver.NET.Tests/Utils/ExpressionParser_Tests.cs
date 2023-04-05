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
}
