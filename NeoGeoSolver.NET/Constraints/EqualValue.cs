using System.Numerics;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

[Serializable]
public class EqualValue : Value {

	public EqualValue(Sketch.Sketch sk) : base(sk) {
	}

	protected override bool OnSatisfy() {
		var c0 = GetConstraint(0) as Value;
		var c1 = GetConstraint(1) as Value;
		if(Math.Sign(c0.GetValue()) != Math.Sign(c1.GetValue())) {
			value.value = -1;
		}
		return true;
	}

	public EqualValue(Sketch.Sketch sk, Value c0, Value c1) : base(sk) {
		AddConstraint(c0);
		AddConstraint(c1);
		value.value = 1.0;
		Satisfy();
	}

	public override IEnumerable<Expression> equations {
		get {
			var c0 = GetConstraint(0) as Value;
			var c1 = GetConstraint(1) as Value;
			yield return c0.GetValueParam().exp - c1.GetValueParam().exp * value;
		}
	}

	void DrawStroke(LineCanvas canvas, Value c, int rpt) {

		ref_points[rpt] = c.pos;
		if(rpt == 0) {
			Vector3 up = c.GetBasis().GetColumn(1);
			pos = c.pos + up.normalized * getPixelSize() * 30f;
		}
	}

	protected override void OnDraw(LineCanvas canvas) {
		DrawStroke(canvas, GetConstraint(0) as Value, 0);
		DrawStroke(canvas, GetConstraint(1) as Value, 1);
		
		if(DetailEditor.instance.hovered == this) {
			DrawReferenceLink(canvas, Camera.main);
		}
	}

	protected override string OnGetLabelValue() {
		return base.OnGetLabelValue() + ":1";
	}

	protected override Matrix4x4 OnGetBasis() {
		return sketch.plane.GetTransform();
	}
}