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
			Vector3.right * 5;

		// position
		this.gameObject.transform.position = 
			GetMainCameraObj().transform.position + 
			Vector3.up * 98 + 
			this.gameObject.transform.forward * 9;
	}
}
