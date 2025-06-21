namespace NeoGeoSolver.NET.Solver;

public class Expression
{
  public enum Op
  {
    Undefined,
    Const,
    Param,
    Add,
    Sub,
    Mul,
    Div,
    Sin,
    Cos,
    ACos,
    ASin,
    Sqrt,
    Sqr,
    Atan2,
    Abs,
    Sign,
    Neg,
    Pos,
    Exp,
    Sinh,
    Cosh,
    SFres,
    CFres
  }

  public static readonly Expression Zero = new(0.0);
  public static readonly Expression One = new(1.0);
  public static readonly Expression MOne = new(-1.0);
  public static readonly Expression Two = new(2.0);

  public Op op;

  public Expression a;
  public Expression b;
  public Param param;
  public double Value;

  private Expression()
  {
  }

  public Expression(double value)
  {
    Value = value;
    op = Op.Const;
  }

  internal Expression(Param p)
  {
    param = p;
    op = Op.Param;
  }

  public static implicit operator Expression(Param param)
  {
    return param.Expr;
  }

  public static implicit operator Expression(double value)
  {
    if (value == 0.0)
    {
      return Zero;
    }

    if (value == 1.0)
    {
      return One;
    }

    var result = new Expression
    {
      Value = value,
      op = Op.Const
    };
    return result;
  }

  public Expression(Op op, Expression a, Expression b)
  {
    this.a = a;
    this.b = b;
    this.op = op;
  }

  public static Expression operator +(Expression a, Expression b)
  {
    if (a.IsZeroConst())
    {
      return b;
    }

    if (b.IsZeroConst())
    {
      return a;
    }

    if (b.op == Op.Neg)
    {
      return a - b.a;
    }

    if (b.op == Op.Pos)
    {
      return a + b.a;
    }

    return new Expression(Op.Add, a, b);
  }

  public static Expression operator -(Expression a, Expression b)
  {
    if (a.IsZeroConst())
    {
      return -b;
    }

    if (b.IsZeroConst())
    {
      return a;
    }

    return new Expression(Op.Sub, a, b);
  }

  public static Expression operator *(Expression a, Expression b)
  {
    if (a.IsZeroConst())
    {
      return Zero;
    }

    if (b.IsZeroConst())
    {
      return Zero;
    }

    if (a.IsOneConst())
    {
      return b;
    }

    if (b.IsOneConst())
    {
      return a;
    }

    if (a.IsMinusOneConst())
    {
      return -b;
    }

    if (b.IsMinusOneConst())
    {
      return -a;
    }

    if (a.IsConst() && b.IsConst())
    {
      return a.Value * b.Value;
    }

    return new Expression(Op.Mul, a, b);
  }

  public static Expression operator /(Expression a, Expression b)
  {
    if (b.IsOneConst())
    {
      return a;
    }

    if (a.IsZeroConst())
    {
      return Zero;
    }

    if (b.IsMinusOneConst())
    {
      return -a;
    }

    return new Expression(Op.Div, a, b);
  }

  public static Expression operator -(Expression a)
  {
    if (a.IsZeroConst())
    {
      return a;
    }

    if (a.IsConst())
    {
      return -a.Value;
    }

    if (a.op == Op.Neg)
    {
      return a.a;
    }

    return new Expression(Op.Neg, a, null);
  }

