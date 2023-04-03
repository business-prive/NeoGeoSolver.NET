namespace NeoGeoSolver.NET.Solver;

public class Expression {
  public enum Op {
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
    Drag,
    Exp,
    Sinh,
    Cosh,
    SFres,
    CFres,

    //Pow,
  }

  public static readonly Expression zero = new(0.0);
  public static readonly Expression one  = new(1.0);
  public static readonly Expression mOne = new(-1.0);
  public static readonly Expression two  = new(2.0);

  public Op op;

  public Expression a;
  public Expression b;
  public Param param;
  public double value;

  private Expression() { }

  public Expression(double value) {
    this.value = value;
    op = Op.Const;
  }

  internal Expression(Param p) {
    param = p;
    op = Op.Param;
  }

  public static implicit operator Expression(Param param) {
    return param.exp;
  }

  public static implicit operator Expression(double value) {
    if(value == 0.0) return zero;
    if(value == 1.0) return one;
    Expression result = new Expression();
    result.value = value;
    result.op = Op.Const;
    return result;
  }

  public Expression(Op op, Expression a, Expression b) {
    this.a = a;
    this.b = b;
    this.op = op;
  }

  static public Expression operator+(Expression a, Expression b) {
    if(a.IsZeroConst()) return b;
    if(b.IsZeroConst()) return a;
    if(b.op == Op.Neg) return a - b.a;
    if(b.op == Op.Pos) return a + b.a;
    return new Expression(Op.Add, a, b);
  }

  static public Expression operator-(Expression a, Expression b) {
    if(a.IsZeroConst()) return -b;
    if(b.IsZeroConst()) return a;
    return new Expression(Op.Sub, a, b);
  }

  static public Expression operator*(Expression a, Expression b) {
    if(a.IsZeroConst()) return zero;
    if(b.IsZeroConst()) return zero;
    if(a.IsOneConst()) return b;
    if(b.IsOneConst()) return a;
    if(a.IsMinusOneConst()) return -b;
    if(b.IsMinusOneConst()) return -a;
    if(a.IsConst() && b.IsConst()) return a.value * b.value;
    return new Expression(Op.Mul, a, b);
  }

  static public Expression operator/(Expression a, Expression b) {
    if(b.IsOneConst()) return a;
    if(a.IsZeroConst()) return zero;
    if(b.IsMinusOneConst()) return -a;
    return new Expression(Op.Div, a, b);
  }
  //static public Exp operator^(Exp a, Exp b) { return new Exp(Op.Pow, a, b); }

  static public Expression operator-(Expression a) {
    if(a.IsZeroConst()) return a;
    if(a.IsConst()) return -a.value;
    if(a.op == Op.Neg) return a.a;
    return new Expression(Op.Neg, a, null);
  }

