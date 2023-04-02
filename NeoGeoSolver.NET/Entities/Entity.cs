using System.Numerics;
using System.Xml;
using NeoGeoSolver.NET.Constraints;
using NeoGeoSolver.NET.Sketch;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Entities;

public abstract partial class Entity : SketchObject, IEntity {

	protected List<Constraint> usedInConstraints = new List<Constraint>();
	List<Entity> children = new List<Entity>();
	public Entity parent { get; private set; }
	public Func<ExpVector, ExpVector> transform = null;
	public IEnumerable<Constraint> constraints { get { return usedInConstraints.AsEnumerable(); } }
	public virtual IEnumerable<PointEntity> points { get { yield break; } }
	public virtual BBox bbox { get { return new BBox(Vector3.zero, Vector3.zero); } }
	public abstract IEntityType type { get; }

	public IPlane plane {
		get {
			return sketch.plane;
		}
	}

	public int GetChildrenCount() {
		return children.Count;
	}

	IEnumerable<ExpVector> IEntity.points {
		get {
			for(var it = points.GetEnumerator(); it.MoveNext(); ) {
				yield return it.Current.exp;
			}
		}
	}

	public virtual IEnumerable<Vector3> segments {
		get {
			if(this is ISegmentaryEntity) return (this as ISegmentaryEntity).segmentPoints;
			if(this is ILoopEntity) return (this as ILoopEntity).loopPoints;
			return Enumerable.Empty<Vector3>();
		}
	}

	protected IEnumerable<Vector3> getSegmentsUsingPointOn(int subdiv) {
		Param pOn = new Param("pOn");
		var on = PointOn(pOn);
		for(int i = 0; i <= subdiv; i++) {
			pOn.value = (double)i / subdiv;
			yield return on.Eval();
		}
	}

	protected IEnumerable<Vector3> getSegments(int subdiv, Func<double, Vector3> pointOn) {
		for (int i = 0; i <= subdiv; i++) {
			yield return pointOn((double)i / subdiv);
		}
	}

	public abstract ExpVector PointOn(Exp t);

	public T AddChild<T>(T e) where T : Entity {
		children.Add(e);
		e.parent = this;
		return e;
	}

	public Entity(Sketch.Sketch sketch) : base(sketch) {
		sketch.AddEntity(this);
	}

	protected override void OnDrag(Vector3 delta) {
		foreach(var p in points) {
			p.Drag(delta);
		}
	}

	public override void Destroy() {
		if(isDestroyed) return;
		while(usedInConstraints.Count > 0) {
			usedInConstraints[0].Destroy();
		}
		base.Destroy();
		if(parent != null) {
			parent.Destroy();
		}
		while(children.Count > 0) {
			children[0].Destroy();
			children.RemoveAt(0);
		}
	}

	public override void Write(XmlTextWriter xml) {
		xml.WriteStartElement("entity");
		xml.WriteAttributeString("type", this.GetType().Name);
		base.Write(xml);
		if(children.Count > 0) {
			foreach(var c in children) {
				c.Write(xml);
			}
		}
		xml.WriteEndElement();
	}

	public override void Read(XmlNode xml) {
		base.Read(xml);
		int i = 0;
		foreach(XmlNode xmlChild in xml.ChildNodes) {
			if(children.Count <= i) {
				var type = xmlChild.Attributes["type"].Value;
				AddChild(New(type, sketch));
			}
			children[i].Read(xmlChild);
			i++;
		}
	}

	public virtual bool IsCrossed(Entity e, ref Vector3 itr) {
		var boxZero = new BBox(Vector3.zero, Vector3.zero);
		if(!e.bbox.Overlaps(bbox) && !e.bbox.Equals(boxZero) && !bbox.Equals(boxZero)) return false;
		if(this is ISegmentaryEntity && e is ISegmentaryEntity) {
			var self = this as ISegmentaryEntity;
			var entity = e as ISegmentaryEntity;

			Vector3 selfPrev = Vector3.zero;
			bool selfFirst = true;
			foreach(var sp in self.segmentPoints) {
				if(!selfFirst) {
					Vector3 otherPrev = Vector3.zero;
					bool otherFirst = true;
					foreach(var ep in entity.segmentPoints) {
						if(!otherFirst) {
							if(GeomUtils.isSegmentsCrossed(selfPrev, sp, otherPrev, ep, ref itr, 1e-6f) == GeomUtils.Cross.INTERSECTION) {
								if(this as Entity == e && selfPrev == otherPrev && sp == ep) continue;
								return true;
							}
						}
						otherFirst = false;
						otherPrev = ep;
					}
				}
				selfFirst = false;
				selfPrev = sp;
			}
		}
		return false;
	}

