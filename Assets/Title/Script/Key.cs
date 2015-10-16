using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	private string nextSceneName;

	void Start()
	{
		nextSceneName = "Main";
	}

	// Update is called once per frame
	void Update() {
		if (KeyEnter())
			switchScene();
	}

	public bool KeyQ() {
		bool f = false;
		if (Input.GetKeyDown(KeyCode.Q)) f = true;
		return f;
		//^ 上のコードは三項演算子を使って、よりコンパクトにすることができます (mako)
		//  return (Input.GetKeyDown(KeyCode.Q)) ? true : false;
	}

	public bool KeyO(){
		bool f = false;
		if (Input.GetKeyDown(KeyCode.O)) f = true;
		return f;
	}

	public bool KeyEnter()
	{
		bool f = false;
		if (Input.GetKeyDown(KeyCode.Return)) f = true;
		return f;
	}
	
	public void switchScene()
	{ // キーボードでシーン切り替え
		PlayCount playCount = GameObject.Find("Screen/PlayCount").GetComponent<PlayCount>();
		playCount.increasePlayNum();
		GameObject.Find("SoundBox").GetComponent<SoundController>().SoundSE();
		GameObject.Find("FadeSystem").GetComponent<Fade>().LoadLevel(nextSceneName, 1f);
	}
}