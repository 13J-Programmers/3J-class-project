using UnityEngine;
using System.Collections;

public class ViewPointChange : MonoBehaviour {
	public GameObject target;		// staring target
	
	// Use this for initialization
	void Start() {
		target = GameObject.Find("BlockPool/Plane");
	}
	
	// Update is called once per frame
	void Update() {
		Vector3 poolPos = target.transform.position;
		poolPos = new Vector3(poolPos.x, poolPos.y + 5, poolPos.z);
		transform.LookAt(poolPos);

		if (Input.GetKey("return")) // rotate anticlockwise
		transform.RotateAround( target.transform.position, Vector3.up, 45 * Time.deltaTime );

		if (Input.GetKey("delete")) // rotate clockwise
		transform.RotateAround( target.transform.position, Vector3.up, -45 * Time.deltaTime );
	}

}
