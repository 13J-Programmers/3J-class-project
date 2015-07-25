using UnityEngine;
using System;
//using System.Linq;
using System.Collections;
using System.Collections.Generic;

// シーン遷移時のフェードイン・アウトを制御するためのクラス .
public class Fade : MonoBehaviour {
	private Fade instance;
	public Fade Instance {
		get {
			// if (instance == null) {
			// 	instance = (Fade)FindObjectOfType(typeof(Fade));
			// 	
			// 	if (instance == null) {
			// 		Debug.LogError(typeof(Fade) + " is nothing");
			// 	}
			// }
			if (instance != null) return instance;
			instance = (Fade)FindObjectOfType(typeof(Fade));
			if (instance != null) return instance;
			Debug.LogError(typeof(Fade) + " is nothing");
			
			return instance;
		}
	}
	// デバッグモード .
	public bool DebugMode = true;
	// フェード中の透明度<
	private float fadeAlpha = 0;
	// フェード中かどうか
	private bool isFading = false;
	// フェード色
	public Color fadeColor = Color.black;
	// インターバル
	public float interval;

	public void Awake() {
		if (this != Instance) {
			Destroy(this.gameObject);
			return;
		}
		DontDestroyOnLoad(this.gameObject); // シーンを切り替えても残る
	}

	public void OnGUI() {
		// Fade .
		if (this.isFading) {
			//色と透明度を更新して白テクスチャを描画 .
			this.fadeColor.a = this.fadeAlpha;
			GUI.color = this.fadeColor;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}
		if (this.DebugMode) {
			if (!this.isFading) {
				// Scene一覧を作成 .
				// (UnityEditor名前空間を使わないと自動取得できなかったので決めうちで作成) .
				List<string> scenes = new List<string>();
				scenes.Add("Title");
				scenes.Add("Test_forTeX2e");
				// scenes.Add("Title02");
				// Sceneが一つもない .
				if (scenes.Count == 0) {
					GUI.Box(new Rect(10, 10, 200, 50), "Fade(Debug Mode)");
					GUI.Label(new Rect(20, 35, 180, 20), "Scene not found.");
					return;
				}
				GUI.Box(new Rect(0, 0, 250, 60 + scenes.Count * 25), "Fade(Debug Mode)");
				GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + Application.loadedLevelName);
				int i = 0;
				foreach (string sceneName in scenes) {
					if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Load Level")) {
						LoadLevel (sceneName,interval);
					}
					GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
					i++;
				}
			}
		}
	}

	// 画面遷移 .
	// 引数
	//   scene : シーン名
	//   interval : 暗転にかかる時間(秒)
	public void LoadLevel(string scene, float interval) {
		StartCoroutine(TransScene(scene, interval));
	}

	// シーン遷移用コルーチン .
	// 引数
	//   scene : シーン名
	//   interval : 暗転にかかる時間(秒)
	private IEnumerator TransScene(string scene, float interval) {
		// だんだん暗く .
		this.isFading = true;
		float time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);      
			time += Time.deltaTime;
			yield return 0;
		}
		
		// シーン切替 .
		Application.LoadLevel(scene);
		
		// だんだん明るく .
		time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}
		this.isFading = false;
	}
}

