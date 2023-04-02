using System.Numerics;
using System.Xml;

using NeoGeoSolver.NET.Solver;
using NeoGeoSolver.NET.Utils;

namespace NeoGeoSolver.NET.Constraints;

[Serializable]
public class ValueConstraint : Constraint {

  protected Param value = new Param("value");
  bool reference_;
  public bool reference {
    get {
      return reference_;
    }
    set {
      reference_ = value;
      sketch.MarkDirtySketch(constraints:true);
    }
  }
  Vector3 position_;

  public Vector3 labelPos {
    get {
      return position_;
    }
  }
  public float labelX { get { return position_.x; } set { position_.x = value; } }
  public float labelY { get { return position_.y; } set { position_.y = value; } }
  public float labelZ { get { return position_.z; } set { position_.z = value; } }

  public virtual bool valueVisible { get { return true; } }

  protected bool selectByRefPoints = false;
	
  [SerializeField]
  public Vector3 pos {
    get {
      return GetBasis().MultiplyPoint(position_);
    }
    set {
      var newPos = GetBasis().inverse.MultiplyPoint(value);
      if(position_ == newPos) return;
      position_ = newPos;
      if(!sketch.is3d) {
        position_.z = 0;
      }
      changed = true;
      //behaviour.Update();
    }
  }

  public ValueConstraint(Sketch.Sketch sk) : base(sk) {}

  public override IEnumerable<Param> parameters {
    get {
      if(!reference) yield break;
      yield return value;
    }
  }

  protected override void OnDrag(Vector3 delta) {
    if(!valueVisible) return;
    if(delta == Vector3.zero) return;
    pos += delta;
  }

  public Matrix4x4 GetBasis() {
    return OnGetBasis()/* * sketch.plane.GetTransform()*/;
  }

  protected virtual Matrix4x4 OnGetBasis() {
    return Matrix4x4.identity;
  }

  public double GetValue() {
    return ValueToLabel(value.value);
  }

  protected virtual string OnGetLabelValue() {
    return Math.Abs(GetValue()).ToString("0.##");
  }

  public string GetLabel() {
    var v = OnGetLabelValue();
    if(reference) v = "<" + v + ">";
    return v;
  }

  public void SetValue(double v) {
    value.value = LabelToValue(v);
  }

  public Param GetValueParam() {
    return value;
  }

  public double dimension { get { return GetValue(); } set { SetValue(value); } }

  public virtual double ValueToLabel(double value) {
    return value;
  }

  public virtual double LabelToValue(double label) {
    return label;
  }

  protected virtual bool OnSatisfy() {
    EquationSystem sys = new EquationSystem();
    sys.revertWhenNotConverged = false;
    sys.AddParameter(value);
    sys.AddEquations(equations);
    return sys.Solve() == EquationSystem.SolveResult.OKAY;
  }

  public bool Satisfy() {
    var result = OnSatisfy();
    if(!result) {
      Debug.LogWarning(GetType() + " satisfy failed!");
    }
    return result;
  }

  protected void setRefPoint(Vector3 pos) {
    ref_points[0] = sketch.plane.ToPlane(pos);
  }

  protected sealed override void OnWrite(XmlTextWriter xml) {
    xml.WriteAttributeString("x", pos.x.ToStr());
    xml.WriteAttributeString("y", pos.y.ToStr());
    xml.WriteAttributeString("z", pos.z.ToStr());
    xml.WriteAttributeString("value", GetValue().ToStr());
    xml.WriteAttributeString("reference", reference.ToString());
    OnWriteValueConstraint(xml);
  }

  protected virtual void OnWriteValueConstraint(XmlTextWriter xml) {

  }

  protected sealed override void OnRead(XmlNode xml) {
    Vector3 pos;
    pos.x = xml.Attributes["x"].Value.ToFloat();
    pos.y = xml.Attributes["y"].Value.ToFloat();
    pos.z = xml.Attributes["z"].Value.ToFloat();
    this.pos = pos;
    SetValue(xml.Attributes["value"].Value.ToDouble());
    if(xml.Attributes["reference"] != null) {
      reference = Convert.ToBoolean(xml.Attributes["reference"].Value);
    }
    OnReadValueConstraint(xml);
  }

