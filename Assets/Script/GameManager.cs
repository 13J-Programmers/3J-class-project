/// 
/// @file  GameManager.cs
/// @brief This script manages important game state and moves game mode.
/// 

/**
 * \mainpage Ecoris - Eco Tetris in 3D
 * 
 * \section s1 About this
 * 
 * Ecoris is the abbreviation of Eco-tetris.
 * 
 * features:
 * - play tetris in 3d
 * - gameplay with LEAP-MOTION
 * - thinking ecology in this game
 * - award the title depending on your score
 * 
 * \section s2 See Also
 * 
 * Every codes of this project are open.
 * 
 * - \link https://github.com/13J-Programmers/3J_class_project \endlink
 */

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public string handedness = "right";
	public int lines = 0; // removed lines
	public int score = 0; // obtained score
	public float remainingTime = 180; // sec
	BlockEntity blockEntity;
	//GamrInfoViewer gameInfo;

	// Use this for initialization
	void Start() {
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();
		//gameInfo = GameObject.Find("Canvas").GetComponent<GameInfoViewer>();

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
		var resultCanvas = 
			GameObject.Find("ResultCanvas").GetComponent<CanvasController>();
		resultCanvas.ShowResult(score);
		var gameInfoViewer = 
			GameObject.Find("GameInfoViewer").GetComponent<GameInfoViewer>();
		gameInfoViewer.enabled = false;
	}
}
