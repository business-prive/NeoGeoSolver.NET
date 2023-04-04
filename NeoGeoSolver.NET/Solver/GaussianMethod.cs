namespace NeoGeoSolver.NET.Solver;

public static class GaussianMethod {

	public const double Epsilon = 1e-10;
	public const double RankEpsilon = 1e-8;
	
	public static string Print<T>(this T[,] a) {
		var result = "";
		for(var r = 0; r < a.GetLength(0); r++) {
			for(var c = 0; c < a.GetLength(1); c++) {
				result += a[r, c] + " ";
			}
			result += "\n";
		}
		return result;
	}

	public static string Print<T>(this T[] a) {
		var result = "";
		for(var r = 0; r < a.GetLength(0); r++) {
			result += a[r] + "\n";
		}
		return result;
	}
	
	public static int Rank(double[,] a) {
		var rows = a.GetLength(0);
		var cols = a.GetLength(1);

		var rank = 0;
		var rowsLength = new double[rows];

		for(var i = 0; i < rows; i++) {
			for(var ii = 0; ii < i; ii++) {
				if(rowsLength[ii] <= RankEpsilon) continue;

				double sum = 0;
				for(var j = 0; j < cols; j++) {
					sum += a[ii, j] * a[i, j];
				}
				for(var j = 0; j < cols; j++) {
					a[i, j] -= a[ii, j] * sum / rowsLength[ii];
				}
			}

			double len = 0;
			for(var j = 0; j < cols; j++) {
				len += a[i, j] * a[i, j];
			}
			if(len > RankEpsilon) {
				rank++;
			}
			rowsLength[i] = len;
		}

		return rank;
	}

	public static void Solve(double[,] a, double[] b, ref double[] x) {

		var rows = a.GetLength(0);
		var cols = a.GetLength(1);
		var t = 0.0;

		for(var r = 0; r < rows; r++) {

			var mr = r;
			var max = 0.0;
			for(var rr = r; rr < rows; rr++) {
				if(Math.Abs(a[rr, r]) <= max) continue;
				max = Math.Abs(a[rr, r]);
				mr = rr;
			}

			if(max < Epsilon) continue;

			for(var c = 0; c < cols; c++) {
				t = a[r, c];
				a[r, c] = a[mr, c];
				a[mr, c] = t;
			}

			t = b[r];
			b[r] = b[mr];
			b[mr] = t;

			// normalize
			/*
		double scale = A[r, r];
		for(int c = 0; c < cols; c++) {
			A[r, c] /= scale;
		}
		B[r] /= scale;
		*/

			// 
			for(var rr = r + 1; rr < rows; rr++) {
				var coef = a[rr, r] / a[r, r];
				for(var c = 0; c < cols; c++) {
					a[rr, c] -= a[r, c] * coef;
				}
				b[rr] -= b[r] * coef;
			}
		}

		for(var r = rows - 1; r >= 0; r--) {
			if(Math.Abs(a[r, r]) < Epsilon) continue;
			var xx = b[r] / a[r, r];
			for(var rr = rows - 1; rr > r; rr--) {
				xx -= x[rr] * a[r, rr] / a[r, r];
			}
			x[r] = xx;
		}
	}
}