  protected virtual void OnReadValueConstraint(XmlNode xml) {

  }

  protected override double OnSelect(Vector3 mouse, Camera camera, Matrix4x4 tf) {
    double distRp = -1;
    if(selectByRefPoints) {
      distRp = base.OnSelect(mouse, camera, tf);
    }
    var pp = camera.WorldToScreenPoint(tf.MultiplyPoint(sketch.plane.ToPlane(pos)));
    pp.z = 0f;
    mouse.z = 0f;
    var dist = (pp - mouse).magnitude - 10;
    if(dist < 0f) return 0f;
    return (distRp >= 0.0) ? Math.Min(dist, distRp) : dist;
  }

  protected override bool OnMarqueeSelect(Rect rect, bool wholeObject, Camera camera, Matrix4x4 tf) {
    if(selectByRefPoints) {
      if(base.OnMarqueeSelect(rect, wholeObject, camera, tf)) return true;
    }
    Vector2 pp = camera.WorldToScreenPoint(tf.MultiplyPoint(sketch.plane.ToPlane(pos)));
    if(rect.Contains(pp)) return true;
    return false;
  }


  protected void drawPointLineDistance(Vector3 lip0_, Vector3 lip1_, Vector3 p0_, LineCanvas renderer, Camera camera) {
		
    float pix = getPixelSize();
		
    Vector3 lip0 = drawPointProjection(renderer, lip0_, R_DASH * pix);
    Vector3 lip1 = drawPointProjection(renderer, lip1_, R_DASH * pix);
    Vector3 p0 = drawPointProjection(renderer, p0_, R_DASH * pix);
		
    if(lip0 != lip0_ || lip1 != lip1_) {
      drawDottedLine(lip0, lip1, renderer, R_DASH * pix);
    }
		
    Matrix4x4 basis = getPointLineDistanceBasis(lip0, lip1, p0, getPlane());
		
    Vector3 lid = normalize(lip1 - lip0);
		
    Vector3 p1 = lip0 + lid * Vector3.Dot(p0 - lip0, lid);
		
    Vector3 vx = basis.GetColumn(0);
    Vector3 vy = basis.GetColumn(1);
    Vector3 vp = basis.GetColumn(3);
		
    Vector3 label_offset = getLabelOffset();
    Vector3 offset = Vector3.zero;
    offset.x = Vector3.Dot(label_offset - vp, vx);
    offset.y = Vector3.Dot(label_offset - vp, vy);
		
    // sgn label y
    float sy = ((offset.y  > EPSILON) ? 1f : 0f) - ((offset.y < -EPSILON) ? 1f : 0f);
		
    Vector3 lp0 = p0 + vy * offset.y;
    Vector3 lp1 = p1 + vy * offset.y;
		
    // vertical lines
    renderer.DrawLine(p0, lp0 + vy * 8f * pix * sy);
		
    float lk = Vector3.Dot(lp1 - lip0, lid);
    if(lk < 0f) {
      renderer.DrawLine(lip0, lp1 + normalize(lp1 - lip0) * 8f * pix);
    } else if(lk > length(lip1 - lip0)) {
      renderer.DrawLine(lip1, lp1 + normalize(lp1 - lip1) * 8f * pix);
    }
		
    // distance line
    renderer.DrawLine(lp0, lp1);
		
    // sgn arrow x
    float sx = 1f;
		
    // half distance
    float half_dist = length(p0 - p1) * 0.5f;
		
    if(Mathf.Abs(offset.x) > half_dist) sx = -1f;
		
    if(sx < 0f || length(lp0 - lp1) > (R_ARROW_W * 2f + 1f) * pix) {
      // arrow lp0
      renderer.DrawLine(lp0, lp0 - vy * R_ARROW_H * pix + vx * R_ARROW_W * pix * sx);
      renderer.DrawLine(lp0, lp0 + vy * R_ARROW_H * pix + vx * R_ARROW_W * pix * sx);
			
      // arrow lp1
      renderer.DrawLine(lp1, lp1 - vy * R_ARROW_H * pix - vx * R_ARROW_W * pix * sx);
      renderer.DrawLine(lp1, lp1 + vy * R_ARROW_H * pix - vx * R_ARROW_W * pix * sx);
    } else {
      // stroke lp0
      renderer.DrawLine(lp0 - vy * R_ARROW_H * pix + vx * R_ARROW_H * pix, lp0 + vy * R_ARROW_H * pix - vx * R_ARROW_H * pix);
			
      // stroke lp1
      renderer.DrawLine(lp1 - vy * R_ARROW_H * pix + vx * R_ARROW_H * pix, lp1 + vy * R_ARROW_H * pix - vx * R_ARROW_H * pix);
    }
		
    Vector3 lv0 = lp0;
    Vector3 lv1 = lp1;
    //bool da1 = arrow1;
		
    // if label lays from other side
    if(offset.x > half_dist) {
      lv0 = lp1;
      lv1 = lp0;
      //da1 = arrow0;
    }
		
    // if label is ouside
    if(Mathf.Abs(offset.x) > half_dist) {
			
      Vector3 dir = vp + vy * offset.y + vx * offset.x - lv0;
      float len = Mathf.Max(length(dir), 21f * pix);
			
      // line to the label
      renderer.DrawLine(lv0, lv0 + normalize(dir) * len);
			
      // opposite arrow line
      /*if(da1)*/ renderer.DrawLine(lv1, lv1 - normalize(dir) * 21f * pix);
      setRefPoint(lv0 + normalize(dir) * (len + 16f * pix));
    } else {
      setRefPoint(basis.MultiplyPoint(offset) + vy * sy * 13f * pix);
    }
		
    //drawLabel(renderer, camera);
  }
	
