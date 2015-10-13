using UnityEngine;
using System;
using System.Collections;

public class Test {
	static public bool test(Func<bool> predicate) {
		try {
			return predicate();
		} catch (Exception) {
			return false;
		}
	}
}