  // https://www.hindawi.com/journals/mpe/2018/4031793/
  public static double CFres(double x)
  {
    var pi = Math.PI;
    var ax = Math.Abs(x);
    var ax2 = ax * ax;
    var ax3 = ax2 * ax;
    var x3 = x * x * x;
    /*
		return (
			-Math.Sin(PI * ax2 / 2.0) / 
			(PI * (x + 20.0 * PI * Math.Exp(-200.0 * PI * Math.Sqrt(ax))))

			+ 8.0 / 25.0 * (1.0 - Math.Exp(-69.0 / 100.0     * PI * x3))
			+ 2.0 / 25.0 * (1.0 - Math.Exp(-9.0 / 2.0        * PI * ax2))
			+ 1.0 / 10.0 * (1.0 - Math.Exp(-1.55294068198794 * PI * x ))
		) * Math.Sign(x);
		
		*/
    return Math.Sign(x) * (
      1.0 / 2.0 + ((1 + 0.926 * ax) / (2 + 1.792 * ax + 3.104 * ax2)) * Math.Sin(Math.PI * ax2 / 2)
      - (1 / (2 + 4.142 * ax + 3.492 * ax2 + 6.67 * ax3)) * Math.Cos(Math.PI * ax2 / 2)
    );
  }

  public static double SFres(double x)
  {
    var pi = Math.PI;
    var ax = Math.Abs(x);
    var ax2 = ax * ax;
    var ax3 = ax2 * ax;
    return Math.Sign(x) * (
      1.0 / 2.0 - ((1 + 0.926 * ax) / (2 + 1.792 * ax + 3.104 * ax2)) * Math.Cos(Math.PI * ax2 / 2)
                - (1 / (2 + 4.142 + 3.492 * ax2 + 6.67 * ax3)) * Math.Sin(Math.PI * ax2 / 2)
    );
  }

  public static Expression Sin(Expression x)
  {
    return new Expression(Op.Sin, x, null);
  }

  public static Expression Cos(Expression x)
  {
    return new Expression(Op.Cos, x, null);
  }

  public static Expression ACos(Expression x)
  {
    return new Expression(Op.ACos, x, null);
  }

  public static Expression ASin(Expression x)
  {
    return new Expression(Op.ASin, x, null);
  }

  public static Expression Sqrt(Expression x)
  {
    return new Expression(Op.Sqrt, x, null);
  }

  public static Expression Sqr(Expression x)
  {
    return new Expression(Op.Sqr, x, null);
  }

  public static Expression Abs(Expression x)
  {
    return new Expression(Op.Abs, x, null);
  }

  public static Expression Sign(Expression x)
  {
    return new Expression(Op.Sign, x, null);
  }

  public static Expression Atan2(Expression x, Expression y)
  {
    return new Expression(Op.Atan2, x, y);
  }

  public static Expression Expo(Expression x)
  {
    return new Expression(Op.Exp, x, null);
  }

  public static Expression Sinh(Expression x)
  {
    return new Expression(Op.Sinh, x, null);
  }

  public static Expression Cosh(Expression x)
  {
    return new Expression(Op.Cosh, x, null);
  }

  public static Expression SFres(Expression x)
  {
    return new Expression(Op.SFres, x, null);
  }

  public static Expression CFres(Expression x)
  {
    return new Expression(Op.CFres, x, null);
  }

  public double Eval()
  {
    switch (op)
    {
      case Op.Const:
        return Value;
      case Op.Param:
        return param.Value;
      case Op.Add:
        return a.Eval() + b.Eval();
      case Op.Sub:
        return a.Eval() - b.Eval();
      case Op.Mul:
        return a.Eval() * b.Eval();
      case Op.Div:
      {
        var bv = b.Eval();
        if (Math.Abs(bv) < 1e-10)
        {
          //Debug.Log("Division by zero");
          bv = 1.0;
        }

        return a.Eval() / bv;
      }
      case Op.Sin:
        return Math.Sin(a.Eval());
      case Op.Cos:
        return Math.Cos(a.Eval());
      case Op.ACos:
        return Math.Acos(a.Eval());
      case Op.ASin:
        return Math.Asin(a.Eval());
      case Op.Sqrt:
        return Math.Sqrt(a.Eval());
      case Op.Sqr:
      {
        var av = a.Eval();
        return av * av;
      }
      case Op.Atan2:
        return Math.Atan2(a.Eval(), b.Eval());
      case Op.Abs:
        return Math.Abs(a.Eval());
      case Op.Sign:
        return Math.Sign(a.Eval());
      case Op.Neg:
        return -a.Eval();
      case Op.Pos:
        return a.Eval();
      case Op.Exp:
        return Math.Exp(a.Eval());
      case Op.Sinh:
        return Math.Sinh(a.Eval());
      case Op.Cosh:
        return Math.Cosh(a.Eval());
      case Op.SFres:
        return SFres(a.Eval());
      case Op.CFres:
        return CFres(a.Eval());

      case Op.Undefined:
      default:
        throw new ArgumentOutOfRangeException();
    }

    return 0.0;
  }

