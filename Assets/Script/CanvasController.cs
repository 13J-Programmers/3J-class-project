using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour {
	public bool isGameFinish = false;

	// Use this for initialization
	void Start() {
		gameObject.GetComponent<Canvas>().enabled = false;
	}
	
	// Update is called once per frame
	void Update() {
		if (isGameFinish) {
			gameObject.GetComponent<Canvas>().enabled = true;
		}
	}
}
