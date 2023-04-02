using System.Numerics;
using System.Xml;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Entities;

[Serializable]
public class Circle : Entity, ILoopEntity {

	public Point c;
	public Param radius = new Param("r");

	public override IEntityType type { get { return IEntityType.Circle; } }

	public Circle(Sketch.Sketch sk) : base(sk) {
		c = AddChild(new Point(sk));
	}

	public double rad { get { return radius.value; } set { radius.value = value; } } 

	public override IEnumerable<Point> points {
		get {
			yield return c;
		}
	}

	public override bool IsChanged() {
		return c.IsChanged() || radius.changed;
	}

	public override IEnumerable<Param> parameters {
		get {
			yield return radius;
		}
	}

	public Point center { get { return c; } }
	public IEnumerable<Vector3> loopPoints {
		get {
			float angle = 360;
			var cp = center.pos;
			var rv = Vector3.left * Mathf.Abs((float)radius.value);
			int subdiv = 36;
			var vz = Vector3.forward;
			for(int i = 0; i < subdiv; i++) {
				var nrv = Quaternion.AngleAxis(angle / (subdiv - 1) * i, vz) * rv;
				yield return nrv + cp;
			}
		}
	}

	public override bool IsCrossed(Entity e, ref Vector3 itr) {
		return false;
	}

	protected override void OnWrite(XmlTextWriter xml) {
		xml.WriteAttributeString("r", Math.Abs(radius.value).ToStr());
	}

	protected override void OnRead(XmlNode xml) {
		radius.value = xml.Attributes["r"].Value.ToDouble();
	}

	public override ExpressionVector PointOn(Expression t) {
		var angle = t * 2.0 * Math.PI;
		return c.exp + new ExpressionVector(Expression.Cos(angle), Expression.Sin(angle), 0.0) * Radius();
	}

	public override ExpressionVector TangentAt(Expression t) {
		var angle = t * 2.0 * Math.PI;
		return new ExpressionVector(-Expression.Sin(angle), Expression.Cos(angle), 0.0);
	}

	public override Expression Length() {
		return new Expression(2.0) * Math.PI * Radius();
	}

	public override Expression Radius() {
		return Expression.Abs(radius);
	}

	public override ExpressionVector Center() {
		return center.exp;
	}

}