  protected Vector3 getLabelOffset() {
    return pos;
  }

  protected void drawPointsDistance(Vector3 pp0, Vector3 pp1, LineCanvas renderer, Camera camera, bool label = false, bool arrow0 = true, bool arrow1 = true, int style = 0) {
    float pix = getPixelSize();
		
    Vector3 p0 = drawPointProjection(renderer, pp0, R_DASH * pix);
    Vector3 p1 = drawPointProjection(renderer, pp1, R_DASH * pix);
		
    Matrix4x4 basis;
		
    if(getPlane() == null) {
      Vector3 p = getLabelOffset();
      Vector3 x = normalize(p1 - p0);
      Vector3 y;
      y = p - projectPointLine(p, p0, p1);
      if(length(y) < EPSILON) y = Vector3.Cross(camera.transform.forward, x);
      y = normalize(y);
      Vector3 z = Vector3.Cross(x, y);
      basis = UnityExt.Basis(x, y, z, (p0 + p1) * 0.5f);
    } else {
      basis = getPointsDistanceBasis(p0, p1, getPlane());
    }
		
    Vector3 vx = basis.GetColumn(0);
    Vector3 vy = basis.GetColumn(1);
    Vector3 vp = basis.GetColumn(3);
		
    Vector3 label_offset = getLabelOffset();
    Vector3 offset = Vector3.zero;
    offset.x = Vector3.Dot(label_offset - vp, vx);
    offset.y = Vector3.Dot(label_offset - vp, vy);
		
    // sgn label y
    float sy = ((offset.y  > EPSILON) ? 1f : 0f) - ((offset.y < -EPSILON) ? 1f : 0f);
    offset.y = sy * Mathf.Max(15f * pix, Mathf.Abs(offset.y));
		
    // distance line points
    Vector3 lp0 = p0 + vy * offset.y;
    Vector3 lp1 = p1 + vy * offset.y;
		
    // vertical lines
    if(Mathf.Abs(sy) > EPSILON) {
      Vector3 salient = vy * 8f * pix * sy;
      if(style == 0) {
        renderer.DrawLine(p0, lp0 + salient);
        renderer.DrawLine(p1, lp1 + salient);
      } else {
        renderer.DrawLine(lp0 - salient, lp0 + salient);
        renderer.DrawLine(lp1 - salient, lp1 + salient);
      }
    }
		
    // distance line
    renderer.DrawLine(lp0, lp1);
		
    // sgn arrow x
    float sx = 1f;
		
    // half distance
    float half_dist = length(p0 - p1) * 0.5f;
		
    // if label ouside
    if(Mathf.Abs(offset.x) > half_dist) sx = -1f;
		
    // if label ourside distance area or sceren distance not too small, draw arrows
    if((sx < 0f || length(lp0 - lp1) > (R_ARROW_W * 2f + 1f) * pix) && style != 1) {
      // arrow lp0
      if(arrow0) {
        renderer.DrawLine(lp0, lp0 - vy * R_ARROW_H * pix + vx * R_ARROW_W * pix * sx);
        renderer.DrawLine(lp0, lp0 + vy * R_ARROW_H * pix + vx * R_ARROW_W * pix * sx);
      }
			
      // arrow lp1
      if(arrow1) {
        renderer.DrawLine(lp1, lp1 - vy * R_ARROW_H * pix - vx * R_ARROW_W * pix * sx);
        renderer.DrawLine(lp1, lp1 + vy * R_ARROW_H * pix - vx * R_ARROW_W * pix * sx);
      }
    } else {
      // stroke lp0
      renderer.DrawLine(lp0 - vy * R_ARROW_H * pix + vx * R_ARROW_H * pix, lp0 + vy * R_ARROW_H * pix - vx * R_ARROW_H * pix);
			
      // stroke lp1
      renderer.DrawLine(lp1 - vy * R_ARROW_H * pix + vx * R_ARROW_H * pix, lp1 + vy * R_ARROW_H * pix - vx * R_ARROW_H * pix);
    }
		
    Vector3 lv0 = lp0;
    Vector3 lv1 = lp1;
    bool da1 = arrow1;
		
    // if label lays from other side
    if(offset.x > half_dist) {
      lv0 = lp1;
      lv1 = lp0;
      da1 = arrow0;
    }
		
    // if label is ouside
    if(Mathf.Abs(offset.x) > half_dist) {
			
      Vector3 dir = vp + vy * offset.y + vx * offset.x - lv0;
      float len = Mathf.Max(length(dir), 21f * pix);
			
      // line to the label
      renderer.DrawLine(lv0, lv0 + normalize(dir) * len);
			
      // opposite arrow line
      if(da1) renderer.DrawLine(lv1, lv1 - normalize(dir) * 21f * pix);
      setRefPoint(lv0 + normalize(dir) * (len + 16f * pix));
    } else {
      setRefPoint(basis.MultiplyPoint(offset) + vy * sy * 13f * pix);
    }
		
    //drawCameraCircle(renderer, camera, getLabelOffset(), 3f * pix);
    //if(label) drawLabel(renderer, camera);
  }

