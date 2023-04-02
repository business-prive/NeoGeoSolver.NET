using System.Numerics;
using System.Xml;
using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

[Serializable]
public class HorizontalVertical : Constraint {

	public ExpressionVector p0exp {
		get {
			return GetPointInPlane(0, sketch.plane);
		}
	}

	public ExpressionVector p1exp {
		get {
			return GetPointInPlane(1, sketch.plane);
		}
	}

	ExpressionVector GetPointInPlane(int index, IPlane plane) {
		if(HasEntitiesOfType(IEntityType.Point, 2)) {
			return GetEntityOfType(IEntityType.Point, index).PointExpInPlane(plane);
		}
		return GetEntityOfType(IEntityType.Line, 0).GetPointAtInPlane(index, plane);
	}

	public HorizontalVerticalOrientation orientation = HorizontalVerticalOrientation.OX;

	public HorizontalVertical(Sketch.Sketch sk) : base(sk) { }

	public HorizontalVertical(Sketch.Sketch sk, IEntity p0, IEntity p1) : base(sk) {
		AddEntity(p0);
		AddEntity(p1);
	}

	public HorizontalVertical(Sketch.Sketch sk, IEntity line) : base(sk) {
		AddEntity(line);
	}

	public override IEnumerable<Expression> equations {
		get {
			switch(orientation) {
				case HorizontalVerticalOrientation.OX: yield return p0exp.x - p1exp.x; break;
				case HorizontalVerticalOrientation.OY: yield return p0exp.y - p1exp.y; break;
				case HorizontalVerticalOrientation.OZ: yield return p0exp.z - p1exp.z; break;
			}
		}
	}

	void DrawStroke(LineCanvas canvas, Vector3 p0, Vector3 p1, int rpt) {
		float len = (p1 - p0).magnitude;
		float size = Mathf.Min(len, 20f * getPixelSize());
		Vector3 dir = (p1 - p0).normalized * size / 2f;
		Vector3 pos = (p1 + p0) / 2f;
		ref_points[rpt] = sketch.plane.ToPlane(pos);
		canvas.DrawLine(pos + dir, pos - dir);
	}

	void DrawPointStroke(LineCanvas canvas, Vector3 p0, Vector3 p1, int rpt) {
		if(rpt == 1) {
			var t = p0;
			p0 = p1;
			p1 = t;
		}
		float size = 20f * getPixelSize();
		Vector3 dir = (p1 - p0).normalized * size / 2f;
		ref_points[rpt] = sketch.plane.ToPlane(p0);
		canvas.DrawLine(p0, p0 + dir);
	}

	protected override void OnDraw(LineCanvas canvas) {
		var p0 = GetPointInPlane(0, null).Eval();
		var p1 = GetPointInPlane(1, null).Eval();

		if(HasEntitiesOfType(IEntityType.Line, 1)) {
			DrawStroke(canvas, p0, p1, 0);
			ref_points[1] = ref_points[0];
		} else {
			DrawPointStroke(canvas, p0, p1, 0);
			DrawPointStroke(canvas, p0, p1, 1);
			if(DetailEditor.instance.hovered == this) {
				DrawReferenceLink(canvas, Camera.main);
			}
		}
	}

	/*protected override void OnDraw(LineCanvas canvas) {
		DrawStroke(canvas, l0);
		DrawStroke(canvas, l1);
	}

	protected override bool OnIsChanged() {
		return l0.IsChanged() || l1.IsChanged();
	}*/

	protected override void OnWrite(XmlTextWriter xml) {
		xml.WriteAttributeString("orientation", orientation.ToString());
	}

	protected override void OnRead(XmlNode xml) {
		xml.Attributes["orientation"].Value.ToEnum(ref orientation);
	}
}