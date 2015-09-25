using UnityEngine;
using System;
using System.Collections;

public class Wall : MonoBehaviour {
	Range<float> xRange = new Range<float>(-0, 0);
	Range<float> zRange = new Range<float>(-0, 0);

	public float GetMinX() { return xRange.GetMin(); }
	public float GetMaxX() { return xRange.GetMax(); }
	public float GetMinZ() { return zRange.GetMin(); }
	public float GetMaxZ() { return zRange.GetMax(); }
	public GameObject GetWall(string path) {
		return GameObject.Find("BlockPool/Walls/" + path);
	}

	void Update() {
		float minX = GetWall("min-x").transform.position.x;
		float maxX = GetWall("max-x").transform.position.x;
		float minZ = GetWall("min-z").transform.position.z;
		float maxZ = GetWall("max-z").transform.position.z;
		xRange = new Range<float>(minX, maxX);
		zRange = new Range<float>(minZ, maxZ);
	}
}

public class Range<T> where T : IComparable {
	private T min;
	private T max;

	public T GetMin() { return min; }
	public T GetMax() { return max; }
	private void SetMin(T min) { this.min = min; }
	private void SetMax(T max) { this.max = max; }

	public Range(T min, T max) {
		if (min.CompareTo(max) > 0) throw new RangeException();
		this.SetMin(min);
		this.SetMax(max);
	}

	override
	public string ToString() {
		return "(" + min + ".." + max + ")";
	}
	
	public bool Include(T val) {
		return (min.CompareTo(val) < 0) && (val.CompareTo(max) < 0);
	}
}

public class RangeException : Exception {
	public RangeException() : base() {}
	public RangeException(string message) : base(message) {}
	public RangeException(string message, Exception inner) : base(message, inner) {}
}
