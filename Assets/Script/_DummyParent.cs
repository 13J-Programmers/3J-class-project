// 
// This script controls motion of every dropping cubes in Pool, 
// after deleting completed row.
// 

using UnityEngine;
using System.Collections;

//
// This class is only used by BlockPoolController.
//
public class _DummyParent : MonoBehaviour {
	private bool _isLanded = false;
	public bool isLanded {
		get { return _isLanded; }
		// set { _isLanded = value; }
	}
	GameObject poolCubes;

	// Use this for initialization
	void Start() {
		poolCubes = GameObject.Find("BlockPool/Cubes");
	}
	
	// Update is called once per frame
	void Update() {
		if (transform.childCount == 0) {
			Setup();
		}
	}

	public void StartDropping() {
		Setup();
		_isLanded = false;
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().AddForce(Vector3.down * 100);
	}

	public void FinishDropping() {
		_isLanded = false;
		GetComponent<Rigidbody>().useGravity = false;

		// move parent
		var dropedCubes = new ArrayList();
		foreach (Transform cube in gameObject.transform) {
			dropedCubes.Add(cube);
		}
		foreach (Transform cube in dropedCubes) {
			cube.transform.parent = poolCubes.transform;
		}
	}

	// private methods ------------------------------------------------

	private void Setup() {
		transform.position = new Vector3(0, 0, 0);
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}

	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "BlockPool") {
			_isLanded = true;
		}
	}
}
