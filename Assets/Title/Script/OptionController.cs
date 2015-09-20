using UnityEngine;
using System.Collections;

public class OptionController : MonoBehaviour {
	private Key key;
	public Canvas option;
	public Canvas title;

	// Use this for initialization
	void Start () {
		key = GameObject.Find("Key").GetComponent<Key>();
        option = GameObject.Find("Option").GetComponent<Canvas>();
        title = GameObject.Find("Screen").GetComponent<Canvas>();
        option.enabled = false;
		title.enabled = true;
	}

	// Update is called once per frame
	void Update() {
		if (key.KeyO()) { // オプションモードの切り替え
			option.enabled = !option.enabled;
			title.enabled = !title.enabled;
		}
	}
}
