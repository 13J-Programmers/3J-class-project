using UnityEngine;
using System.Collections;

public class CameraCircularMotion  : MonoBehaviour {
	public GameObject target;
	public float rad; // 半径
	public float hight; // 高さ
	public float angle; // ラジアン値
	public float speed;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		float d = Time.deltaTime;
		Vector3 CameraPos = target.transform.position;
		// オブジェクトの周りをカメラが円運動する
		transform.position = new Vector3(
			CameraPos.x + Mathf.Cos(angle) * rad, 
			CameraPos.y + hight, 
			CameraPos.z + Mathf.Sin(angle) * rad
		);
		angle += speed * d;
		//print("MainCamera x:" + this.transform.position.x);
		//print("MainCamera y:" + this.transform.position.y);
		//print("MainCamera z:" + this.transform.position.z);
		transform.LookAt(CameraPos); // カメラをtargetへ向かせる
	}
}