	public void ForEachSegment(Func<Vector3, Vector3, bool> action) {
		IEnumerable<Vector3> points = null;
		if(this is ISegmentaryEntity) points = (this as ISegmentaryEntity).segmentPoints;
		if(this is ILoopEntity) points = (this as ILoopEntity).loopPoints;
		if(points == null) return;
		Vector3 prev = Vector3.zero;
		bool first = true;
		foreach(var ep in points) {
			if(!first) {
				if(!action(prev, ep)) {
					return;
				}
			}
			first = false;
			prev = ep;
		}
	}

	public bool IsEnding(PointEntity p) {
		if(!(this is ISegmentaryEntity)) return false;
		var se = this as ISegmentaryEntity;
		return se.begin == p || se.end == p;
	}

	protected virtual Entity OnSplit(Vector3 position) {
		return null;
	}

	public Entity Split(Vector3 position) {
		return OnSplit(position);
	}

	protected override double OnSelect(Vector3 mouse, Camera camera, Matrix4x4 tf) {
		double minDist = -1.0;
		ForEachSegment((a, b) => {
			var ap = camera.WorldToScreenPoint(tf.MultiplyPoint(a));
			var bp = camera.WorldToScreenPoint(tf.MultiplyPoint(b));
			var dist = Mathf.Abs(GeomUtils.DistancePointSegment2D(mouse, ap, bp));
			if(minDist < 0.0 || dist < minDist) {
				minDist = dist;
			}
			return true;
		});
		return minDist;
	}

	protected static bool MarqueeSelectSegment(Rect rect, bool wholeObject, Vector3 ap, Vector3 bp) {
		if(wholeObject) {
			if(rect.Contains(ap) && rect.Contains(bp)) {
				return true; 
			}
		} else {
			if(rect.Contains(ap) || rect.Contains(bp)) {
				return true; 
			}
			var line = Rect.MinMaxRect(
				Mathf.Min(ap.x, bp.x), 
				Mathf.Min(ap.y, bp.y),
				Mathf.Max(ap.x, bp.x), 
				Mathf.Max(ap.y, bp.y)
			);
			if(!rect.Overlaps(line)) {
				return false;
			}
			Vector3 res = Vector3.zero;
			Vector3[] points = {
				new Vector3(rect.xMin, rect.yMin),
				new Vector3(rect.xMax, rect.yMin),
				new Vector3(rect.xMax, rect.yMax),
				new Vector3(rect.xMin, rect.yMax)
			};
			for(int i = 0; i < points.Length; i++) {
				if(GeomUtils.isSegmentsCrossed(ap, bp, points[i], points[(i + 1) % points.Length], ref res, 1e-6f) == GeomUtils.Cross.INTERSECTION) {
					return true;
				}
			}
			return false;
		}
		return false;
	}

	protected override bool OnMarqueeSelect(Rect rect, bool wholeObject, Camera camera, Matrix4x4 tf) {
		var any = false;
		var whole = true;
		ForEachSegment((a, b) => {
			Vector2 ap = camera.WorldToScreenPoint(tf.MultiplyPoint(a));
			Vector2 bp = camera.WorldToScreenPoint(tf.MultiplyPoint(b));
			var segSelected = MarqueeSelectSegment(rect, wholeObject, ap, bp);
			any = any || segSelected;
			if(!wholeObject && any) {
				// break for each loop
				return false; 
			}
			whole = whole && segSelected;
			if(wholeObject && !whole) {
				// break for each loop
				return false;
			}
			// continue for each loop
			return true;
		});
		return wholeObject && whole || !wholeObject && any;
	}


	protected override void OnDraw(LineCanvas canvas) {
		if(isError) {
			canvas.SetStyle("error");
		} else {
			canvas.SetStyle("entities");
		}
		ForEachSegment((a, b) => {
			canvas.DrawLine(a, b);
			return true;
		});
	}

	public virtual ExpVector TangentAt(Exp t) {
		Param p = new Param("pOn");
		var pt = PointOn(p);
		var result = new ExpVector(pt.x.Deriv(p), pt.y.Deriv(p), pt.z.Deriv(p));
		result.x.Substitute(p, t);
		result.y.Substitute(p, t);
		result.z.Substitute(p, t);
		return result;
	}

	public abstract Exp Length();
	public abstract Exp Radius();

	public virtual ExpVector Center() {
		return null;
	}

	public static Entity New(string typeName, Sketch.Sketch sk) {
		Type[] types = { typeof(Sketch.Sketch) };
		object[] param = { sk };
		var type = Type.GetType(typeName);
		if(type == null) {
			Debug.LogError("Can't create entity of type " + typeName);
			return null;
		}
		return type.GetConstructor(types).Invoke(param) as Entity;
	}

}