///
/// @file  CameraController.cs
/// @brief This script controls camera position and direction.
///

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameObject target; ///< staring target

	// Use this for initialization
	void Start() {
		target = GameObject.Find("BlockPool/Ground");
	}

	// Update is called once per frame
	void Update() {
		Vector3 poolPos = target.transform.position;
		poolPos = new Vector3(poolPos.x, poolPos.y + 6, poolPos.z);
		transform.LookAt(poolPos);
	}

	/// rotate camera on an axis of Pool center.
	/// @param direct - rotating direction: 1 or -1
	public void RotateCamera(int direct) {
		transform.RotateAround( target.transform.position, Vector3.up, direct * 45 * Time.deltaTime );
	}

	/// @param direct - rotating direction: Vector3.right or Vector3.left
	public void RotateCamera(Vector3 direct) {
		if (direct == Vector3.right) {
			RotateCamera(1);
		} else if (direct == Vector3.left) {
			RotateCamera(-1);
		}
	}
}
