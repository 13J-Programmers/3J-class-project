///
/// @file  GameInfoViewer.cs
/// @brief This script displays important game status to screen.
///

using UnityEngine;
using System.Collections;

public class GameInfoViewer : MonoBehaviour {
	private GameManager gameManager;
	private BlockEntity blockEntity;
	private GUIStyle guiStyle;
	private GUIStyleState guiStyleState;

	// Use this for initialization
	void Start() {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update() {
		guiStyle = new GUIStyle();
		guiStyle.fontSize = 30;
		guiStyleState = new GUIStyleState();
		guiStyleState.textColor = Color.black;//Color.white;
		guiStyle.normal = guiStyleState;
	}

	/// show following item
	///   * lines
	///   * score
	///   * game time
	private void OnGUI() {
		GUI.Label(new Rect(10, Screen.height - guiStyle.fontSize * 3.4f, 100, 60), "Lines : " + gameManager.lines, guiStyle);
		GUI.Label(new Rect(10, Screen.height - guiStyle.fontSize * 2.2f, 100, 60), "Score : " + gameManager.score, guiStyle);
		int timeLeft = (int)gameManager.remainingTime;
		int min = timeLeft / 60;
		int sec = timeLeft % 60;
		string time = "Time Left : ";
		if (min != 0) time += min + "min ";
		time += sec + "sec";
		GUI.Label(new Rect(10, Screen.height - guiStyle.fontSize * 1, 100, 60), time, guiStyle);
	}
}
