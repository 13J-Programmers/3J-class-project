using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {
	public Camera main;
	public Camera second;
	public Key key;

	// Use this for initialization
	void Start() {
		// keyをオブジェクトkeyを格納、CameraChangeを使用できるようにする
		key = GameObject.Find("Key").GetComponent<Key>();
		main.enabled = false;
		second.enabled = true;
	}
	
	// Update is called once per frame
	void Update() {
		if (key.CameraChange() == 1) {
			CameraChange();
		}
	}
	public void CameraChange(){
		main.enabled = !main.enabled;
		second.enabled = !second.enabled;
	}
}
