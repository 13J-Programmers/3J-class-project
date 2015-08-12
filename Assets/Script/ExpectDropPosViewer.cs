// 
// This script displays block expected dropping position.
// 

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ExpectDropPosViewer : MonoBehaviour {
	int POOL_X;
	int POOL_Y;
	int POOL_Z;
	BlockPoolController blockPoolControl;
	GameObject controllingBlock;
	GameObject showDropPosBlock;
	GameObject ground;
	bool isSync = true;

	void Awake() {
		blockPoolControl = GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
		ground = GameObject.Find("BlockPool/Ground");
		controllingBlock = gameObject;
	}

	// Use this for initialization
	void Start() {
		CloneSkeltonBlock();
	}
	
	// Update is called once per frame
	void Update() {
		SyncOriginBlock();
	}

	public void StopSync() {
		isSync = false;
	}

	public void StopShowing() {
		Destroy(showDropPosBlock);
	}

	// private methods --------------------------------

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
		// set expected x,z //
		Vector3 correctedBlockPos = roundXZ(position);

		// set expected y //
		// get offsets for search in array
		float halfOfWidth = 0.5f;
		Dictionary<string, float> wallPos = blockPoolControl.GetWallPosition();
		Vector3 offset = new Vector3(0, 0, 0);
		offset.x = -wallPos["x-min"] - halfOfWidth;
		offset.z = -wallPos["z-min"] - halfOfWidth;
		offset.y = -ground.transform.position.y - halfOfWidth;

		// set y of controllingBlock
		int maxPosY = (int)Math.Round(controllingBlock.transform.position.y);
		Vector3 maxHeight = new Vector3(0, maxPosY, 0);

		var blockCubesPos = new ArrayList();
		blockCubesPos.Add(roundXZ(controllingBlock.transform.position) - maxHeight);
		foreach (Transform cube in controllingBlock.transform) {
			blockCubesPos.Add(roundXZ(cube.gameObject.transform.position) - maxHeight);
		}

		// set expected y
		int height;
		for (height = maxPosY; height >= 0; height--) {
			bool isCube = false;

			foreach (Vector3 cubePos in blockCubesPos) {
				int cubePosX = (int)Math.Round(cubePos.x + offset.x);
				int cubePosY = (int)Math.Round(cubePos.y) + height;
				int cubePosZ = (int)Math.Round(cubePos.z + offset.z);

				if (cubePosY < 0) {
					isCube = true;
				} else if (BlockPoolController.POOL_Y <= cubePosY) {
					continue;
				} else if (!(0 <= cubePosX && cubePosX < BlockPoolController.POOL_X 
						&& 0 <= cubePosZ && cubePosZ < BlockPoolController.POOL_Z)) {
					continue;
				} else if (blockPoolControl.blockPool[cubePosX, cubePosY, cubePosZ] != null) {
					isCube = true;
				}
				// print(
				// 	new Vector3(
				// 		Math.Round(cubePos.x + offset.x), 
				// 		cubePosY, 
				// 		Math.Round(cubePos.z + offset.z)
				// 	) + " : " + isCube
				// );
				if (isCube) break;
			}

			if (isCube) {
				break;
			} else {
				continue;
			}
		}
		height++;
		correctedBlockPos.y = (float)(height - offset.y);

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


