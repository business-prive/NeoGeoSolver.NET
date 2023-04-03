using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Equal : Value
{
  public enum LengthType
  {
    Length,
    Radius,
    Diameter
  }

  private LengthType[] _lengthType = new LengthType[2];

  public Equal(IEntity l0, IEntity l1)
  {
    AddEntity(l0);
    AddEntity(l1);
    value.value = 1.0;
  }

  public override IEnumerable<Expression> equations
  {
    get
    {
      var len = new Expression[2];

      for (var i = 0; i < 2; i++)
      {
        var e = GetEntity(i);
        switch (_lengthType[i])
        {
          case LengthType.Length:
            len[i] = e.Length();
            break;
          case LengthType.Radius:
            len[i] = e.Radius();
            break;
          case LengthType.Diameter:
            len[i] = e.Radius() * 2.0;
            break;
        }
      }

      yield return len[0] - len[1] * value;
    }
  }
}
