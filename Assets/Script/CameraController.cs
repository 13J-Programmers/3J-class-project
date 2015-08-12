// 
// This script controls variable camera position and direction.
// 

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	GameObject target; // staring target

	// Use this for initialization
	void Start() {
		target = GameObject.Find("BlockPool/Ground");
	}
	
	// Update is called once per frame
	void Update() {
		Vector3 poolPos = target.transform.position;
		poolPos = new Vector3(poolPos.x, poolPos.y + 8, poolPos.z);
		transform.LookAt(poolPos);
	}

	public void RotateCam(int direct) {
		transform.RotateAround( target.transform.position, Vector3.up, direct * 45 * Time.deltaTime );
	}
}
