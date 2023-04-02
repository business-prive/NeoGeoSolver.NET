using System.Xml;
using NeoGeoSolver.NET.Entities;
using NeoGeoSolver.NET.Sketch;
using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

public class Constraint : SketchObject {
	
	[NonSerialized] public bool changed;
	List<IdPath> ids = new List<IdPath>();
	protected Vector3[] ref_points = new Vector3[2];
	List<Constraint> usedInConstraints = new List<Constraint>();

	enum Option {
		Default
	}

	protected virtual Enum optionInternal { get { return Option.Default; } set { } }

	protected void AddEntity<T>(T e) where T : IEntity {
		if(e is Entities.Entity) (e as Entities.Entity).AddConstraint(this);
		ids.Add(e.id);
	}

	protected void AddObject(ICADObject o) {
		if(o is IEntity) AddEntity(o as IEntity);
		if(o is Constraint) AddConstraint(o as Constraint);
	}

	protected void AddConstraint(Constraint c) {
		c.usedInConstraints.Add(this);
		ids.Add(c.id);
	}

	public Constraint(Sketch.Sketch sk) : base(sk) {
		sk.AddConstraint(this);
	}

	public override void Destroy() {
		if(isDestroyed) return;
		while(usedInConstraints.Count > 0) {
			usedInConstraints[0].Destroy();
		}
		base.Destroy();
		for(int i = 0; i < ids.Count; i++) {
			var ent = GetEntity(i) as Entities.Entity;
			if(ent != null) {
				ent.RemoveConstraint(this);
			} else {
				var c = GetConstraint(i);
				if(c != null) {
					c.usedInConstraints.Remove(this);
				}
			}
		}
	}

	protected override void OnDestroy() {

	}

	public virtual void ChooseBestOption() {
		OnChooseBestOption();
	}

	protected virtual void OnChooseBestOption() {
		var type = optionInternal.GetType();
		var names = Enum.GetNames(type);
		if(names.Length < 2) return;
		
		double min_value = -1.0;
		int best_option = 0;
		
		for(int i = 0; i < names.Length; i++) {
			optionInternal = (Enum)Enum.Parse(type, names[i]);
			List<Exp> exprs = equations.ToList();
			
			double cur_value = exprs.Sum(e => Math.Abs(e.Eval()));
			Debug.Log(String.Format("check option {0} (min: {1}, cur: {2})\n", optionInternal, min_value, cur_value));
			if(min_value < 0.0 || cur_value < min_value) {
				min_value = cur_value;
				best_option = i;
			}
		}
		optionInternal = (Enum)Enum.Parse(type, names[best_option]);
		Debug.Log("best option = " + optionInternal.ToString());
	}

	public override void Write(XmlTextWriter xml) {
		xml.WriteStartElement("constraint");
		xml.WriteAttributeString("type", this.GetType().Name);
		if(Enum.GetNames(optionInternal.GetType()).Length >= 2) {
			xml.WriteAttributeString("chirality", optionInternal.ToString());
		}
		base.Write(xml);
		foreach(var id in ids) {
			xml.WriteStartElement("link");
			xml.WriteAttributeString("path", id.ToString());
			xml.WriteEndElement();
		}
		xml.WriteEndElement();
	}

	public override void Read(XmlNode xml) {
		ids.Clear();
		if(Enum.GetNames(optionInternal.GetType()).Length >= 2) {
			Enum output = optionInternal;
			xml.Attributes["chirality"].Value.ToEnum(ref output);
			optionInternal = output;
		}
		foreach(XmlNode node in xml.ChildNodes) {
			if(node.Name != "entity" && node.Name != "link") continue;
			var path = IdPath.From(node.Attributes["path"].Value);
			ICADObject o = null;
			if(sketch.idMapping != null) {
				o = sketch.GetChild(sketch.idMapping[path.path.Last()]);
			} else {
				o = sketch.feature.detail.GetObjectById(path);
			}
				
			AddObject(o);
		}
		base.Read(xml);
	}

	public IEntity GetEntity(int i) {
		return sketch.feature.detail.GetObjectById(ids[i]) as IEntity;
	}

	public IEnumerable<ICADObject> objects {
		get {
			foreach(var id in ids) {
				yield return sketch.feature.detail.GetObjectById(id) as ICADObject;
			}
		}
	}

	public Constraint GetConstraint(int i) {
		return sketch.feature.detail.GetObjectById(ids[i]) as Constraint;
	}

	public int GetEntitiesCount() {
		return ids.Count;
	}

	public bool HasEntitiesOfType(IEntityType type, int required) {
		int count = 0;
		for(int i = 0; i < GetEntitiesCount(); i++) {
			var e = GetEntity(i);
			if(e.type == type) count++;
		}
		return count == required;
	}

