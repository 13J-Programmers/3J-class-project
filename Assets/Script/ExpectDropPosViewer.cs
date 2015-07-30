using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ExpectDropPosViewer : MonoBehaviour {
	int POOL_X;
	int POOL_Y;
	int POOL_Z;
	BlockController blockController;
	BlockPoolController blockPoolControl;
	bool[,,] pool;
	GameObject controllingBlock;
	GameObject showDropPosBlock;
	GameObject ground;
	bool isSync = true;

	void Awake() {
		blockController = gameObject.GetComponent<BlockController>();
		blockPoolControl = GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
		ground = GameObject.Find("BlockPool/Ground");
		controllingBlock = gameObject;
	}

	// Use this for initialization
	void Start() {
		POOL_X = BlockPoolController.POOL_X;
		POOL_Y = BlockPoolController.POOL_Y;
		POOL_Z = BlockPoolController.POOL_Z;
		pool = new bool[POOL_X, POOL_Y, POOL_Z];

		SetBlockPoolCube();
		CloneSkeltonBlock();
	}
	
	// Update is called once per frame
	void Update() {
		SetBlockPoolCube();
		SyncOriginBlock();

		// for debug
		if (Input.GetKeyDown("p")) {
			for (int z = 0; z < POOL_Z; z++) {
				for (int y = 0; y < POOL_Y; y++) {
					string str = "";
					for (int x = 0; x < POOL_X; x++) {
						str += (pool[x, y, z] == false) ? "_": "o";
					}
					print("(z, y) = (" + z + ", " + y + ") : " + str);
				}
			}
		}
	}

	public void StopSync() {
		isSync = false;
	}

	public void StopShowing() {
		Destroy(showDropPosBlock);
	}

	// private methods --------------------------------

	private void SetBlockPoolCube() {
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					if (blockPoolControl.blockPool[x, y, z] == null) continue;
					pool[x, y, z] = true;
				}
			}
		}
	}

	// round x,z coordinate
	private Vector3 roundXZ(Vector3 vector) {
		Vector3 _vector;
		_vector.x = (float)Math.Round(vector.x);
		_vector.y = vector.y;
		_vector.z = (float)Math.Round(vector.z);
		return _vector;
	}

	// decide position of showDropPosBlock
	private Vector3 ExpectDropPos(Vector3 position) {
		Vector3 correctedBlockPos = roundXZ(position);

		float halfOfWidth = 0.5f;
		Dictionary<string, float> wallPos = blockPoolControl.GetWallPosition();
		Vector3 offset = new Vector3(0, 0, 0);
		offset.x = -wallPos["x-min"] - halfOfWidth;
		offset.z = -wallPos["z-min"] - halfOfWidth;
		offset.y = -ground.transform.position.y - halfOfWidth;

		Vector3 height = new Vector3(0, 10f, 0);
		var blockCubesPos = new ArrayList();
		blockCubesPos.Add(roundXZ(controllingBlock.transform.position) - height);
		foreach (Transform cube in controllingBlock.transform) {
			blockCubesPos.Add(roundXZ(cube.gameObject.transform.position) - height);
		}

		int maxPosY = (int)controllingBlock.transform.position.y;
		int minPosY = 0;
		correctedBlockPos.y = maxPosY;

		int i = 0;

		// need some time to consider
		/* 
		for (i = 0; i < maxPosY; i++) {
			foreach (Vector3 pos in blockCubesPos) {
				bool isCube = false;

				//print(pos);
				print(pos.y + i);
				print(new Vector3((int)(pos.x + offset.x), (int)(i - 1), (int)(pos.z + offset.z)));
				if (pos.y + i <= 0) {
					isCube = true;
				} else if (BlockPoolController.POOL_Y < pos.y + i) {
					isCube = false;
				} else {
					isCube = pool[
						(int)(pos.x + offset.x), 
						(int)(pos.y + i - 1), 
						(int)(pos.z + offset.z)
					];
				}
				
				if (!isCube) {
					//print(i + 1);
					goto LOOP_END;
				}
			}
		}
		LOOP_END: ;
		*/

		correctedBlockPos.y = (float)(i - offset.y);

		/*
		foreach (Vector3 pos in blockCubesPos) {
			print(pos);
		}
		print("-----");
		*/
		
		return correctedBlockPos;
	}

	// clone block to create skelton it
	private void CloneSkeltonBlock() {
		Vector3 originBlockPos = controllingBlock.transform.position;
		Vector3 cloneBlockPos = ExpectDropPos(originBlockPos);

		// instantiate
		showDropPosBlock = Instantiate(
			controllingBlock, 
			cloneBlockPos,
			controllingBlock.transform.rotation
		) as GameObject;
		showDropPosBlock.name = "block(expected-drop-pos)";

		// delete components
		Destroy(showDropPosBlock.GetComponent<Rigidbody>());
		Destroy(showDropPosBlock.GetComponent<BoxCollider>());
		foreach (Transform cube in showDropPosBlock.transform) {
			Destroy(cube.gameObject.GetComponent<BoxCollider>());
		}
		Destroy(showDropPosBlock.GetComponent<BlockController>());
		Destroy(showDropPosBlock.GetComponent<ExpectDropPosViewer>());

		// set skelton cubes
		Color alpha = new Color(1f, 1f, 1f, 0.5f);
		Material material = showDropPosBlock.gameObject.GetComponent<Renderer>().material;
		StandardShader.SetBlendMode(material, BlendMode.Fade);
		material.color = alpha;
		foreach (Transform cube in showDropPosBlock.transform) {
			Material childMaterial = cube.gameObject.GetComponent<Renderer>().material;
			StandardShader.SetBlendMode(childMaterial, BlendMode.Fade);
			childMaterial.color = alpha;
		}
	}

	private void SyncOriginBlock() {
		if (!GameObject.Find("block(expected-drop-pos)")) return;
		if (!isSync) return;

		Vector3 originBlockPos = controllingBlock.transform.position;
		Vector3 cloneBlockPos = ExpectDropPos(originBlockPos);

		if (!showDropPosBlock) return;
		showDropPosBlock.transform.localPosition = cloneBlockPos;
		showDropPosBlock.transform.rotation = controllingBlock.transform.rotation;
	}
}


