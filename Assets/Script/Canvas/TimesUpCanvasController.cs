using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimesUpCanvasController : MonoBehaviour {
	private Image timesUpImage;

	void Awake() {
		timesUpImage = GameObject.Find("TimesUpCanvas/Image - Time's Up").GetComponent<Image>();
	}

	void Setup() {
		timesUpImage.enabled = false;
	}
	
	public void SetImage() {
		SetImage(true);
	}

	public void SetImage(bool flag) {
		timesUpImage.enabled = flag;
	}
}
