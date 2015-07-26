using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public string handedness = "right";
	public int score = 0;
	public float remainingTime = 180; // sec
	BlockEntity blockEntity;
//	GamrInfoViewer gameInfo;


	// Use this for initialization
	void Start() {
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();
	//	gameInfo = GameObject.Find("Canvas").GetComponent<GameInfoViewer>();

	}
	
	// Update is called once per frame
	void Update() {
		if (remainingTime <= 0)
			GameFinish();
		remainingTime -= Time.deltaTime;
	}

	public void GameStart() {
		score = 0;
		remainingTime = 180;
		blockEntity.CreateRandomBlock();

	}

	public void GameOver() {
		// call result scene
	}

	public void GameFinish() {
		// call result scene
	}
}
