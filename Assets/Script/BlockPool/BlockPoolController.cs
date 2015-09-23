/// 
/// @file   BlockPoolController.cs
/// @brief 
///   This script controls Pool which is stored cubes position.
///   control means merge new block in Pool and delete completed rows.
/// 

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

///
/// Behavior about BlockPoolController class
/// 
// Script Lifecycle Flowchart
// 
//       |call
//       ∨
//     BlockPoolController
//       ControlBlock()  << -----+
//       | MergeBlock()          | update()
//       | SearchCubePos()       |   if (_DummyParent.isLanded) 
//       | RemoveCompletedRow()  |     dummyParent.FinishDropping()
//       |                       |     recall
//       ∨                       |
//    
//     _DummyParent --------> isLanded = true
//       StartDropping()
/// 
/// After remove completed row, we need to wait cubes dropping.
/// then recall ControlBlock()
/// 
public class BlockPoolController : MonoBehaviour {
	private BlockPool blockPool = new BlockPool(5, 10, 5);
	private GameObject ground, poolCubes;
	private _DummyParent dummyParent;
	private GameManager gameManager;

	public int GetSizeX() { return blockPool.POOL_X; }
	public int GetSizeY() { return blockPool.POOL_Y; }
	public int GetSizeZ() { return blockPool.POOL_Z; }
	public BlockPool GetPool() { return blockPool; }

	void Awake() {
		ground = GameObject.Find("BlockPool/Ground");
		poolCubes = GameObject.Find("BlockPool/Cubes");
		dummyParent = GameObject.Find("_DummyParent").GetComponent<_DummyParent>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Start() {
		int size = blockPool.POOL_X;
		// set position of BlockPool obj and scale
		if (size == 5) {
			// size : 5
			// position : (0, -2.5, 0)
			// scale : (0.5, 1.0, 0.5)
			transform.Translate(new Vector3(0f, -2.5f, 0f), Space.World);
			transform.localScale = new Vector3(0.5f, 1.0f, 0.5f);
		} else if (size == 6) {
			// size : 6
			// position : (0.5, -2.5, 0.5)
			// scale : (0.6, 1.0, 0.6)
			transform.Translate(new Vector3(0.5f, -2.5f, 0.5f), Space.World);
			transform.localScale = new Vector3(0.6f, 1.0f, 0.6f);
		} else {
			throw new Exception("POOL_X is expected to be 5 or 6. Instead of " + blockPool.POOL_X);
		}
	}
	
	void Update() {
		if (dummyParent.isLanded) {
			dummyParent.FinishDropping();
			ControlBlock(null);
		}

		// for debug
		/*
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
		*/
	}

	/// main process in Pool
	public void ControlBlock(GameObject block) {
		InitPool();
		MergeBlock(block);
		SearchCubePos();
		FixCubePos();
		RemoveCompletedRow();
	}

	/// return the 4 walls position
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
		return wallPosition;
	}

	// private methods ------------------------------

	/// init the Pool
	private void InitPool() {
		blockPool.Init();
	}

	/// merge block cubes in BlockPool/Cubes
	/// 
	///     before:
	///    
	///       ├── BlockPool
	///       │   ├── Ground
	///       │   ├── Wall
	///       │   └── Cubes
	///       └── block(dropping)
	///           ├── Cube
	///           ├── Cube
	///           └── Cube
	///       
	///     after:
	///    
	///       └── BlockPool
	///           ├── Ground
	///           ├── Wall
	///           └── Cubes
	///               ├── Cube
	///               ├── Cube
	///               └── Cube
	/// 
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

	/// collect each position of block's cubes
	/// blockPool[,,] has each cubes position
	/// @see SetCubePos()
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

	/// set the cube position to Pool
	private void SetCubePos(Transform obj, Vector3 offset) {
		float halfOfWidth = 0.5f;
		int x = (int)Mathf.Round(obj.position.x + offset.x - halfOfWidth);
		int y = (int)Mathf.Round(obj.position.y + offset.y - halfOfWidth);
		int z = (int)Mathf.Round(obj.position.z + offset.z - halfOfWidth);
		// print("offset: " + offset);
		// print("target: " + obj.transform.position);
		// print("index : >> " + new Vector3(x, y, z));
		try {
			blockPool.SetGameObject(x, y, z, obj.gameObject);
		} catch (IndexOutOfRangeException) {
			gameManager.GameOver();
		}
	}

	/// sometimes, cube is put unexpected position.
	/// therefore, cubes need to correct y position.
	private void FixCubePos() {
		for (int z = 0; z < blockPool.POOL_Z; z++) {
			for (int y = 0; y < blockPool.POOL_Y; y++) {
				for (int x = 0; x < blockPool.POOL_X; x++) {
					if (blockPool.GetGameObject(x, y, z) == null) continue;
					Vector3 currentPos = blockPool.GetGameObject(x, y, z).transform.position;
					blockPool.GetGameObject(x, y, z).transform.position = new Vector3(
						currentPos.x,
						(float)Math.Round(currentPos.y),
						currentPos.z
					);
				}
			}
		}
	}

	private void RemoveCompletedRow() {
		blockPool.RemoveCompletedRow();
	}
}











