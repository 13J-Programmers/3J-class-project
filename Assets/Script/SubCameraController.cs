using UnityEngine;
using System.Collections;

public class SubCameraController : MonoBehaviour {
	private GameObject GetMainCameraObj() {
		return GameObject.Find("Main Camera");
	}
	
	void Update() {
		// angle
		this.gameObject.transform.localEulerAngles = 
			GetMainCameraObj().transform.eulerAngles + 
			Vector3.right * -10;

		// position
		this.gameObject.transform.position = 
			GetMainCameraObj().transform.position + 
			Vector3.up * 93 + 
			this.gameObject.transform.forward * 7;
	}
}
