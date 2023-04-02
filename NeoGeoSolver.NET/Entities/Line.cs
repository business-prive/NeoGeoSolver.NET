using System.Numerics;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public class Line : Entity, ISegmentaryEntity {

	public Point p0;
	public Point p1;

	public Line(Sketch.Sketch sk) : base(sk) {
		p0 = AddChild(new Point(sk));
		p1 = AddChild(new Point(sk));
	}

	public override IEntityType type { get { return IEntityType.Line; } }

	public override IEnumerable<Point> points {
		get {
			yield return p0;
			yield return p1;
		}
	}

	public override bool IsChanged() {
		return p0.IsChanged() || p1.IsChanged();
	}

	public Point begin { get { return p0; } }
	public Point end { get { return p1; } }
	public IEnumerable<Vector3> segmentPoints {
		get {
			yield return p0.GetPosition();
			yield return p1.GetPosition();
		}
	}

	public override BBox bbox { get { return new BBox(p0.pos, p1.pos); } }

	protected override Entity OnSplit(Vector3 position) {
		var part = new Line(sketch);
		part.p1.pos = p1.pos;
		p1.pos = position;
		part.p0.pos = p1.pos;
		return part;
	}

	protected override double OnSelect(Vector3 mouse, Camera camera, Matrix4x4 tf) {
		var ap = camera.WorldToScreenPoint(tf.MultiplyPoint(p0.GetPosition()));
		var bp = camera.WorldToScreenPoint(tf.MultiplyPoint(p1.GetPosition()));
		return GeomUtils.DistancePointSegment2D(mouse, ap, bp);
	}

	protected override bool OnMarqueeSelect(Rect rect, bool wholeObject, Camera camera, Matrix4x4 tf) {
		var ap = camera.WorldToScreenPoint(tf.MultiplyPoint(p0.GetPosition()));
		var bp = camera.WorldToScreenPoint(tf.MultiplyPoint(p1.GetPosition()));
		return MarqueeSelectSegment(rect, wholeObject, ap, bp);
	}

	public override ExpressionVector PointOn(Expression t) {
		var pt0 = p0.exp;
		var pt1 = p1.exp;
		return pt0 + (pt1 - pt0) * t;
	}

	public override ExpressionVector TangentAt(Expression t) {
		return p1.exp - p0.exp;
	}

	public override Expression Length() {
		return (p1.exp - p0.exp).Magnitude();
	}

	public override Expression Radius() {
		return null;
	}
}