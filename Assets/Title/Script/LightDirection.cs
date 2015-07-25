using UnityEngine;
using System.Collections;

public class LightDirection : MonoBehaviour {
	public GameObject target;
	private GameObject Oya;

	// Use this for initialization
	void Start() {
		Oya = gameObject.transform.parent.gameObject; // 親オブジェクトの取得
	}
	
	// Update is called once per frame
	void Update() {
		Vector3 pos = Oya.transform.position; // 親オブジェクトの座標
		Vector3 LightPos = target.transform.position; // targetの座標
		transform.LookAt(LightPos); // 常にtargetへ向く
		this.transform.position = pos; // 座標の更新
	}
}
