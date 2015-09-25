/// 
/// @file  ExpectDropPosViewer.cs
/// @brief This script displays block expected dropping position.
/// 

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ExpectDropPosViewer : MonoBehaviour {
	private BlockPoolController blockPoolController;
	private GameObject controllingBlock;
	private GameObject showDropPosBlock;
	private GameObject ground;
	/// is syncing with the controlling block?
	private bool isSync = true;

	/// This function is always called before 
	/// any Start functions and also just after a prefab is instantiated.
	void Awake() {
		blockPoolController = GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
		ground = GameObject.Find("BlockPool/Ground");
		controllingBlock = gameObject;
	}

	// Use this for initialization
	void Start() {
		CloneSkeltonBlock();
		BlockController.StartFalling += new EventHandler(StopSync);
		BlockController.StopFalling += new EventHandler(StopShowing);
	}
	
	// Update is called once per frame
	void Update() {
		SyncOriginBlock();
	}

	public void StopSync(object sender, EventArgs e) {
		isSync = false;
	}

	/// destory expected drop pos
	public void StopShowing(object sender, EventArgs e) {
		Destroy(showDropPosBlock);
	}

	// private methods --------------------------------

	/// round x,z coordinate
	private Vector3 roundXZ(Vector3 vector) {
		Vector3 _vector;
		_vector.x = (float)Math.Round(vector.x);
		_vector.y = vector.y;
		_vector.z = (float)Math.Round(vector.z);
		return _vector;
	}

	/// decide position of showDropPosBlock
	/// * @param position - original block position
	/// * @return expected dropping position
	private Vector3 ExpectDropPos(Vector3 position) {
		// set expected x,z //
		Vector3 correctedBlockPos = roundXZ(position);

		// set expected y //
		// get offsets for search in array
		float halfOfWidth = 0.5f;
		Wall wall = blockPoolController.GetWall();
		Vector3 offset = new Vector3(0, 0, 0);
		offset.x = -wall.GetMinX() - halfOfWidth;
		offset.z = -wall.GetMinZ() - halfOfWidth;
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
				} else if (blockPoolController.GetSizeY() <= cubePosY) {
					continue;
				} else if (!(0 <= cubePosX && cubePosX < blockPoolController.GetSizeX() 
						&& 0 <= cubePosZ && cubePosZ < blockPoolController.GetSizeZ())) {
					continue;
				} else if (blockPoolController.GetPool().GetGameObject(cubePosX, cubePosY, cubePosZ) != null) {
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

	/// clone block to create skelton it
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