	public IEntity GetEntityOfType(IEntityType type, int index) {
		int curIndex = 0;
		for(int i = 0; i < GetEntitiesCount(); i++) {
			var e = GetEntity(i);
			if(e.type != type) continue;
			if(curIndex == index) return e;
			curIndex++;
		}
		return null;
	}

	protected void SetEntity(int i, IEntity e) {
		var ent = GetEntity(i) as Entities.Entity;
		if(ent != null) {
			ent.RemoveConstraint(this);
		}
		ids[i] = e.id;
		ent = GetEntity(i) as Entities.Entity;
		if(ent != null) {
			ent.AddConstraint(this);
		}
		changed = true;
	}

	public override bool IsChanged() {
		return base.IsChanged() || changed;
	}

	public bool ReplaceEntity(IEntity before, IEntity after) {
		bool result = false;
		var beforeId = before.id;
		for(int i = 0; i < ids.Count; i++) {
			if(ids[i] != beforeId) continue;
			SetEntity(i, after);
			result = true;
		}
		return result;
	}

	protected static float length(Vector3 v) {
		return v.magnitude;
	}

	protected static Vector3 normalize(Vector3 v) {
		return v.normalized;
	}

	public static float EPSILON   = 1e-4f;
	public static float R_ARROW_H = 2.5f;
	public static float R_ARROW_W = 12.5f;
	public static float R_CIRLE_R = 5f;
	public static float R_DASH    = 8f;

	protected IPlane getPlane() {
		return sketch.plane;
	}

	public static float getPixelSize() {
		return (float)DraftStroke.getPixelSize();
	}
	
	public void DrawReferenceLink(LineCanvas renderer, Camera camera) {
		float pix = getPixelSize();
		float size = 12f * pix;
		var ref_points = this.ref_points.Select(p => sketch.plane.FromPlane(p)).ToArray();
		drawCameraCircle(renderer, camera, ref_points[0], size, 16);
		if(ref_points.Length > 1) {
			drawCameraCircle(renderer, camera, ref_points[1], size, 16);
			if(length(ref_points[1] - ref_points[0]) > 2f * size) {
				Vector3 dir = normalize(ref_points[1] - ref_points[0]);
				drawDottedLine(ref_points[0] + dir * size, ref_points[1] - dir * size, renderer, R_DASH * pix);
			}
		}
	}

	Vector3 rotatedAbout(Vector3 v, Vector3 axis, float angle) {
		return Quaternion.AxisAngle(axis, angle) * v;
	}

	protected void drawDottedLine(Vector3 p0, Vector3 p1, LineCanvas renderer, float step) {
		if(step == 0f) {
			renderer.DrawLine(p0, p1);
			return;
		}
		float len = length(p1 - p0);
		Vector3 dir = normalize(p1 - p0);
		Vector3 p = p0;
		int count = (int)Math.Floor(len / step);
		if(count > 1000) {
			count = 1000;
			step = len / (count - 1f);
		}
		
		bool draw = 0 % 2 == 0;
		for(int i=0; i<count; i++) {
			if(draw) renderer.DrawLine(p, p + dir * step);
			p += dir * step;
			draw = (i + 1) % 2 == 0;
		}
		if(draw) {
			float frac = len - count * step;
			if(draw) renderer.DrawLine(p, p + dir * frac);
		}
	}
	
	protected void drawAngleArc(LineCanvas renderer, Vector3 p0, Vector3 c, float angle, Vector3 vz, bool dash = false, float step = 0f) {
		float subdiv = 32f;
		if(step > 0f) {
			float len = length(p0 - c) * angle;
			subdiv = len / step;
			if(subdiv > 1000f) subdiv = 1000f;
		}
		
		Vector3 rv = p0 - c;
		Vector3 orv = rv;
		
		float i = 0f;
		int index = 0;
		while(i < subdiv) {
			i += 1f;
			if(i > subdiv) i = subdiv;
			Vector3 nrv = rotatedAbout(rv, vz, angle / subdiv * i);
			if(!dash || index % 2 == 0) renderer.DrawLine(orv + c, nrv + c);
			orv = nrv;
			index += 1;
		}
	}
	
