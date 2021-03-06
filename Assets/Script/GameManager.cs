﻿///
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
using System;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using Leap;

public class GameManager : MonoBehaviour {
	public bool isCountDownMode = false;
	public bool isGamePlayMode = false;
	public int isTimesUpMode = 0; ///< 0: not started, 1: times up mode, 2: finish
	public bool isGameFinish = false;

	public string handedness = "right";
	public int lines = 0; ///< removed lines
	public int score = 0; ///< obtained score
	public float remainingTime = 90; ///< sec

	private GameInfoViewer GetGameInfoViewer() {
		return GameObject.Find("GameInfoViewer").GetComponent<GameInfoViewer>();
	}
	private StartCanvasController GetStartCanvasController() {
		return GameObject.Find("StartCanvas").GetComponent<StartCanvasController>();
	}
	private TimesUpCanvasController GetTimesUpCanvasController() {
		return GameObject.Find("TimesUpCanvas").GetComponent<TimesUpCanvasController>();
	}

	public static event EventHandler StartGame;
	public static event EventHandler FinishGame;
	public static event EventHandler EndGame; ///< when game-over

	public AudioClip audioClip;
	AudioSource[] audioSources;

	void Awake() {
		InitEveryStaticField();
	}

	// Use this for initialization
	void Start() {
		// start count down
		isCountDownMode = true;
		StartCoroutine(CountDown());
		audioSources = gameObject.GetComponents<AudioSource>();
	}

	// game flow:
	//
	//        |transition
	//        ∨
	//     CountDown (3,2,1,start!)
	//        |
	//        ∨
	//     GamePlay------------+
	//        |                |
	//        |times_up        |pool_overflow
	//        ∨                |
	//     TimesUp             |
	//        |                |
	//        ∨                ∨
	//     ShowResult       GameOver
	//        |                |
	//        ∨                |
	//     RestartGame <~------+
	//        |
	//        |transition
	//        ∨
	//
	void Update() {
		if (Input.GetKey(KeyCode.Escape)) RestartGame();

		// start
		if (isCountDownMode) return;
		if (!isCountDownMode && !isGamePlayMode && !isGameFinish) {
			GameStart();
		}

		// in game
		if (isGamePlayMode) {
			remainingTime -= Time.deltaTime;
		}

		// to finish
		if (isGamePlayMode && remainingTime <= 1) {
			GetGameInfoViewer().enabled = false;

			// display "Time's Up"
			if (isTimesUpMode == 0) {
				isTimesUpMode = 1;
				DisableGameModules();
				StartCoroutine(TimesUp());
			}

			if (isTimesUpMode == 1) return;
			GameFinish();
		}

		// in result
		if (!isGamePlayMode
				&& (Input.GetKey("return") || LeapGesture()) ) {
			RestartGame();
		}
	}

	public void GameStart() {
		isGamePlayMode = true;
		int playAudioIndex = UnityEngine.Random.Range(0, 2); // 0以上2未満の整数乱数を返す
		audioSources[playAudioIndex].Play();

		if (StartGame != null) {
			StartGame(this, EventArgs.Empty);
		}
	}

	public void GameOver() {
		FinishGameProcess();
		DisableGameModules();

		if (EndGame != null) {
			EndGame(this, EventArgs.Empty);
		}
	}

	public void GameFinish() {
		FinishGameProcess();

		if (FinishGame != null) {
			FinishGame(this, EventArgs.Empty);
		}

		InitEveryStaticField();
	}

	public void RestartGame() {
		if (GameObject.Find("FadeSystem")) {
			GameObject.Find("FadeSystem").GetComponent<Fade>().LoadLevel("Title", 1f);
		} else {
			//Application.LoadLevel("Title");
			SceneManager.LoadScene("Title");
		}
	}

	// private ------------------------------------------

	/// display number to count down.
	/// then shows image "start!".
	private IEnumerator CountDown() {
		// display number
		GetStartCanvasController().SetText("3");
		yield return new WaitForSeconds(1);
		GetStartCanvasController().SetText("2");
		yield return new WaitForSeconds(1);
		GetStartCanvasController().SetText("1");
		yield return new WaitForSeconds(1);
		GetStartCanvasController().SetText("");
		// display image
		GetStartCanvasController().SetImage();
		isCountDownMode = false;
		yield return new WaitForSeconds(1);
		GetStartCanvasController().SetImage(false);
	}

	/// display image "Times Up" while 2 seconds.
	private IEnumerator TimesUp() {
		// display image
		GetTimesUpCanvasController().SetImage();
		yield return new WaitForSeconds(2);
		GetTimesUpCanvasController().SetImage(false);
		isTimesUpMode = 2;
	}

	/// perform this process when the game is finished.
	private void FinishGameProcess() {
		isGamePlayMode = false;
		isGameFinish = true;
		GetGameInfoViewer().enabled = false;
	}

	/// stop specific game modules
	private void DisableGameModules() {
		string[] moduleNames = {
			"Main Camera#CameraController",
			"BlockEntity#BlockEntity",
			"LeapHandAction#LeapHandAction",
			"KeyAction#KeyAction",
		};

		var moduleInfos = moduleNames
			.Select(str => str.Split('#'))
			.Select(ary => new ComponentInfo(ary[0], ary[1]));

		foreach (var moduleInfo in moduleInfos) {
			Destroy(moduleInfo.GetComponent());
		}
	}

	// The static field events badly influence game scene after play first.
	// Therefore, static fields should initialize with null.
	public void InitEveryStaticField() {
		this.InitStaticField();
		BlockEntity.InitStaticField();
		BlockController.InitStaticField();
	}

	public void InitStaticField() {
		StartGame = FinishGame = EndGame = null;
	}

	//
	// for leap motion
	//
	private bool LeapGesture() {
		Hand hand = new Controller().Frame().Hands[0];

		return (isFingersFolded(hand) && hand.PalmVelocity.z > 300);
	}

	private bool isFingersFolded(Hand hand) {
		Vector origin = hand.Fingers[0].TipPosition;
		float dist = 0;

		foreach (Finger finger in hand.Fingers) {
			dist += finger.TipPosition.DistanceTo(origin);
		}
		return (dist < 250) ? true : false;
	}
}
