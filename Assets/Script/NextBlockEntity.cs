using UnityEngine;
using System;
using System.Collections;

public class NextBlockEntity : MonoBehaviour {
	void Start() {
		BlockEntity.CreateNewBlock  += new EventHandler(ShowBlock);
		BlockController.StopFalling += new EventHandler(DestoryBlock);
	}

	private GameObject GetNextBlockObj() {
		return GameObject.Find("BlockEntity").GetComponent<BlockEntity>().PeekNextBlock();
	}

	public void ShowBlock(object sender, EventArgs e) {
		InstantiateBlock(GetNextBlockObj());
	}

	private void InstantiateBlock(GameObject block) {
		GameObject nextBlockObj = Instantiate(
			block, // instance object
			new Vector3(0, 100, 0), // coordinate
			block.transform.rotation // rotation
		) as GameObject;

		nextBlockObj.name = "nextBlock";
	}

	public void DestoryBlock(object sender, EventArgs e) {
		DestoryBlock();
	}

	public void DestoryBlock() {
		if (GameObject.Find("nextBlock")) {
			Destroy(GameObject.Find("nextBlock").gameObject);
		} else {
			print("when destroy nextBlock, it is not found.");
		}
	}
}





