using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Utils;

public class ExpressionParser {
	private Dictionary <string, Expression.Op> functions = new()
	{
		{ "sin",	Expression.Op.Sin },
		{ "cos",	Expression.Op.Cos },
		{ "atan2",	Expression.Op.Atan2 },
		{ "sqr",	Expression.Op.Sqr },
		{ "sqrt",	Expression.Op.Sqrt },
		{ "abs",	Expression.Op.Abs },
		{ "sign",	Expression.Op.Sign },
		{ "acos",	Expression.Op.ACos },
		{ "asin",	Expression.Op.Cos },
		{ "exp",	Expression.Op.Exp },
		{ "sinh",	Expression.Op.Sinh },
		{ "cosh",	Expression.Op.Cosh },
		{ "sfres",	Expression.Op.SFres },
		{ "cfres",	Expression.Op.CFres },
	};

	private Dictionary <char, Expression.Op> operators = new()
	{
		{ '+', Expression.Op.Add },
		{ '-', Expression.Op.Sub },
		{ '*', Expression.Op.Mul },
		{ '/', Expression.Op.Div },
	};

	private Dictionary <string, double> constants = new()
	{
		{ "pi", Math.PI },
		{ "e", Math.E },
	};

	private string toParse;
	private int index = 0;
    
	public List<Param> parameters = new();
    
    
	public static void Test() {
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

		foreach(var e in exps) {
			var parser = new ExpressionParser(e);
			var exp = parser.Parse();
			// TODO		Debug.Log("src: \"" + e + "\" -> \"" + exp.ToString() + "\"");
		}

		Dictionary<string, double> results = new()
		{
			{ "2 * 3", 6.0 },
			{ "2 + 1", 3.0 },
			{ "-2 + 2", 0.0 },
			{ "+2 - -2", 4.0 },
			{ "-2 * -2", 4.0 },
			{ "+1 * +2", 2.0 },
			{ "2 + 3 * 6", 20.0 },
			{ "2 + (3 * 6)", 20.0 },
			{ "(2 + 3) * 6", 30.0 },
			{ "((2 + 3) * (6))", 30.0 },
			{ "cos(0)", 1.0 },
			{ "sqr(cos(2)) + sqr(sin(2))", 1.0 },
			{ "pi", Math.PI },
			{ "e", Math.E },
		};

		foreach(var e in results) {
			var parser = new ExpressionParser(e.Key);
			var exp = parser.Parse();
			//TODO		Debug.Log("src: \"" + e + "\" -> \"" + exp.ToString() + "\" = " + exp.Eval().ToStr());
			if(exp.Eval() != e.Value) {
				{
					// TODO		Debug.Log("result fail: get \"" + exp.Eval() + "\" excepted: \"" + e.Value + "\"");
				}
			}
		}

	}

	public ExpressionParser(string str) {
		toParse = str;
	}

	public void SetString(string str) {
		toParse = str;
		index = 0;
	}

	private char next {
		get {
			return toParse[index];
		}
	}

	private bool IsSpace(char c) {
		return Char.IsWhiteSpace(c);
	}

	private bool IsDigit(char c) {
		return Char.IsDigit(c);
	}

	private bool IsDelimiter(char c) {
		return c == '.';
	}

	private bool IsAlpha(char c) {
		return Char.IsLetter(c);
	}

	private void SkipSpaces() {
		if(!HasNext()) return;
		while(HasNext() && IsSpace(next)) index++;
	}

	private Param GetParam(string name) {
		return parameters.Find(p => p.name == name);
	}

	private void Skip(char c) {
		SkipSpaces();
		if(!HasNext() || next != c) {
			error("\"" + c + "\" excepted!");
		}
		index++;
	}

	private bool SkipIf(char c) {
		SkipSpaces();
		if(!HasNext() || next != c) {
			return false;
		}
		index++;
		return true;
	}

