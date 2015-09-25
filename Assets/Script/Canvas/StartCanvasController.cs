using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartCanvasController : MonoBehaviour {
	private Text GetText() {
		return GameObject.Find("StartCanvas/Text - CountUp").GetComponent<Text>();
	}

	private Image GetImage() {
		return GameObject.Find("StartCanvas/Image - Start").GetComponent<Image>();
	}

	void Start() {
		GetText().enabled = true;
		GetImage().enabled = false;
	}

	public void SetText(string str) {
		GetText().text = str;
	}

	public void SetImage() {
		SetImage(true);
	}

	public void SetImage(bool flag) {
		GetImage().enabled = flag;
	}
}
