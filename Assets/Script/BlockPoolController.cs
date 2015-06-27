using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BlockPoolController : MonoBehaviour {
	const int POOL_X = 6;      // width
	const int POOL_Y = 10;     // height
	const int POOL_Z = POOL_X; // depth
	GameObject[,,] blockPool = new GameObject[POOL_X, POOL_Y, POOL_Z];
	GameObject ground;

	// Use this for initialization
	void Start() {
		ground = GameObject.Find("BlockPool/Ground");
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void ControlBlock(GameObject block) {
		block.name = "block(land)";
		block.tag = "BlockPool";
		MergeBlock(block);

		// remove completed row
		SearchCubePos();
		if (RemoveCompletedRow()) {
			InitPool();
			// wait
			// SearchCubePos();
		}
		
		print("done");
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
		// print("Wall x min-max : " + wallPos["x-min"] + "..." + wallPos["x-max"]);
		// print("Wall z min-max : " + wallPos["z-min"] + "..." + wallPos["z-max"]);
		return wallPosition;
	}

	// private methods ------------------------------

	// init the pool block
	private void InitPool() {
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_X; y++) {
				for (int x = 0; x < POOL_X; x++) {
					blockPool[x, y, z] = null;
				}
			}
		}
	}

	// merge to child
	private void MergeBlock(GameObject block) {
		block.transform.parent = gameObject.transform;
	}

	// collect each position of block's cubes
	private void SearchCubePos() {
		Dictionary<string, float> wallPos = GetWallPosition();

		Vector3 offset = new Vector3(0, 0, 0);
		offset.x = -wallPos["x-min"];
		offset.z = -wallPos["z-min"];
		offset.y = -ground.transform.position.y;

		foreach (Transform block in transform) {
			if (block.name.CompareTo("block(land)") != 0) continue;

			SetCubePos(block, offset);

			foreach (Transform cube in block) {
				SetCubePos(cube, offset);
			}
		}
	}

	// set the cube position to blockPool
	private void SetCubePos(Transform tf, Vector3 offset) {
		float halfOfWidth = 0.5f;
		int x = (int)(Mathf.Round(tf.transform.position.x + offset.x - halfOfWidth));
		int y = (int)(Mathf.Round(tf.transform.position.y + offset.y - halfOfWidth));
		int z = (int)(Mathf.Round(tf.transform.position.z + offset.z - halfOfWidth));
		// print("offset: " + offset);
		// print("target: " + tf.transform.position);
		// print("index : >> " + new Vector3(x, y, z));
		blockPool[x, y, z] = tf.gameObject;
	}

	private bool RemoveCompletedRow() {
		bool isRemoved = false;

		// check completed row
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_X; y++) {
				bool isCompleted = true;
				// string row = "";
				for (int x = 0; x < POOL_X; x++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
					// row += (blockPool[x, y, z] == null) ? "_ " : "o ";
				}
				// print("(y:" + y + ", z:" + z + ")>>" + row);

				if (!isCompleted) continue;

				// remove competed row
				isRemoved = true;
				for (int x = 0; x < POOL_X; x++) {
					print("Destroy : " + blockPool[x, y, z]);
					Destroy(blockPool[x, y, z]);
				}
			}
		}

		if (isRemoved) return true;

		// check completed col
		for (int x = 0; x < POOL_Z; x++) {
			for (int y = 0; y < POOL_X; y++) {
				bool isCompleted = true;
				// string row = "";
				for (int z = 0; z < POOL_X; z++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
					// row += (blockPool[x, y, z] == null) ? "_ " : "o ";
				}
				// print("(y:" + y + ", z:" + z + ")>>" + row);

				if (!isCompleted) continue;

				// remove competed row
				isRemoved = true;
				for (int z = 0; z < POOL_X; z++) {
					print("Destroy : " + blockPool[x, y, z]);
					Destroy(blockPool[x, y, z]);
				}
			}
		}

		return isRemoved;
	}

	private void FullPool() {

	}

	private void NextPhase() {

	}
}










