using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CanvasController : MonoBehaviour {
	public bool isGameFinish = false;
	public string[] titles = new string[] {
		"Basic Ecorist", "Good Ecorist", "Super Ecorist", "Eco Master"
	};

	private Dictionary<string, Image> titleImages = new Dictionary<string, Image>();
	private Text resultText;

	void Awake() {
		resultText = GameObject.Find("ResultCanvas/Text - Result").GetComponent<Text>();
	}

	// Use this for initialization
	void Start() {
		gameObject.GetComponent<Canvas>().enabled = false;

		// init to set Images component
		foreach (string titleName in titles) {
			string path = "ResultCanvas/Title/Image - " + titleName;
			titleImages[titleName] = GameObject.Find(path).GetComponent<Image>();
		}

		// un-enable image
		foreach (string titleName in titles) {
			titleImages[titleName].enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (isGameFinish) {
			ShowResult(0);
		}
	}

	/// Show result screen.
	/// Display the gained score and more details.
	/// Selecting display title depends on player's score.
	/// 
	/// @param score - player's score
	public void ShowResult(int score) {
		gameObject.GetComponent<Canvas>().enabled = true;

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
		titleImages[title].enabled = true;

		resultText.text = "Score : " + score;
	}
}