  public bool IsZeroConst()
  {
    return op == Op.Const && Value == 0.0;
  }

  public bool IsOneConst()
  {
    return op == Op.Const && Value == 1.0;
  }

  public bool IsMinusOneConst()
  {
    return op == Op.Const && Value == -1.0;
  }

  public bool IsConst()
  {
    return op == Op.Const;
  }

  public bool IsUnary()
  {
    switch (op)
    {
      case Op.Const:
      case Op.Param:
      case Op.Sin:
      case Op.Cos:
      case Op.ACos:
      case Op.ASin:
      case Op.Sqrt:
      case Op.Sqr:
      case Op.Abs:
      case Op.Sign:
      case Op.Neg:
      case Op.Pos:
      case Op.Exp:
      case Op.Cosh:
      case Op.Sinh:
      case Op.CFres:
      case Op.SFres:
        return true;
    }

    return false;
  }

  public bool IsAdditive()
  {
    switch (op)
    {
      case Op.Sub:
      case Op.Add:
        return true;
    }

    return false;
  }

  private string Quoted()
  {
    if (IsUnary())
    {
      return ToString();
    }

    return "(" + ToString() + ")";
  }

  private string QuotedAdd()
  {
    if (!IsAdditive()) return ToString();
    return "(" + ToString() + ")";
  }

  public override string ToString()
  {
    switch (op)
    {
      case Op.Const:
        return Value.ToString();
      case Op.Param:
        return param.Name;
      case Op.Add:
        return a + " + " + b;
      case Op.Sub:
        return a + " - " + b.QuotedAdd();
      case Op.Mul:
        return a.QuotedAdd() + " * " + b.QuotedAdd();
      case Op.Div:
        return a.QuotedAdd() + " / " + b.Quoted();
      case Op.Sin:
        return "sin(" + a + ")";
      case Op.Cos:
        return "cos(" + a + ")";
      case Op.ASin:
        return "asin(" + a + ")";
      case Op.ACos:
        return "acos(" + a + ")";
      case Op.Sqrt:
        return "sqrt(" + a + ")";
      case Op.Sqr:
        return a.Quoted() + " ^ 2";
      case Op.Abs:
        return "abs(" + a + ")";
      case Op.Sign:
        return "sign(" + a + ")";
      case Op.Atan2:
        return "atan2(" + a + ", " + b + ")";
      case Op.Neg:
        return "-" + a.Quoted();
      case Op.Pos:
        return "+" + a.Quoted();
      case Op.Exp:
        return "exp(" + a + ")";
      case Op.Sinh:
        return "sinh(" + a + ")";
      case Op.Cosh:
        return "cosh(" + a + ")";
      case Op.SFres:
        return "sfres(" + a + ")";
      case Op.CFres:
        return "cfres(" + a + ")";
    }

    return "";
  }

  public Expression Deriv(Param p)
  {
    return D(p);
  }

