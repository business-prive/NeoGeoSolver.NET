using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Utils;

public class ExpressionParser
{
  private Dictionary<string, Expression.Op> _functions = new()
  {
    {"sin", Expression.Op.Sin},
    {"cos", Expression.Op.Cos},
    {"atan2", Expression.Op.Atan2},
    {"sqr", Expression.Op.Sqr},
    {"sqrt", Expression.Op.Sqrt},
    {"abs", Expression.Op.Abs},
    {"sign", Expression.Op.Sign},
    {"acos", Expression.Op.ACos},
    {"asin", Expression.Op.Cos},
    {"exp", Expression.Op.Exp},
    {"sinh", Expression.Op.Sinh},
    {"cosh", Expression.Op.Cosh},
    {"sfres", Expression.Op.SFres},
    {"cfres", Expression.Op.CFres},
  };

  private Dictionary<char, Expression.Op> _operators = new()
  {
    {'+', Expression.Op.Add},
    {'-', Expression.Op.Sub},
    {'*', Expression.Op.Mul},
    {'/', Expression.Op.Div},
  };

  private Dictionary<string, double> _constants = new()
  {
    {"pi", Math.PI},
    {"e", Math.E},
  };

  private string _toParse;
  private int _index = 0;

  public readonly List<Param> Parameters = new();

  public ExpressionParser(string str)
  {
    _toParse = str;
  }

  public void SetString(string str)
  {
    _toParse = str;
    _index = 0;
  }

  private char Next
  {
    get
    {
      return _toParse[_index];
    }
  }

  private bool IsSpace(char c)
  {
    return char.IsWhiteSpace(c);
  }

  private bool IsDigit(char c)
  {
    return char.IsDigit(c);
  }

  private bool IsDelimiter(char c)
  {
    return c == '.';
  }

  private bool IsAlpha(char c)
  {
    return char.IsLetter(c);
  }

  private void SkipSpaces()
  {
    if (!HasNext())
    {
      return;
    }

    while (HasNext() && IsSpace(Next))
    {
      _index++;
    }
  }

  private Param GetParam(string name)
  {
    return Parameters.Find(p => p.Name == name);
  }

  private void Skip(char c)
  {
    SkipSpaces();
    if (!HasNext() || Next != c)
    {
      Error("\"" + c + "\" excepted!");
    }

    _index++;
  }

  private bool SkipIf(char c)
  {
    SkipSpaces();
    if (!HasNext() || Next != c)
    {
      return false;
    }

    _index++;
    return true;
  }

  private bool ParseDigits(ref double digits)
  {
    SkipSpaces();
    if (!HasNext())
    {
      Error("operand exepted");
    }

    if (!IsDigit(Next))
    {
      return false;
    }

    var start = _index;
    while (HasNext() && (IsDigit(Next) || IsDelimiter(Next)))
    {
      _index++;
    }

    var str = _toParse.Substring(start, _index - start);
    digits = double.Parse(str);
    return true;
  }

  private bool ParseAlphas(ref string alphas)
  {
    SkipSpaces();
    if (!HasNext())
    {
      Error("operand exepted");
    }

    if (!IsAlpha(Next))
    {
      return false;
    }

    var start = _index;
    while (HasNext() && (IsAlpha(Next) || IsDigit(Next)))
    {
      _index++;
    }

    alphas = _toParse.Substring(start, _index - start);
    return true;
  }

  private Expression.Op GetFunction(string name)
  {
    if (_functions.ContainsKey(name))
    {
      return _functions[name];
    }

    return Expression.Op.Undefined;
  }

  private Expression GetConstant(string name)
  {
    if (_constants.ContainsKey(name))
    {
      return _constants[name];
    }

    return null;
  }

  private void Error(string error = "")
  {
    var str = _toParse;
    if (_index < str.Length)
    {
      str.Insert(_index, "?");
    }

    var msg = error + " (error in \"" + str + "\")";
    throw new Exception(msg);
  }

  private Expression ParseValue()
  {
    var digits = 0.0;
    if (ParseDigits(ref digits))
    {
      return new Expression(digits);
    }

    var braced = false;

    var alphas = "";
    if (ParseAlphas(ref alphas))
    {
      var func = GetFunction(alphas);
      if (func != Expression.Op.Undefined)
      {
        if (SkipIf('('))
        {
          var a = ParseExp(ref braced);
          Expression b = null;
          if (SkipIf(','))
          {
            b = ParseExp(ref braced);
          }

          Skip(')');
          if (func == Expression.Op.Atan2 && b == null)
          {
            Error("second function argument execpted");
          }

          return new Expression(func, a, b);
        }

        Error("function arguments execpted");
      }

      var constant = GetConstant(alphas);
      if (constant != null)
      {
        return constant;
      }

      var param = GetParam(alphas);
      if (param == null)
      {
        param = new Param(alphas);
        Parameters.Add(param);
      }

      return new Expression(param);
    }

    Error("valid operand excepted");
    return null;
  }

  private int OrderOf(Expression.Op op)
  {
    switch (op)
    {
      case Expression.Op.Add:
      case Expression.Op.Sub:
        return 1;
      case Expression.Op.Mul:
      case Expression.Op.Div:
        return 2;
      default:
        return 0;
    }
  }

  private Expression.Op ParseOp()
  {
    SkipSpaces();
    if (_operators.ContainsKey(Next))
    {
      var result = _operators[Next];
      _index++;
      return result;
    }

    return Expression.Op.Undefined;
  }

  private bool HasNext()
  {
    return _index < _toParse.Length;
  }

  private Expression.Op ParseUnary()
  {
    SkipSpaces();
    if (Next == '+')
    {
      _index++;
      return Expression.Op.Pos;
    }

    if (Next == '-')
    {
      _index++;
      return Expression.Op.Neg;
    }

    return Expression.Op.Undefined;
  }

  private Expression ParseExp(ref bool braced)
  {
    var uop = ParseUnary();

    Expression a = null;
    var aBraced = false;
    if (SkipIf('('))
    {
      var br = false;
      a = ParseExp(ref br);
      Skip(')');
      aBraced = true;
    }
    else
    {
      a = ParseValue();
    }

    if (uop != Expression.Op.Undefined && uop != Expression.Op.Pos)
    {
      a = new Expression(uop, a, null);
    }

    SkipSpaces();
    if (!HasNext() || Next == ')' || Next == ',')
    {
      braced = aBraced;
      return a;
    }

    var op = ParseOp();
    if (op == Expression.Op.Undefined)
    {
      Error("operator execpted");
    }

    var bBraced = false;
    var b = ParseExp(ref bBraced);

    if (!bBraced && b.HasTwoOperands() && OrderOf(op) > OrderOf(b.op))
    {
      b.a = new Expression(op, a, b.a);
      return b;
    }

    return new Expression(op, a, b);
  }

  public Expression Parse()
  {
    try
    {
      var braced = false;
      return ParseExp(ref braced);
    }
    catch (Exception)
    {
      return null;
    }
  }
}
