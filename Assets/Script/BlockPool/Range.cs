using UnityEngine;
using System;
using System.Collections;

public class Range<T> where T : IComparable {
	private T min;
	private T max;

	public T GetMin() { return min; }
	public T GetMax() { return max; }
	private void SetMin(T min) { this.min = min; }
	private void SetMax(T max) { this.max = max; }

	public Range(T min, T max) {
		if (min.CompareTo(max) > 0) throw new RangeException();
		this.min = min;
		this.max = max;
	}

	override
	public string ToString() {
		return "(" + min + ".." + max + ")";
	}
	
	public bool Include(T val) {
		return (min.CompareTo(val) <= 0) && (val.CompareTo(max) < 0);
	}
}

public class RangeException : Exception {
	public RangeException() : base() {}
	public RangeException(string message) : base(message) {}
	public RangeException(string message, Exception inner) : base(message, inner) {}
}

