using UnityEngine;
using System.Collections;
/// <summary>
/// ジェスチャーによる操作(追加の可能性あり)
/// 1.手を握るとカメラが止まる
/// </summary>
public class HandGesture : MonoBehaviour {
	private CameraCircularMotion mainCameraMotion;
	private CameraCircularMotion secondCameraMotion;
	
	void Start() {
		mainCameraMotion = GameObject.Find("MainCamera").GetComponent<CameraCircularMotion>();
		secondCameraMotion = GameObject.Find("SecondCamera").GetComponent<CameraCircularMotion>();  
	}
	
	void Update() {
	}
	
	public void cameraStop(bool flag) {
		mainCameraMotion.enabled = flag;
		secondCameraMotion.enabled = flag;
	}
	//^ 関数名は英語なので、動詞から始まるのが一般的です。
	//  stopCamera()
	//
	//^ 関数名は「カメラを停止する」なのに、trueを引数に渡すと
	//  mainCameraMotion.enabled = true;
	//  となって「カメラを作動させる」プログラムに見えるので、
	//  関数名と実際の処理を一致させること。
	//
	//^ 2つのカメラを停止させる処理は、本当に HandGestureクラス で実装するべきなのか
	//  2てのカメラを管理する親のようなクラスを検討したほうがいいと思います。
	//
	// (mako)
}
