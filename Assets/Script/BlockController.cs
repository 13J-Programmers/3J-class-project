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
}
