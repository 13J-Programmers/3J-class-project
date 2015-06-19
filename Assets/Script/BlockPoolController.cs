using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockPoolController : MonoBehaviour {
	// const int POOL_X = 6;      // width
	// const int POOL_Y = 10;     // height
	// const int POOL_Z = POOL_X; // depth
	// int[,,] blockPool = new int[POOL_X, POOL_Y, POOL_Z]; 

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void ControlBlock(GameObject block) {
		block.name = "block(land)";
		block.tag = "BlockPool";
		MergeBlock(block);
	}

	// return the 4 walls position
	public Dictionary<string, float> GetWallPosition() {
		Dictionary<string, float> wallPosition = new Dictionary<string, float>();

		GameObject wallXMin = GameObject.Find("BlockPool/Wall(x-min)");
		GameObject wallXMax = GameObject.Find("BlockPool/Wall(x-max)");
		GameObject wallZMin = GameObject.Find("BlockPool/Wall(z-min)");
		GameObject wallZMax = GameObject.Find("BlockPool/Wall(z-max)");
		wallPosition["x-min"] = wallXMin.transform.position.x;
		wallPosition["x-max"] = wallXMax.transform.position.x;
		wallPosition["z-min"] = wallZMin.transform.position.z;
		wallPosition["z-max"] = wallZMax.transform.position.z;
		// print("x-min : " + wallPosition["x-min"]);
		// print("x-max : " + wallPosition["x-max"]);
		// print("z-min : " + wallPosition["z-min"]);
		// print("z-max : " + wallPosition["z-max"]);
		return wallPosition;
	}

	// private methods ------------------------------

	// merge to child
	private void MergeBlock(GameObject block) {
		block.transform.parent = gameObject.transform;
	}

	private void RemoveCompletedRow() {

	}

	private void FullPool() {

	}

	private void NextPhase() {

	}
}
