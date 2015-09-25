using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ResultCanvasController : MonoBehaviour, IResultCanvas {
	public string[] titles = new string[] {
		"Basic Ecorist", "Good Ecorist", "Super Ecorist", "Eco Master"
	};

	private Canvas GetCanvas() {
		return GameObject.Find("ResultCanvas").GetComponent<Canvas>();
	}

	private Text GetText() {
		return GameObject.Find("ResultCanvas/Text - Result").GetComponent<Text>();
	}

	private Image GetImage(string titleName) {
		string path = "ResultCanvas/Title/Image - " + titleName;
		if (!GameObject.Find(path)) throw new Exception("not found image."); 
		return GameObject.Find(path).GetComponent<Image>();
	}

	// Use this for initialization
	void Start() {
		GameManager.FinishGame += new EventHandler(ShowResult);
	}

	public void ShowResult(object sender, EventArgs e) {
		GameManager game = (GameManager)sender;
		ShowResult(game.score);
	}

	/// Show result screen.
	/// Display the gained score and more details.
	/// Selecting display title depends on player's score.
	/// 
	/// @param score - player's score
	public void ShowResult(int score) {
		GetCanvas().enabled = true;

		string title;
		if (score < 500) {
			title = "Basic Ecorist";
		} else if (score < 1000) {
			title = "Good Ecorist";
		} else if (score < 2000) {
			title = "Super Ecorist";
		} else {
			title = "Eco Master";
		}
		GetImage(title).enabled = true;

		GetText().text = "Score : " + score;
	}
}

