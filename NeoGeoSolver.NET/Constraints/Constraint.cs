using System.Diagnostics;
using System.Numerics;
using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public abstract class Constraint {
	public bool changed;
	protected Vector3[] ref_points = new Vector3[2];
	private List<Constraint> usedInConstraints = new List<Constraint>();
	public virtual IEnumerable<Param> parameters { get { yield break; } }
	public virtual IEnumerable<Expression> equations { get { yield break; } }

	private enum Option {
		Default
	}

	protected virtual Enum optionInternal { get { return Option.Default; } set { } }

	protected void AddEntity<T>(T e) where T : IEntity {
		if(e is Entities.Entity) (e as Entities.Entity).AddConstraint(this);
		ids.Add(e.id);
	}

	protected void AddConstraint(Constraint c) {
		c.usedInConstraints.Add(this);
		ids.Add(c.id);
	}

	public Constraint(Sketch.Sketch sk) : base(sk) {
		sk.AddConstraint(this);
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
			List<Expression> exprs = equations.ToList();
			
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

	public IEntity GetEntity(int i) {
		return sketch.feature.detail.GetObjectById(ids[i]) as IEntity;
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
}
