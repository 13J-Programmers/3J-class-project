using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public string handedness = "right";
	public int lines = 0; // removed lines
	public int score = 0; // obtained score
	public float remainingTime = 180; // sec
	BlockEntity blockEntity;
//	GamrInfoViewer gameInfo;

	// Use this for initialization
	void Start() {
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();
	//	gameInfo = GameObject.Find("Canvas").GetComponent<GameInfoViewer>();

		GameStart();
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
		print("GameOver");
	}

	public void GameFinish() {
		// call result scene
		print("GameFinish");
	}
}
