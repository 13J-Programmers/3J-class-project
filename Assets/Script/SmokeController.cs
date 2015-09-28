using UnityEngine;
using System.Collections;

public class SmokeController : MonoBehaviour {
	void Start() {
		Destroy(gameObject, 3.0f);
	}
}
