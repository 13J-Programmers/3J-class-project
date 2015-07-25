using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public string handedness = "right";
	public int score = 0;
	public int remainingTime = 180; // sec

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		if (remainingTime <= 0)
			GameFinish();
		remainingTime -= Time.deltaTime;
	}

	public void GameStart() {
		
	}

	public void GameOver() {
		// call result scene
	}

	public void GameFinish() {
		// call result scene
	}
}
