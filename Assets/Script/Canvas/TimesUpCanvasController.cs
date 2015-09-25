using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimesUpCanvasController : MonoBehaviour {
	private Image GetImage() {
		return GameObject.Find("TimesUpCanvas/Image - Time's Up").GetComponent<Image>();
	}

	void Setup() {
		GetImage().enabled = false;
	}
	
	public void SetImage() {
		SetImage(true);
	}

	public void SetImage(bool flag) {
		GetImage().enabled = flag;
	}
}
