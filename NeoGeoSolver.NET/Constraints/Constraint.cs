using System.Diagnostics;
using NeoGeoSolver.NET.Entities;

using NeoGeoSolver.NET.Solver;

namespace NeoGeoSolver.NET.Constraints;

public abstract class Constraint {
	private readonly List<IEntity> _entities = new();
	public virtual IEnumerable<Param> parameters { get { yield break; } }
	public virtual IEnumerable<Expression> equations { get { yield break; } }

	private enum Option {
		Default
	}

	protected virtual Enum optionInternal { get { return Option.Default; } set { } }

	protected void AddEntity<T>(T entity) where T : IEntity {
		_entities.Add(entity);
	}

	public void ChooseBestOption() {
		var type = optionInternal.GetType();
		var names = Enum.GetNames(type);
		if(names.Length < 2) return;
		
		var minValue = -1.0;
		var bestOption = 0;
		
		for(var i = 0; i < names.Length; i++) {
			optionInternal = (Enum)Enum.Parse(type, names[i]);
			var exprs = equations.ToList();
			
			var curValue = exprs.Sum(e => Math.Abs(e.Eval()));
			// TODO		Debug.Log(String.Format("check option {0} (min: {1}, cur: {2})\n", optionInternal, min_value, cur_value));
			if(minValue < 0.0 || curValue < minValue) {
				minValue = curValue;
				bestOption = i;
			}
		}
		optionInternal = (Enum)Enum.Parse(type, names[bestOption]);
		// TODO		Debug.Log("best option = " + optionInternal.ToString());
	}

	public IEntity GetEntity(int i) {
		return _entities[i];
	}

	public bool HasEntitiesOfType(EntityType type, int required) {
		var count = 0;
		for(var i = 0; i < _entities.Count; i++) {
			var e = GetEntity(i);
			if(e.type == type) count++;
		}
		return count == required;
	}

	protected IEntity GetEntityOfType(EntityType type, int index) {
		var curIndex = 0;
		for(var i = 0; i < _entities.Count; i++) {
			var e = GetEntity(i);
			if(e.type != type) continue;
			if(curIndex == index) return e;
			curIndex++;
		}
		return null;
	}

	protected void SetEntity(int i, IEntity e) {
		var ent = GetEntity(i) as Entity;
		if(ent != null) {
		}
		ent = GetEntity(i) as Entity;
		if(ent != null) {
		}
	}
}
