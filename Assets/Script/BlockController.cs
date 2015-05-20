using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {
	// Use this for initialization
	void Start () {
		transform.localPosition = new Vector3(0, 5, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// if the gameObject is out of camera range, destroy it.
	void OnBecameInvisible(){
    	Destroy(gameObject);
    }

	public void Move(float x, float z) {
		Vector3 v = new Vector3(x, 0, z);
		transform.Translate(v, Space.World);
	}
}
