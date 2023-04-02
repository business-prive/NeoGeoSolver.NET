namespace NeoGeoSolver.NET.Sketch;

public abstract class CADObject : ICADObject {
  public abstract Id guid { get; }
  public abstract ICADObject GetChild(Id guid);
  public abstract CADObject parentObject { get; }

  public IdPath id {
    get {
      return GetRelativePath(null);
    }
  }

  public IdPath GetRelativePath(CADObject from) {
    var result = new IdPath();
    var p = this;
    while(p != null) {
      if(p == from) return result;
      if(p.guid == Id.Null) return result;
      result.path.Insert(0, p.guid);
      p = p.parentObject;
    }
    return result;
  }

  public virtual ICADObject GetObjectById(IdPath id, int index = 0) {
    if(id.path.Count == 0) return null;
    var r = GetChild(id.path[index]);
    var co = r as CADObject;
    if(co == null || index + 1 >= id.path.Count) return r;
    return co.GetObjectById(id, index + 1);
  }
}