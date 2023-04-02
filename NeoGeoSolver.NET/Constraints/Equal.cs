using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public class Equal : Value {

	public Equal(Sketch.Sketch sk) : base(sk) { selectByRefPoints = true; }

	public enum LengthType {
		Length,
		Radius,
		Diameter
	}

	private LengthType[] lengthType = new LengthType[2];
	
	public Equal(Sketch.Sketch sk, IEntity l0, IEntity l1) : base(sk) {
		AddEntity(l0);
		AddEntity(l1);
		value.value = 1.0;
		selectByRefPoints = true;
	}

	public override IEnumerable<Expression> equations {
		get {
			Expression[] len = new Expression[2];

			for(int i = 0; i < 2; i++) {
				var e = GetEntity(i);
				switch(lengthType[i]) {
					case LengthType.Length: len[i] = e.Length(); break;
					case LengthType.Radius: len[i] = e.Radius(); break;
					case LengthType.Diameter: len[i] = e.Radius() * 2.0; break;
				}
			}
			yield return len[0] - len[1] * value;
		}
	}
}
