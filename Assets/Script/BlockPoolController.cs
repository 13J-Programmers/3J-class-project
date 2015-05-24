using UnityEngine;
using System.Collections;

public class BlockPoolController : MonoBehaviour {
	const int POOL_X = 6;      // width
	const int POOL_Y = 10;     // height
	const int POOL_Z = POOL_X; // depth
	int[,,] blockPool = new int[POOL_X, POOL_Y, POOL_Z]; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ControlBlock() {

	}

	// private methods ------------------------------

	private void MergeBlock() {

	}

	private void RemoveCompletedRow() {

	}

	private void FullPool() {

	}

	private void NextPhase() {

	}
}
