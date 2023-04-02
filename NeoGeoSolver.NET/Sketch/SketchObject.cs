using System.Numerics;
using System.Xml;
using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Sketch;

public abstract class SketchObject : CADObject, ICADObject {

	Sketch sk;
	public Sketch sketch { get { return sk; } }
	public bool isDestroyed { get; private set; }
	public bool isVisible = true;

	Id guid_;
	public override Id guid { get { return guid_; } }

	public override CADObject parentObject {
		get {
			return sketch;
		}
	}

	public override ICADObject GetChild(Id guid) {
		return null;
	}

	public SketchObject(Sketch sketch) {
		sk = sketch;
		guid_ = sketch.idGenerator.New();
	}

	public virtual IEnumerable<Param> parameters { get { yield break; } }
	public virtual IEnumerable<Exp> equations { get { yield break; } }

	protected virtual void OnDrag(Vector3 delta) { }

	public void Drag(Vector3 delta) {
		OnDrag(delta);
	}

	bool hovered;
	public bool isHovered {	get { return hovered; }	set { hovered = value; } }

	bool error;
	public bool isError { get { return error; } set { error = value; } }

	bool selectable = true;
	public bool isSelectable { get { return selectable; } set { selectable = value; } }

	public virtual void Destroy() {
		if(isDestroyed) return;
		isDestroyed = true;
		sketch.Remove(this);
		OnDestroy();
	}

	protected virtual void OnDestroy() {

	}

	public virtual void Write(XmlTextWriter xml) {
		xml.WriteAttributeString("id", guid.ToString());
		if(isVisible == false) xml.WriteAttributeString("visible", isVisible.ToString());
		OnWrite(xml);
	}

	protected virtual void OnWrite(XmlTextWriter xml) {

	}

	public virtual void Read(XmlNode xml) {
		var newGuid = sketch.idGenerator.Create(xml.Attributes["id"].Value);
		if(xml.Attributes["visible"] != null) isVisible = Convert.ToBoolean(xml.Attributes["visible"].Value);
		if(sketch.idMapping != null) {
			sketch.idMapping[newGuid] = guid_;
		} else {
			guid_ = newGuid;
		}
		OnRead(xml);
	}

	protected virtual void OnRead(XmlNode xml)  {
	
	}

	public virtual void Draw(LineCanvas canvas) {
		OnDraw(canvas);
	}

	public virtual bool IsChanged() {
		return OnIsChanged();
	}

	protected virtual bool OnIsChanged() {
		return false;
	}

	protected virtual void OnDraw(LineCanvas canvas) {
		
	}

	public double Select(Vector3 mouse, Camera camera, Matrix4x4 tf) {
		return OnSelect(mouse, camera, tf);
	}

	protected virtual double OnSelect(Vector3 mouse, Camera camera, Matrix4x4 tf) {
		return -1.0;
	}

	public virtual bool MarqueeSelect(Rect rect, bool wholeObject, Camera camera, Matrix4x4 tf) {
		return OnMarqueeSelect(rect, wholeObject, camera, tf);
	}

	protected virtual bool OnMarqueeSelect(Rect rect, bool wholeObject, Camera camera, Matrix4x4 tf) {
		return false;
	}
}