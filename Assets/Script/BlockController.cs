using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GetComponent<Collider>().isTrigger = true;

		transform.localPosition = new Vector3(0, 5, 0);
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (state.CompareTo("falling") == 0) {
			transform.Translate(0, -0.1f, 0);
		}
		*/
	}

	private void OnTriggerEnter(Collider other){
		
	}

	// if the gameObject is out of camera range, destroy it.
	void OnBecameInvisible(){
    	Destroy(gameObject);
    }
}
