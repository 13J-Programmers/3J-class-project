using UnityEngine;
using System.Collections;

public class GameoverCanvasController : MonoBehaviour, ICanvas {

	// Use this for initialization
	void Start() {
		gameObject.GetComponent<Canvas>().enabled = false;
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void ShowResult(int score) {
		gameObject.GetComponent<Canvas>().enabled = true;
	}
}