	protected void drawArc(LineCanvas renderer, Vector3 p0, Vector3 p1, Vector3 c, Vector3 vz, bool dash = false, float step = 0f) {
		float angle = Mathf.Acos(Vector3.Dot(normalize(p0 - c), normalize(p1 - c)));
		float subdiv = 32f;
		
		if(step > 0f) {
			float len = length(p0 - c) * angle;
			subdiv = len / step;
			if(subdiv > 1000f) subdiv = 1000f;
		}
		
		if(Vector3.Dot(Vector3.Cross(p0 - c, p1 - c), vz) < 0f) angle = -angle;
		
		Vector3 rv = p0 - c;
		Vector3 orv = rv;
		
		float i = 0f;
		int index = 0;
		while(i < subdiv) {
			i += 1f;
			if(i > subdiv) i = subdiv;
			Vector3 nrv = rotatedAbout(rv, vz, angle / subdiv * i);
			if(!dash || index % 2 == 0) renderer.DrawLine(orv + c, nrv + c);
			orv = nrv;
			index += 1;
		}
	}
	
	void drawArcExtend(LineCanvas renderer, Vector3 p0, Vector3 p1,
		Vector3 c, Vector3 to, Vector3 vz,
		bool dash, float step) {
		float dd0 = Vector3.Dot(Vector3.Cross(p0 - c, p1 - c), vz);
		bool greater180 = dd0 < 0;
		Vector3 c0 = Vector3.Cross(to - c, p0 - c);
		Vector3 c1 = Vector3.Cross(to - c, p1 - c);
		float d0 = Vector3.Dot(c0, vz);
		float d1 = Vector3.Dot(c1, vz);
		
		if(greater180) {
			if(d0 < 0f || d1 > 0f) return;
		} else {
			if(!(d1 < 0f || d0 > 0f)) return;
		}
		
		Vector3 from = p0;
		if(length(to - p1) < length(to - p0)) from = p1;
		drawArc(renderer, from, to, c, vz, dash, step);
	}
	
	bool drawLineExtend(LineCanvas renderer, Vector3 p0, Vector3 p1, Vector3 to, float step, float salient, bool from_to) {
		Vector3 dir = p1 - p0;
		float k = Vector3.Dot(dir, to - p0) / Vector3.Dot(dir, dir);
		Vector3 pt_on_line = p0 + dir * k;
		drawDottedLine(to, pt_on_line, renderer, step);
		Vector3 sd = Vector3.zero;
		if(salient > 0f) sd = normalize(dir) * salient;
		if(k < 0f) {
			if(from_to) {
				drawDottedLine(pt_on_line - sd, p0, renderer, step);
			} else {
				drawDottedLine(p0, pt_on_line - sd, renderer, step);
			}
			return true;
		} else
		if(k > 1f) {
			if(from_to) {
				drawDottedLine(pt_on_line + sd, p1, renderer, step);
			} else {
				drawDottedLine(p1, pt_on_line + sd, renderer, step);
			}
			return true;
		}
		return false;
	}
	
	protected bool drawLineExtendInPlane(IPlane plane, LineCanvas renderer, Vector3 p0, Vector3 p1, Vector3 to, float step, float salient = 0f, bool from_to = false) {
		Vector3 dir = p1 - p0;
		if(plane != null) {
			dir = plane.projectVectorInto(p1) - plane.projectVectorInto(p0);
		}
		float k = Vector3.Dot(dir, to - p0) / Vector3.Dot(dir, dir);
		Vector3 pt_on_line = p0 + (p1 - p0) * k;
		drawDottedLine(to, pt_on_line, renderer, step);
		Vector3 sd = Vector3.zero;
		if(salient > 0f) sd = normalize(dir) * salient;
		if(k < 0f) {
			if(from_to) {
				drawDottedLine(pt_on_line - sd, p0, renderer, step);
			} else {
				drawDottedLine(p0, pt_on_line - sd, renderer, step);
			}
			return true;
		} else
		if(k > 1f) {
			if(from_to) {
				drawDottedLine(pt_on_line + sd, p1, renderer, step);
			} else {
				drawDottedLine(p1, pt_on_line + sd, renderer, step);
			}
			return true;
		}
		return false;
	}
	
	void drawTangentCross(LineCanvas renderer, Vector3 pos, Vector3 dir, Vector3 pn, float pix) {
		float size = 10f * pix;
		Vector3 perp = Vector3.Cross(dir, pn);
		renderer.DrawLine(pos - perp * size, pos + perp * size);
		renderer.DrawLine(pos - dir * size, pos + dir * size);
	}
	
	protected void drawCameraCircle(LineCanvas renderer, Camera camera, Vector3 pos, float size, int num_segments = 32) {
		float angle = 2f * Mathf.PI / (float)num_segments;
		Vector3 r0 = camera.transform.right * size;
		for(int i=0; i<num_segments; i++) {
			Vector3 r1 = rotatedAbout(r0, camera.transform.forward, angle);
			renderer.DrawLine(pos + r0, pos + r1);
			r0 = r1;
		}
	}
	
