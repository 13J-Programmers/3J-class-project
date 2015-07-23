using UnityEngine;
using System.Collections;

public class OptionController : MonoBehaviour {
	private Key key;
	public MouseAction mouse;
	public Canvas option;
	public Canvas title;
	// Use this for initialization
	void Start () {
		key = GameObject.Find ("Key").GetComponent<Key> ();
		mouse.enabled = true;
		option.enabled = false;
		title.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (key.OptionTrigger () == true) {//オプションモードの切り替え
			mouse.enabled = !mouse.enabled;
			option.enabled = !option.enabled;
			title.enabled = !title.enabled;
		}
	}
}