  private Expression D(Param p)
  {
    switch (op)
    {
      case Op.Const:
        return Zero;
      case Op.Param:
        return (param == p) ? One : Zero;
      case Op.Add:
        return a.D(p) + b.D(p);
      case Op.Sub:
        return a.D(p) - b.D(p);
      case Op.Mul:
        return a.D(p) * b + a * b.D(p);
      case Op.Div:
        return (a.D(p) * b - a * b.D(p)) / Sqr(b);
      case Op.Sin:
        return a.D(p) * Cos(a);
      case Op.Cos:
        return a.D(p) * -Sin(a);
      case Op.ASin:
        return a.D(p) / Sqrt(One - Sqr(a));
      case Op.ACos:
        return a.D(p) * MOne / Sqrt(One - Sqr(a));
      case Op.Sqrt:
        return a.D(p) / (Two * Sqrt(a));
      case Op.Sqr:
        return a.D(p) * Two * a;
      case Op.Abs:
        return a.D(p) * Sign(a);
      case Op.Sign:
        return Zero;
      case Op.Neg:
        return -a.D(p);
      case Op.Atan2:
        return (b * a.D(p) - a * b.D(p)) / (Sqr(a) + Sqr(b));
      case Op.Exp:
        return a.D(p) * Expo(a);
      case Op.Sinh:
        return a.D(p) * Cosh(a);
      case Op.Cosh:
        return a.D(p) * Sinh(a);
      case Op.SFres:
        return a.D(p) * Sin(Math.PI * Sqr(a) / 2.0);
      case Op.CFres:
        return a.D(p) * Cos(Math.PI * Sqr(a) / 2.0);
    }

    return Zero;
  }

  public bool IsSubstitionForm()
  {
    return op == Op.Sub && a.op == Op.Param && b.op == Op.Param;
  }

  public Param GetSubstitutionParamA()
  {
    if (!IsSubstitionForm())
    {
      return null;
    }

    return a.param;
  }

  public Param GetSubstitutionParamB()
  {
    if (!IsSubstitionForm())
    {
      return null;
    }

    return b.param;
  }

  public void Substitute(Param pa, Param pb)
  {
    if (a != null)
    {
      a.Substitute(pa, pb);
      if (b != null)
      {
        b.Substitute(pa, pb);
      }
    }
    else if (op == Op.Param && param == pa)
    {
      param = pb;
    }
  }

  public void Substitute(Param p, Expression e)
  {
    if (a != null)
    {
      a.Substitute(p, e);
      if (b != null)
      {
        b.Substitute(p, e);
      }
    }
    else if (op == Op.Param && param == p)
    {
      op = e.op;
      a = e.a;
      b = e.b;
      param = e.param;
      Value = e.Value;
    }
  }

  public Expression DeepClone()
  {
    var result = new Expression
    {
      op = op,
      param = param,
      Value = Value
    };
    if (a != null)
    {
      result.a = a.DeepClone();
      if (b != null)
      {
        result.b = b.DeepClone();
      }
    }

    return result;
  }

  public bool HasTwoOperands()
  {
    return a != null && b != null;
  }

  public Op GetOp()
  {
    return op;
  }

  public override bool Equals(object obj)
  {
    if (ReferenceEquals(this, obj))
      return true;
  
    if (obj is not Expression other)
      return false;
  
    if (op != other.op)
      return false;
  
    switch (op)
    {
      case Op.Const:
        return Value.Equals(other.Value);
  
      case Op.Param:
        return param == other.param;
  
      case Op.Neg:
      case Op.Pos:
      case Op.Sin:
      case Op.Cos:
      case Op.ASin:
      case Op.ACos:
      case Op.Sqrt:
      case Op.Sqr:
      case Op.Abs:
      case Op.Sign:
      case Op.Exp:
      case Op.Sinh:
      case Op.Cosh:
      case Op.SFres:
      case Op.CFres:
        return Equals(a, other.a);
  
      case Op.Add:
      case Op.Sub:
      case Op.Mul:
      case Op.Div:
      case Op.Atan2:
        return Equals(a, other.a) && Equals(b, other.b);
  
      case Op.Undefined:
      default:
        return true;
    }
  }
  
  public static bool operator ==(Expression left, Expression right)
  {
    if (ReferenceEquals(left, right))
      return true;
  
    if (left is null || right is null)
      return false;
  
    return left.Equals(right);
  }
  
  public static bool operator !=(Expression left, Expression right)
  {
    return !(left == right);
  }
  
}
