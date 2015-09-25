using UnityEngine;
using System;
using System.Collections;

public class GameoverCanvasController : MonoBehaviour, IResultCanvas {
	void Start() {
		GameManager.EndGame += new EventHandler(ShowResult);
	}

	public void ShowResult(object sender, EventArgs e) {
		GameManager game = (GameManager)sender;
		ShowResult(game.score);
	}

	public void ShowResult(int score) {
		gameObject.GetComponent<Canvas>().enabled = true;
	}
}
