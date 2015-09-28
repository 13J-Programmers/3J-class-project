using UnityEngine;
using System;
using System.Collections;

public class NextBlockEntity : MonoBehaviour {
	private GameObject nextBlockObj;

	private GameObject GetNextBlockObj() {
		return GameObject.Find("BlockEntity").GetComponent<BlockEntity>().PeekNextBlock();
	}

	void Start() {
		BlockEntity.CreateNewBlock  += new EventHandler(InstantiateBlock);
		BlockController.StopFalling += new EventHandler(DestoryBlock);
	}

	private void InstantiateBlock(object sender, EventArgs e) {
		InstantiateBlock(GetNextBlockObj());
	}

	private void InstantiateBlock(GameObject block) {
		nextBlockObj = Instantiate(
			block, // instance object
			new Vector3(0, 100, 0), // coordinate
			block.transform.rotation // rotation
		) as GameObject;

		nextBlockObj.name = "nextBlock";
	}

	private void DestoryBlock(object sender, EventArgs e) {
		Destroy(nextBlockObj);
	}
}
