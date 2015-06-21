using UnityEngine;
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
		CollectCubePos();
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

	// merge to child
	private void MergeBlock(GameObject block) {
		block.transform.parent = gameObject.transform;
	}

	// collect each position of block's cubes
	private void CollectCubePos() {
		Dictionary<string, float> wallPos = GetWallPosition();

		Vector3 offset = new Vector3(0, 0, 0);
		offset.x = -wallPos["x-min"];
		offset.z = -wallPos["z-min"];
		offset.y = -ground.transform.position.y;

		foreach (Transform block in transform) {
			if (block.name.CompareTo("block(land)") != 0) continue;

			print("--------");
			SetCubePos(block, offset);

			foreach (Transform cube in block) {
				SetCubePos(cube, offset);
			}
		}
	}

	// set the cube position to blockPool
	private void SetCubePos(Transform tf, Vector3 offset) {
		int x = (int)tf.transform.position.x + (int)offset.x;
		int y = (int)tf.transform.position.y + (int)offset.y;
		int z = (int)tf.transform.position.z + (int)offset.z;
		print(new Vector3(x, y, z));
		blockPool[x, y, z] = tf.gameObject;
	}

	private void RemoveCompletedRow() {

	}

	private void FullPool() {

	}

	private void NextPhase() {

	}
}
