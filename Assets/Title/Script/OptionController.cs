using UnityEngine;
using System.Collections;

public class OptionController : MonoBehaviour {
	private Key key;
	private CameraSwitch cameraswitch;
	public GameObject mouseAction;
	public GameObject titleMode;
	public GameObject optionMode;

	// Use this for initialization
	void Start () {
		key = GameObject.Find ("Key").GetComponent<Key> ();
		cameraswitch = GameObject.Find ("CameraSwitching").GetComponent<CameraSwitch> ();
	}

	// Update is called once per frame
	private bool f = false;
	void Update () {
		MouseAction a = mouseAction.GetComponent<MouseAction> ();
		Canvas b = optionMode.GetComponent<Canvas> ();
		Canvas c = titleMode.GetComponent<Canvas> ();
		if (key.OptionTrigger () == true) {//オプションモードの切り替え
			a.enabled = !a.enabled;
			b.enabled = !b.enabled;
			c.enabled = !c.enabled;
		}
	}
}
