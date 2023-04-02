namespace NeoGeoSolver.NET.Constraints;

public abstract partial class Entity {

  internal void AddConstraint(Constraint c) {
    usedInConstraints.Add(c);
  }

  internal void RemoveConstraint(Constraint c) {
    usedInConstraints.Remove(c);
  }
}