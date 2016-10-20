using UnityEngine;
using System;
//using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// シーン遷移時のフェードイン・アウトを制御するためのクラス .
public class Fade : MonoBehaviour {
	// // unused code
	// private Fade instance;
	// public Fade Instance {
	// 	get {
	// 		if (instance != null) return instance;
	// 		instance = (Fade)FindObjectOfType(typeof(Fade));
	// 		if (instance != null) return instance;
	// 		Debug.LogError(typeof(Fade) + " is nothing");
	//
	// 		return instance;
	// 	}
	// }

	public bool DebugMode = false; ///< デバッグモード
	private float fadeAlpha = 0; ///< フェード中の透明度
	private bool isFading = false; ///< フェード中かどうか
	public Color fadeColor = Color.black; ///< フェード色
	public float interval; ///< インターバル

	void Awake() {
		// // unreachable code
		// if (this != Instance) {
		// 	Destroy(this.gameObject);
		// 	return;
		// }
		DontDestroyOnLoad(this.gameObject); // シーンを切り替えても残る
	}

	void Update() {
		// When this game object come into Main scene, it will be destroyed in 1 seconds.
		if (SceneManager.GetActiveScene().name == "Main") {
			StartCoroutine(DestroyIn1sec());
		}
	}

	public void OnGUI() {
		// Fade
		if (this.isFading) {
			// 色と透明度を更新して白テクスチャを描画
			this.fadeColor.a = this.fadeAlpha;
			GUI.color = this.fadeColor;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}

		// show load level infomation
		DebugMode = false;
		if (this.DebugMode) {
			if (!this.isFading) {
				// Scene一覧を作成
				// (UnityEditor名前空間を使わないと自動取得できなかったので決めうちで作成)
				List<string> scenes = new List<string>();
				scenes.Add("Title");
				scenes.Add("Main");
				// scenes.Add("Title02");
				// Sceneが一つもない
				if (scenes.Count == 0) {
					GUI.Box(new Rect(10, 10, 200, 50), "Fade(Debug Mode)");
					GUI.Label(new Rect(20, 35, 180, 20), "Scene not found.");
					return;
				}
				GUI.Box(new Rect(0, 0, 250, 60 + scenes.Count * 25), "Fade(Debug Mode)");
				GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + SceneManager.GetActiveScene().name);
				int i = 0;
				foreach (string sceneName in scenes) {
					if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Load Level")) {
						LoadLevel(sceneName, interval);
					}
					GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
					i++;
				}
			}
		}
	}

	// 画面遷移
	// @param  scene    : シーン名
	// @param  interval : 暗転にかかる時間(秒)
	public void LoadLevel(string scene, float interval) {
		StartCoroutine(TransScene(scene, interval));
	}

	// シーン遷移用コルーチン
	// @param  scene    : シーン名
	// @param  interval : 暗転にかかる時間(秒)
	private IEnumerator TransScene(string scene, float interval) {
		// だんだん暗く .
		this.isFading = true;
		float time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		// シーン切替
		// Application.LoadLevel(scene);
		SceneManager.LoadScene(scene);

		// だんだん明るく
		time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}
		this.isFading = false;
	}

	// destroy game object in 1 seconds
	private IEnumerator DestroyIn1sec() {
		yield return new WaitForSeconds(1);
		Destroy(this.gameObject);
	}
}