  // https://www.hindawi.com/journals/mpe/2018/4031793/
  public static double CFres(double x) {
		
    var PI = Math.PI;
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
      -(1 / (2 + 4.142 * ax + 3.492 * ax2 + 6.67 * ax3)) * Math.Cos(Math.PI * ax2 / 2)
    );
  }

  public static double SFres(double x) {
		
    var PI = Math.PI;
    var ax = Math.Abs(x);
    var ax2 = ax * ax;
    var ax3 = ax2 * ax;
    return Math.Sign(x) * (
      1.0 / 2.0 - ((1 + 0.926 * ax) / (2 + 1.792 * ax + 3.104 * ax2)) * Math.Cos(Math.PI * ax2 / 2)
                -(1 / (2 + 4.142 + 3.492 * ax2 + 6.67 * ax3)) * Math.Sin(Math.PI * ax2 / 2)
    );
  }

	
  static public Expression Sin	(Expression x) { return new Expression(Op.Sin,	x, null); }
  static public Expression Cos	(Expression x) { return new Expression(Op.Cos,	x, null); }
  static public Expression ACos	(Expression x) { return new Expression(Op.ACos,	x, null); }
  static public Expression ASin	(Expression x) { return new Expression(Op.ASin,	x, null); }
  static public Expression Sqrt	(Expression x) { return new Expression(Op.Sqrt,	x, null); }
  static public Expression Sqr	(Expression x) { return new Expression(Op.Sqr,	x, null); }
  static public Expression Abs	(Expression x) { return new Expression(Op.Abs,	x, null); }
  static public Expression Sign	(Expression x) { return new Expression(Op.Sign,	x, null); }
  static public Expression Atan2	(Expression x, Expression y) { return new Expression(Op.Atan2, x, y); }
  static public Expression Expo	(Expression x) { return new Expression(Op.Exp,	x, null); }
  static public Expression Sinh	(Expression x) { return new Expression(Op.Sinh,	x, null); }
  static public Expression Cosh	(Expression x) { return new Expression(Op.Cosh,	x, null); }
  static public Expression SFres	(Expression x) { return new Expression(Op.SFres,	x, null); }
  static public Expression CFres	(Expression x) { return new Expression(Op.CFres,	x, null); }
  //static public Exp Pow  (Exp x, Exp y) { return new Exp(Op.Pow,   x, y); }

  public Expression Drag(Expression to) {
    return new Expression(Op.Drag, this, to);
  }

  public double Eval() {
    switch(op) {
      case Op.Const:	return value;
      case Op.Param:	return param.value;
      case Op.Add:	return a.Eval() + b.Eval();
      case Op.Drag:
      case Op.Sub:	return a.Eval() - b.Eval();
      case Op.Mul:	return a.Eval() * b.Eval();
      case Op.Div: {
        var bv = b.Eval();
        if(Math.Abs(bv) < 1e-10) {
          //Debug.Log("Division by zero");
          bv = 1.0;
        }
        return a.Eval() / bv;
      }
      case Op.Sin:	return Math.Sin(a.Eval());
      case Op.Cos:	return Math.Cos(a.Eval());
      case Op.ACos:	return Math.Acos(a.Eval());
      case Op.ASin:	return Math.Asin(a.Eval());
      case Op.Sqrt:	return Math.Sqrt(a.Eval());
      case Op.Sqr:	{  double av = a.Eval(); return av * av; }
      case Op.Atan2:	return Math.Atan2(a.Eval(), b.Eval());
      case Op.Abs:	return Math.Abs(a.Eval());
      case Op.Sign:	return Math.Sign(a.Eval());
      case Op.Neg:	return -a.Eval();
      case Op.Pos:	return a.Eval();
      case Op.Exp:	return Math.Exp(a.Eval());
      case Op.Sinh:	return Math.Sinh(a.Eval());
      case Op.Cosh:	return Math.Cosh(a.Eval());
      case Op.SFres:	return SFres(a.Eval());
      case Op.CFres:	return CFres(a.Eval());
      //case Op.Pow:	return Math.Pow(a.Eval(), b.Eval());
    }
    return 0.0;
  }

  public bool IsZeroConst()		{ return op == Op.Const && value ==  0.0; }
  public bool IsOneConst()		{ return op == Op.Const && value ==  1.0; }
  public bool IsMinusOneConst()	{ return op == Op.Const && value == -1.0; }
  public bool IsConst()			{ return op == Op.Const; }
  public bool IsDrag()			{ return op == Op.Drag; }

  public bool IsUnary() {
    switch(op) {
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

  public bool IsAdditive() {
    switch(op) {
      case Op.Drag:
      case Op.Sub:
      case Op.Add:
        return true;
    }
    return false;
  }

  private string Quoted() {
    if(IsUnary()) return ToString();
    return "(" + ToString() + ")";
  }

  private string QuotedAdd() {
    if(!IsAdditive()) return ToString();
    return "(" + ToString() + ")";
  }

  public override string ToString() {
    switch(op) {
      case Op.Const:	return value.ToString();
      case Op.Param:	return param.name;
      case Op.Add:	return a.ToString() + " + " + b.ToString();
      case Op.Sub:	return a.ToString() + " - " + b.QuotedAdd();
      case Op.Mul:	return a.QuotedAdd() + " * " + b.QuotedAdd();
      case Op.Div:	return a.QuotedAdd() + " / " + b.Quoted();
      case Op.Sin:	return "sin(" + a.ToString() + ")";
      case Op.Cos:	return "cos(" + a.ToString() + ")";
      case Op.ASin:	return "asin(" + a.ToString() + ")";
      case Op.ACos:	return "acos(" + a.ToString() + ")";
      case Op.Sqrt:	return "sqrt(" + a.ToString() + ")";
      case Op.Sqr:	return a.Quoted() + " ^ 2";
      case Op.Abs:	return "abs(" + a.ToString() + ")";
      case Op.Sign:	return "sign(" + a.ToString() + ")";
      case Op.Atan2:	return "atan2(" + a.ToString() + ", " + b.ToString() + ")";
      case Op.Neg:	return "-" + a.Quoted();
      case Op.Pos:	return "+" + a.Quoted();
      case Op.Drag:   return a.ToString() + " ≈ " + b.QuotedAdd();
      case Op.Exp:	return "exp(" + a.ToString() + ")";
      case Op.Sinh:	return "sinh(" + a.ToString() + ")";
      case Op.Cosh:	return "cosh(" + a.ToString() + ")";
      case Op.SFres:	return "sfres(" + a.ToString() + ")";
      case Op.CFres:	return "cfres(" + a.ToString() + ")";
      //case Op.Pow:	return Quoted(a) + " ^ " + Quoted(b);
    }
    return "";
  }

  public bool IsDependOn(Param p) {
    if(op == Op.Param) return param == p;
    if(a != null) {
      if(b != null) {
        return a.IsDependOn(p) || b.IsDependOn(p);
      }
      return a.IsDependOn(p);
    }
    return false;
  }

  public Expression Deriv(Param p) {
    return d(p);
  }

  private Expression d(Param p) {
    switch(op) {
      case Op.Const:	return zero;
      case Op.Param:	return (param == p) ? one : zero;
      case Op.Add:	return a.d(p) + b.d(p);
      case Op.Drag:
      case Op.Sub:	return a.d(p) - b.d(p);
      case Op.Mul:	return a.d(p) * b + a * b.d(p);
      case Op.Div:	return (a.d(p) * b - a * b.d(p)) / Sqr(b);
      case Op.Sin:	return a.d(p) * Cos(a);
      case Op.Cos:	return a.d(p) * -Sin(a);
      case Op.ASin:	return a.d(p) / Sqrt(one - Sqr(a));
      case Op.ACos:	return a.d(p) * mOne / Sqrt(one - Sqr(a));
      case Op.Sqrt:	return a.d(p) / (two * Sqrt(a));
      case Op.Sqr:	return a.d(p) * two * a;
      case Op.Abs:	return a.d(p) * Sign(a);
      case Op.Sign:	return zero;
      case Op.Neg:    return -a.d(p);
      case Op.Atan2:	return (b * a.d(p) - a * b.d(p)) / (Sqr(a) + Sqr(b));
      case Op.Exp:	return a.d(p) * Expo(a);
      case Op.Sinh:	return a.d(p) * Cosh(a);
      case Op.Cosh:	return a.d(p) * Sinh(a);
      case Op.SFres:	return a.d(p) * Sin(Math.PI * Sqr(a) / 2.0);
      case Op.CFres:	return a.d(p) * Cos(Math.PI * Sqr(a) / 2.0);
    }
    return zero;
  }

  public bool IsSubstitionForm() {
    return op == Op.Sub && a.op == Op.Param && b.op == Op.Param;
  }

  public Param GetSubstitutionParamA() {
    if(!IsSubstitionForm()) return null;
    return a.param;
  }

  public Param GetSubstitutionParamB() {
    if(!IsSubstitionForm()) return null;
    return b.param;
  }

  public void Substitute(Param pa, Param pb) {
    if(a != null) {
      a.Substitute(pa, pb);
      if(b != null) {
        b.Substitute(pa, pb);
      }
    } else
    if(op == Op.Param && param == pa) {
      param = pb;
    }
  }

  public void Substitute(Param p, Expression e) {
    if(a != null) {
      a.Substitute(p, e);
      if(b != null) {
        b.Substitute(p, e);
      }
    } else
    if(op == Op.Param && param == p) {
      op = e.op;
      a = e.a;
      b = e.b;
      param = e.param;
      value = e.value;
    }
  }

  public void Walk(Action<Expression> action) {
    action(this);
    if(a != null) {
      action(a);
      if(b != null) {
        action(b);
      }
    }
  }

  public Expression DeepClone() {
    Expression result = new Expression();
    result.op = op;
    result.param = param;
    result.value = value;
    if(a != null) {
      result.a = a.DeepClone();
      if(b != null) {
        result.b = b.DeepClone();
      }
    }
    return result;
  }
	
  public void ReduceParams(List<Param> pars) {
    if(op == Op.Param) {
      if(param.reduceable && !pars.Contains(param)) {
        value = Eval();
        op = Op.Const;
        param = null;
      }
      return;
    }

    if(a != null) {
      a.ReduceParams(pars);
      if(b != null) b.ReduceParams(pars);
      if(a.IsConst() && (b == null || b.IsConst())) {
        value = Eval();
        op = Op.Const;
        a = null;
        b = null;
        param = null;
      }
    }

  }

  public bool HasTwoOperands() {
    return a != null && b != null;
  }

  public Op GetOp() {
    return op;
  }
}
