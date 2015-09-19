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
	public bool isGamePlayMode = false;
	public string handedness = "right";
	public int lines = 0; // removed lines
	public int score = 0; // obtained score
	public float remainingTime = 180; // sec
	BlockEntity blockEntity;
	GameInfoViewer gameInfoViewer;

	void Awake() {
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();
		gameInfoViewer = GameObject.Find("GameInfoViewer").GetComponent<GameInfoViewer>();
	}

	// Use this for initialization
	void Start() {
		GameStart();
	}
	
	// Update is called once per frame
	void Update() {
		// in game
		if (isGamePlayMode) {
			remainingTime -= Time.deltaTime;
		}

		if (isGamePlayMode && remainingTime <= 0) {
			GameFinish();
		}

		// in result
		if (!isGamePlayMode && Input.GetKey("return")) {
			RestartGame();
		}
	}

	public void GameStart() {
		isGamePlayMode = true;
		score = 0;
		remainingTime = 180;
		blockEntity.CreateRandomBlock();
	}

	public void GameOver() {
		print("GameOver");
		FinishGameProcess();
		var gameoverCanvas = GameObject.Find("GameoverCanvas").GetComponent<ICanvas>();
		gameoverCanvas.ShowResult(score);
	}

	public void GameFinish() {
		print("GameFinish");
		FinishGameProcess();
		var resultCanvas = GameObject.Find("ResultCanvas").GetComponent<ICanvas>();
		resultCanvas.ShowResult(score);
	}

	public void RestartGame() {
		if (GameObject.Find("FedeSystem")) {
			GameObject.Find("FedeSystem").GetComponent<Fade>().LoadLevel("Title", 1f);
		} else {
			Application.LoadLevel("Title");
		}
	}

	// private ------------------------------------------

	/// perform this process when the game is finished.
	private void FinishGameProcess() {
		isGamePlayMode = false;
		gameInfoViewer.enabled = false;
		DisableGameModules();
	}

	/// stop specific game modules
	private void DisableGameModules() {
		// modules to be stopped
		string[] modules = new string[] {
			"Main Camera#CameraController", 
			"BlockEntity#BlockEntity", 
			"LeapHandAction#LeapHandAction", 
			"KeyAction#KeyAction", 
			"BlockPool#BlockPoolController", 
			"block(new)#BlockController,ExpectDropPosViewer", 
		};

		// stop modules
		foreach (string module in modules) {
			string[] tmp = module.Split('#');
			string objectName = tmp[0];
			string[] moduleNames = tmp[1].Split(',');
			foreach (string moduleName in moduleNames) {
				//print(objectName + "." + moduleName);
				Component targetModule = GameObject.Find(objectName).GetComponent(moduleName);
				Destroy(targetModule);
			}
		}
	}
}