	void drawGrounding(LineCanvas renderer, Camera camera, Vector3 pos, float size) {
		Vector3 x = camera.transform.right * size;
		Vector3 y = -camera.transform.up * size;
		
		renderer.DrawLine(pos, pos + y * 5f);
		renderer.DrawLine(pos + y * 5f + x * 5f, pos + y * 5f - x * 3f);
		
		// physical ground
		for(float i = -3f; i <= 5f; i += 2f) {
			renderer.DrawLine(pos + y * 5f + x * i, pos + y * 8f + x * (i - 2f));
		}
		
		// electronic ground
		// renderer.DrawLine(pos + y * 8f + x * 3f, pos + y * 8f - x * 3f);
		// renderer.DrawLine(pos + y * 11f + x * 1f, pos + y * 11f - x * 1f);
	}
	
	protected Vector3 drawPointProjection(LineCanvas renderer, Vector3 point, float step) {
		IPlane plane = getPlane();
		if(plane == null) return point;
		Vector3 proj = plane.projectVectorInto(point);
		if(proj != point) drawDottedLine(point, proj, renderer, step);
		return proj;
	}
	
	protected Matrix4x4 getPointLineDistanceBasis(Vector3 lip0_, Vector3 lip1_, Vector3 ap_, IPlane plane) {
	
		Vector3 lip0 = lip0_;
		Vector3 lip1 = lip1_;
		Vector3 ap = ap_;
		
		if(plane != null) {
			lip0 = plane.projectVectorInto(lip0);
			lip1 = plane.projectVectorInto(lip1);
			ap = plane.projectVectorInto(ap);
		}
		
		Vector3 lid = normalize(lip1 - lip0);
		Vector3 bp = lip0 + lid * Vector3.Dot(ap - lip0, lid);
		Vector3 x = normalize(bp - ap);
		Vector3 y = lid;
		Vector3 z = normalize(Vector3.Cross(x, y));
		Vector3 p = (ap + bp) * 0.5f;
		return UnityExt.Basis(x, y, z, p);
	}
	
	protected Matrix4x4 getPointsDistanceBasis(Vector3 app, Vector3 bpp, IPlane plane) {
		if(plane == null) return Matrix4x4.Translate((app + bpp) / 2f);
		Vector3 z = plane.n;
		Vector3 ap = plane.projectVectorInto(app);
		Vector3 bp = plane.projectVectorInto(bpp);
		Vector3 x = normalize(bp - ap);
		Vector3 y = normalize(Vector3.Cross(x, z));
		Vector3 p = (ap + bp) * 0.5f;
		return UnityExt.Basis(x, y, z, p);
	}
	
	protected Vector3 projectPointLine(Vector3 p, Vector3 p0, Vector3 p1) {
		Vector3 dir = p1 - p0;
		float k = Vector3.Dot(p - p0, dir) / Vector3.Dot(dir, dir);
		return p0 + dir * k;
	}
	
	float projectPointLineCoeff(Vector3 p, Vector3 p0, Vector3 p1) {
		Vector3 dir = p1 - p0;
		return Vector3.Dot(p - p0, dir) / Vector3.Dot(dir, dir);
	}
	
	Vector3 getBasisPlaneDir(Vector3 def) {
		if(getPlane() != null) return getPlane().n;
		return def;
	}
	
	protected Vector3 getVisualPlaneDir(Vector3 def) {
		if(getPlane() != null) return getPlane().n;
		return def;
	}

	protected override double OnSelect(Vector3 mouse, Camera camera, Matrix4x4 tf) {
		double result = -1.0;
		for(int i = 0; i < ref_points.Length; i++) {
			var pp = camera.WorldToScreenPoint(tf.MultiplyPoint(ref_points[i]));
			pp.z = 0f;
			mouse.z = 0f;
			var dist = (pp - mouse).magnitude - 7;
			if(dist < 0f) return 0f;
			if(result > 0.0 && dist > result) continue;
			result = dist;
		}
		return result;
	}

	protected override bool OnMarqueeSelect(Rect rect, bool wholeObject, Camera camera, Matrix4x4 tf) {
		for(int i = 0; i < ref_points.Length; i++) {
			Vector2 pp = camera.WorldToScreenPoint(tf.MultiplyPoint(ref_points[i]));
			if(rect.Contains(pp)) return true;
		}
		return false;
	}

	public static Constraint New(string typeName, Sketch.Sketch sk) {
		Type[] types = { typeof(Sketch.Sketch) };
		object[] param = { sk };
		var type = Type.GetType(typeName);
		if(type == null) {
			Debug.LogError("Can't create entity of type " + typeName);
			return null;
		}
		return type.GetConstructor(types).Invoke(param) as Constraint;
	}
}