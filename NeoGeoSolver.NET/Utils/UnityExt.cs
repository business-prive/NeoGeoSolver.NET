using System.Numerics;

namespace NeoGeoSolver.NET.Utils;

public static class UnityExt
{
	public static Matrix4x4 Basis(Vector3 x, Vector3 y, Vector3 z, Vector3 p)
	{
		Matrix4x4 result = Matrix4x4.Identity;
		result.SetColumn(0, x);
		result.SetColumn(1, y);
		result.SetColumn(2, z);
		Vector4 pp = p;
		pp.w = 1;
		result.SetColumn(3, pp);
		return result;
	}
}
