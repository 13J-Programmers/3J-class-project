///
/// @file  Range.cs
/// @brief range instances have max and min value.
///

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Range {
	private int min;
	private int max;
	private int iter;

	public Range(int min, int max) {
		if (min > max) throw new RangeException();
		this.min = min;
		this.max = max;
		this.iter = this.min;
	}

	public IEnumerable<int> enumerable {
		get { return ToEnumerable(); }
	}

	public IEnumerable<int> ToEnumerable() {
		while (this.iter < this.max) {
			yield return this.iter++;
		}
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

