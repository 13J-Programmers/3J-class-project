using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
		switchScene ();
	}
	public int CameraChange(){
		int f;
		if (Input.GetKeyDown (KeyCode.Q)) {
			f=1;
		}
		else f = 0;
		return(f);
	}
	public bool OptionTrigger(){
		bool f=false;
		if (Input.GetKeyDown (KeyCode.O)) {
			f=true;
		}
		return(f);
	}
	public string nextSceneName;
	public void switchScene(){//キーボードでシーン切り替え
		if (Input.GetKeyDown (KeyCode.Return)) {
			GameObject.Find ("SoundBox").GetComponent<SoundController> ().SoundSE ();
			GameObject.Find ("FedeSystem").GetComponent<Fade> ().LoadLevel (nextSceneName, 1f);
		}
	}
}
