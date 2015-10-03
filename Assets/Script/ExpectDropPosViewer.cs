///
/// @file  ExpectDropPosViewer.cs
/// @brief This script displays block expected dropping position.
///

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ExpectDropPosViewer : MonoBehaviour {
	/// gameobject to show expected drop position
	private GameObject showDropPosBlock;
	/// this has syncing status with the controlling block
	private bool isSync = true;

	private BlockPoolController GetBlockPoolController() {
		return GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
	}

	private GameObject GetControllingBlockObj() {
		return gameObject;
	}
	
	private GameObject GetBlockPoolAt(int x, int y, int z) {
		return GetBlockPoolController().GetPool().GetGameObject(x, y, z);
	}

	// Use this for initialization
	void Start() {
		CloneSkeltonBlock();
		BlockController.StartFalling += new EventHandler(StopSync);
		BlockController.StopFalling  += new EventHandler(StopShowing);
	}

	// Update is called once per frame
	void Update() {
		SyncOriginBlock();
	}

	// private methods --------------------------------

	private void StopSync(object sender, EventArgs e) {
		isSync = false;
	}

	private void StopShowing(object sender, EventArgs e) {
		Destroy(showDropPosBlock);
		Destroy(this);
	}

	/// decide position of showDropPosBlock
	/// * @param position - original block position
	/// * @return expected dropping position
	private Vector3 ExpectDropPos(Vector3 position) {
		// set expected x,z //
		Vector3 correctedBlockPos = VectorUtil.RoundXZ(position);

		// set expected y //
		// get offsets for search in array
		float halfOfWidth = 0.5f;
		Wall wall = GetBlockPoolController().GetWall();
		Vector3 offset = new Vector3(0, 0, 0);
		offset.x = -wall.GetMinX() - halfOfWidth;
		offset.z = -wall.GetMinZ() - halfOfWidth;
		offset.y = -GameObject.Find("BlockPool/Ground").transform.position.y - halfOfWidth;

		// set y of GetControllingBlockObj()
		int maxPosY = (int)Math.Round(GetControllingBlockObj().transform.position.y);
		Vector3 maxHeight = new Vector3(0, maxPosY, 0);

		var blockCubesPos = new ArrayList();
		blockCubesPos.Add(
			VectorUtil.RoundXZ(GetControllingBlockObj().transform.position) - maxHeight
		);
		foreach (Transform cube in GetControllingBlockObj().transform) {
			blockCubesPos.Add(
				VectorUtil.RoundXZ(cube.gameObject.transform.position) - maxHeight
			);
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
					break;
				}

				if (GetBlockPoolController().GetSizeY() <= cubePosY) {
					continue;
				}

				if (GetBlockPoolAt(cubePosX, cubePosY, cubePosZ) != null) {
					isCube = true;
					break;
				}
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
		Vector3 originBlockPos = GetControllingBlockObj().transform.position;
		Vector3 cloneBlockPos = ExpectDropPos(originBlockPos);

		// instantiate
		showDropPosBlock = Instantiate(
			GetControllingBlockObj(),
			cloneBlockPos,
			GetControllingBlockObj().transform.rotation
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

		Vector3 originBlockPos = GetControllingBlockObj().transform.position;
		Vector3 cloneBlockPos = ExpectDropPos(originBlockPos);

		if (!showDropPosBlock) return;
		showDropPosBlock.transform.localPosition = cloneBlockPos;
		showDropPosBlock.transform.rotation = GetControllingBlockObj().transform.rotation;
	}

	public void DestroyChildBlocks() {
		foreach (Transform child in showDropPosBlock.transform) {
			Destroy(child.gameObject);
		}
	}
}