	private bool ParseDigits(ref double digits) {
		SkipSpaces();
		if(!HasNext()) error("operand exepted");
		if(!IsDigit(next)) return false;
		var start = index;
		while(HasNext() && (IsDigit(next) || IsDelimiter(next))) index++;
		var str = toParse.Substring(start, index - start);
		digits = str.ToDouble();
		return true;
	}

	private bool ParseAlphas(ref string alphas) {
		SkipSpaces();
		if(!HasNext()) error("operand exepted");
		if(!IsAlpha(next)) return false;
		var start = index;
		while(HasNext() && (IsAlpha(next) || IsDigit(next))) index++;
		alphas = toParse.Substring(start, index - start);
		return true;
	}

	private Expression.Op GetFunction(string name) {
		if(functions.ContainsKey(name)) {
			return functions[name];
		}
		return Expression.Op.Undefined;
	}

	private Expression GetConstant(string name) {
		if(constants.ContainsKey(name)) {
			return constants[name];
		}
		return null;
	}

	private void error(string error = "") {
		var str = toParse;
		if(index < str.Length) {
			str.Insert(index, "?");
		}
		var msg = error + " (error in \"" + str + "\")";
		// TODO		Debug.Log(msg);
		throw new Exception(msg);
	}

	private Expression ParseValue() {
        
		double digits = 0.0;
		if(ParseDigits(ref digits)) {
			return new Expression(digits);
		}
		bool braced = false;
        
		string alphas = "";
		if(ParseAlphas(ref alphas)) {
			var func = GetFunction(alphas);
			if(func != Expression.Op.Undefined) {
				if(SkipIf('(')) {
					Expression a = ParseExp(ref braced);
					Expression b = null;
					if(SkipIf(',')) {
						b = ParseExp(ref braced);
					}
					Skip(')');
					if(func == Expression.Op.Atan2 && b == null) {
						error("second function argument execpted");
					}
					return new Expression(func, a, b);
				} else error("function arguments execpted");
			}

			var constant = GetConstant(alphas);
			if(constant != null) return constant;

			var param = GetParam(alphas);
			if(param == null) {
				param = new Param(alphas);
				parameters.Add(param);
			}
			return new Expression(param);
		}
		error("valid operand excepted");
		return null;
	}

	private int OrderOf(Expression.Op op) {
		switch(op) {
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

	private Expression.Op ParseOp() {
		SkipSpaces();
		if(operators.ContainsKey(next)) {
			var result = operators[next];
			index++;
			return result;
		}
		return Expression.Op.Undefined;
	}

	private bool HasNext() {
		return index < toParse.Length;
	}

	private Expression.Op ParseUnary() {
		SkipSpaces();
		if(next == '+') {
			index++;
			return Expression.Op.Pos;
		}
		if(next == '-') {
			index++;
			return Expression.Op.Neg;
		}
		return Expression.Op.Undefined;
	}

	private Expression ParseExp(ref bool braced) {

		var uop = ParseUnary();
				
		Expression a = null;
		bool aBraced = false;
		if(SkipIf('(')) {
			bool br = false;
			a = ParseExp(ref br);
			Skip(')');
			aBraced = true;
		} else {
			a = ParseValue();
		}
		if(uop != Expression.Op.Undefined && uop != Expression.Op.Pos) {
			a = new Expression(uop, a, null);
		}
        
		SkipSpaces();
		if(!HasNext() || next == ')' || next == ',') {
			braced = aBraced;
			return a;
		}
        
		var op = ParseOp();
		if(op == Expression.Op.Undefined) error("operator execpted");
        
		bool bBraced = false;
		var b = ParseExp(ref bBraced);
        
		if(!bBraced && b.HasTwoOperands() && OrderOf(op) > OrderOf(b.op)) {
			b.a = new Expression(op, a, b.a);
			return b;
		}
		return new Expression(op, a, b);
	}
    
	public Expression Parse() {
		try {
			bool braced = false;
			return ParseExp(ref braced);
		} catch (Exception) {
			return null;
		}
	}
}
