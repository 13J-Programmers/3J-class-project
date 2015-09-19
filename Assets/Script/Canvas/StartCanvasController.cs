using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartCanvasController : MonoBehaviour {
	private Text countUpText;
	private Image startImage;

	void Awake() {
		countUpText = GameObject.Find("StartCanvas/Text - CountUp").GetComponent<Text>();
		startImage = GameObject.Find("StartCanvas/Image - Start").GetComponent<Image>();
	}

	// Use this for initialization
	void Start() {
		countUpText.enabled = true;
		startImage.enabled = false;
	}

	public void SetText(string str) {
		countUpText.text = str;
	}

	public void SetStart() {
		SetStart(true);
	}

	public void SetStart(bool flag) {
		startImage.enabled = flag;
	}
}
