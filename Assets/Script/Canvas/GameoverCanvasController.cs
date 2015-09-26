using UnityEngine;
using System;
using System.Collections;

public class GameoverCanvasController : MonoBehaviour, IResultCanvas {
	private Canvas GetCanvas() {
		return GameObject.Find("GameoverCanvas").GetComponent<Canvas>();
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
	}
}
