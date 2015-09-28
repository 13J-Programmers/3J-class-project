using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameoverCanvasController : MonoBehaviour, IResultCanvas {
	private Canvas GetCanvas() {
		return GameObject.Find("GameoverCanvas").GetComponent<Canvas>();
	}

	private Text GetText() {
		return GameObject.Find("GameoverCanvas/Text - Result").GetComponent<Text>();
	}

	void Start() {
		GameManager.EndGame += new EventHandler(ShowResult);
	}

	private void ShowResult(object sender, EventArgs e) {
		GameManager game = (GameManager)sender;
		ShowResult(game.score);
	}

	public void ShowResult(int score) {
		GetCanvas().enabled = true;
		GetText().text = "Score : " + score;
	}
}