  protected void drawBasis(LineCanvas canvas) {
    var basis = GetBasis();
    var pix = getPixelSize();
    Vector3 vx = basis.GetColumn(0);
    Vector3 vy = basis.GetColumn(1);
    //Vector3 vz = basis.GetColumn(2);
    Vector3 p = basis.GetColumn(3);
    canvas.DrawLine(p, p + vx * 10f * pix);
    canvas.DrawLine(p, p + vy * 10f * pix);
  }

  public override void Draw(LineCanvas canvas) {
    base.Draw(canvas);
    //drawBasis(canvas);
  }

  protected void drawArrow(LineCanvas canvas, Vector3 pos, Vector3 dir, bool stroke = false) {
    dir = dir.normalized;
    var f = getVisualPlaneDir(Camera.main.transform.forward);
    var n = Vector3.Cross(dir, f).normalized;
    var pix = getPixelSize();

    // if label ourside distance area or sceren distance not too small, draw arrows
    if(!stroke) {
      canvas.DrawLine(pos, pos - n * R_ARROW_H * pix - dir * R_ARROW_W * pix);
      canvas.DrawLine(pos, pos + n * R_ARROW_H * pix - dir * R_ARROW_W * pix);
    } else {
      canvas.DrawLine(pos - n * R_ARROW_H * pix + dir * R_ARROW_H * pix, pos + n * R_ARROW_H * pix - dir * R_ARROW_H * pix);
    }
  }
}