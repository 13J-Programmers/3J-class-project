///
/// @file  VectorUtil.cs
/// @brief Helper method of Vectors
///

using UnityEngine;
using System;
using System.Collections;

public static class VectorUtil {
	/// round x,z coordinate
	public static Vector3 RoundXZ(Vector3 vector) {
		Vector3 _vector;
		_vector.x = (float)Math.Round(vector.x);
		_vector.y = vector.y;
		_vector.z = (float)Math.Round(vector.z);
		return _vector;
	}

	/// round y coordinate
	public static Vector3 RoundY(Vector3 vector) {
		Vector3 _vector;
		_vector.x = vector.x;
		_vector.y = (float)Math.Round(vector.y);
		_vector.z = vector.z;
		return _vector;
	}

	/// convert Leap Motion Vector to Vector3
	public static Vector3 ToVector3(Leap.Vector v) {
		return new UnityEngine.Vector3(v.x, v.y, v.z);
	}
}
