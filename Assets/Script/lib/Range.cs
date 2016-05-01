///
/// @file  Range.cs
/// @brief range instances have max and min value.
///

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Range {
	private Range<int> range;

	public Range(int min, int max) {
		if (min > max) throw new RangeException();
		this.range = new Range<int>(min, max);
	}

	override
	public string ToString() {
		return range.ToString();
	}

	public IEnumerable<int> enumerable {
		get { return ToEnumerable(); }
	}

	public IEnumerable<int> ToEnumerable() {
		for (int iter = range.min; iter < this.range.max; iter++) {
			yield return iter;
		}
	}
}


public class Range<T> where T : IComparable {
	public T min { get; private set; }
	public T max { get; private set; }

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
