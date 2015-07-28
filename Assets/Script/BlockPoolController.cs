using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Behavior about BlockPoolController class
// 
//   |call
//   ∨
// BlockPoolController
//   ControlBlock() <--------+
//   | MergeBlock()          | update()
//   | SearchCubePos()       |   if (_DummyParent.isLanded) 
//   | RemoveCompletedRow()  |     recall
//   |                       |     
//   ∨                       |
//
// _DummyParent --------> isLanded = true
//   StartDropping()
// 
// 
// After remove completed row, we need to wait cubes dropping.
// then recall ControlBlock()
// 
public class BlockPoolController : MonoBehaviour {
	public const int POOL_X = 5;      // width
	public const int POOL_Y = 10;     // height
	public const int POOL_Z = POOL_X; // depth
	GameObject[,,] blockPool = new GameObject[POOL_X, POOL_Y, POOL_Z];
	GameObject ground, poolCubes;
	_DummyParent dummyParent;
	GameManager gameManager;

	// Use this for initialization
	void Awake() {
		ground = GameObject.Find("BlockPool/Ground");
		poolCubes = GameObject.Find("BlockPool/Cubes");
		dummyParent = GameObject.Find("_DummyParent").GetComponent<_DummyParent>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Start() {
		// BlockPool Transform
		//
		// size : 5
		// position : (-0.5, -2, -0.5)
		// scale : (0.5, 1.0, 0.5)
		// 
		// size : 6
		// position : (0.5, -2.5, 0.5)
		// scale : (0.6, 1.0, 0.6)
		//
		switch (POOL_X) {
			case 5: 
				transform.Translate(new Vector3(-0.5f, -2f, -0.5f), Space.World);
				transform.localScale = new Vector3(0.5f, 1.0f, 0.5f);
				break;
			case 6:
				transform.Translate(new Vector3(0.5f, -2.5f, 0.5f), Space.World);
				transform.localScale = new Vector3(0.6f, 1.0f, 0.6f);
				break;
			default:
				throw new Exception("Pool width is expected to be 5 or 6.");
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (dummyParent.isLanded) {
			dummyParent.FinishDropping();
			ControlBlock(null);
		}

		// for debug
		if (Input.GetKeyDown("p")) {
			for (int z = 0; z < POOL_Z; z++) {
				for (int y = 0; y < POOL_Y; y++) {
					string str = "";
					for (int x = 0; x < POOL_X; x++) {
						str += (blockPool[x, y, z] == null) ? "_": "o";
					}
					print("(z, y) = (" + z + ", " + y + ") : " + str);
				}
			}
		}
	}

	public void ControlBlock(GameObject block) {
		InitPool();
		MergeBlock(block);
		SearchCubePos();
		FixCubePos();
		RemoveCompletedRow();
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
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					blockPool[x, y, z] = null;
				}
			}
		}
	}

	// merge block cubes in BlockPool/Cubes
	private void MergeBlock(GameObject block) {
		if (block == null) return;
		// move block cubes into poolCubes
		block.tag = "BlockPool";
		block.name = "Cube";
		block.transform.parent = poolCubes.transform;
		// remove rigitbody
		Destroy(block.GetComponent<Rigidbody>());

		var blockCubes = new ArrayList();
		foreach (Transform cube in block.transform) {
			blockCubes.Add(cube);
		}
		foreach (Transform cube in blockCubes) {
			cube.tag = "BlockPool";
			cube.transform.parent = poolCubes.transform;
		}
	}

	// collect each position of block's cubes
	private void SearchCubePos() {
		Dictionary<string, float> wallPos = GetWallPosition();

		Vector3 offset = new Vector3(0, 0, 0);
		offset.x = -wallPos["x-min"];
		offset.z = -wallPos["z-min"];
		offset.y = -ground.transform.position.y;

		foreach (Transform cube in poolCubes.transform) {
			SetCubePos(cube, offset);
		}
	}

	// set the cube position to blockPool
	private void SetCubePos(Transform obj, Vector3 offset) {
		float halfOfWidth = 0.5f;
		int x = (int)Mathf.Round(obj.position.x + offset.x - halfOfWidth);
		int y = (int)Mathf.Round(obj.position.y + offset.y - halfOfWidth);
		int z = (int)Mathf.Round(obj.position.z + offset.z - halfOfWidth);
		// print("offset: " + offset);
		// print("target: " + obj.transform.position);
		// print("index : >> " + new Vector3(x, y, z));
		try {
			blockPool[x, y, z] = obj.gameObject;
		} catch (IndexOutOfRangeException) {
			// TODO: call GameOver()
			print("GameOver");
		}
	}

	private void FixCubePos() {
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					if (blockPool[x, y, z] == null) continue;
					Vector3 currentPos = blockPool[x, y, z].transform.position;
					blockPool[x, y, z].transform.position = new Vector3(
						currentPos.x,
						(float)Math.Round(currentPos.y),
						currentPos.z
					);
				}
			}
		}
	}

	private bool RemoveCompletedRow() {
		// Flow:
		//   1. Marking cubes of every completed row. (A)
		//   2. Marking cubes which above them. (B)
		//   3. Cubes (B) belong to a Dummy parent
		//   4. Add RigitBody to the Dummy parent
		//   5. Remove (A)
		//   6. Rid RigitBody from the Dummy parent
		//   7. After cubes (B) landed, 
		//      cubes (B) independent of the Dummy parent.
		//   8. jump to first.

		bool hasCompletedRow = false;
		int removeRowNum = 0;
		bool[,,] willRemoveCube = new bool[POOL_X, POOL_Y, POOL_Z];
		bool[,,] onRemoveCube   = new bool[POOL_X, POOL_Y, POOL_Z];

		// check completed row
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				bool isCompleted = true;
				for (int x = 0; x < POOL_X; x++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
				}
				if (!isCompleted) continue;

				hasCompletedRow = true;
				removeRowNum++;

				// check completed row
				for (int x = 0; x < POOL_X; x++) {
					willRemoveCube[x, y, z] = true;
				}
			}
		}
		for (int x = 0; x < POOL_X; x++) {
			for (int y = 0; y < POOL_Y; y++) {
				bool isCompleted = true;
				for (int z = 0; z < POOL_Z; z++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
				}
				if (!isCompleted) continue;

				hasCompletedRow = true;
				removeRowNum++;

				// check completed row
				for (int z = 0; z < POOL_Z; z++) {
					willRemoveCube[x, y, z] = true;
				}
			}
		}
		
		if (!hasCompletedRow) return false;

		// add num of rows to display
		gameManager.lines += removeRowNum;

		// mark cubes above completed row
		for (int z = 0; z < POOL_Z; z++) {
			for (int x = 0; x < POOL_X; x++) {
				bool hasRemoveCube = false;
				for (int y = 0; y < POOL_Y; y++) {
					if (blockPool[x, y, z] == null) continue;

					if (willRemoveCube[x, y, z] == true) {
						hasRemoveCube = true;
						continue;
					}

					if (hasRemoveCube) {
						onRemoveCube[x, y, z] = true;
						blockPool[x, y, z].transform.parent = dummyParent.transform;
					}
				}
			}
		}

		// destroy completed row
		int cubeScore = 0;
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					if (willRemoveCube[x, y, z] == true) {
						cubeScore += blockPool[x, y, z].GetComponent<CubeInfo>().score;
						Destroy(blockPool[x, y, z]);
					}
				}
			}
		}

		// add game-score to display
		gameManager.score += cubeScore * removeRowNum;

		// start to drop dummyParent
		dummyParent.StartDropping();

		return true;
	}
}










