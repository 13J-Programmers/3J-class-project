/// 
/// @file  BlockEntity.cs
/// @brief This script is for create block object instance.
/// 

using UnityEngine;
using System;
using System.Collections;

public class BlockEntity : MonoBehaviour {
	public const int prefabMaxNum = 18;
	/// block prefabs
	public GameObject[] blocks = new GameObject[prefabMaxNum];
	private Queue queue = new Queue();

	public int GetPrefabMaxNum() { return prefabMaxNum; }

	/// send notification when new block is created
	public static event EventHandler CreateNewBlock;

	/// BlockEntity methods are invoked from Start() in GameManager.
	/// therefore, initializing variables have to write in Awake().
	void Awake() {
		PushNextBlock(RandomBlock());
	}

	void Start() {
		GameManager.StartGame       += new EventHandler(CreateRandomBlock);
		BlockController.StopFalling += new EventHandler(CreateRandomBlock);

		// change every cube of block
		BoxCollider bc;
		Vector3 colliderSize = new Vector3(0.95f, 0.95f, 0.95f);
		for (int i = 0; i < prefabMaxNum; i++) {
			bc = blocks[i].GetComponent<BoxCollider>();
			bc.size = colliderSize;
			foreach (Transform cube in blocks[i].transform) {
				bc = cube.gameObject.GetComponent<BoxCollider>();
				bc.size = colliderSize;
			}
		}
	}

	private void CreateRandomBlock(object sender, EventArgs e) {
		if (GameObject.Find("block(new)")) return;

		// shift new block list
		GameObject randBlock = ShiftNextBlock();
		PushNextBlock(RandomBlock());

		// create new block
		GameObject newBlock = Instantiate(
			randBlock, // instance object
			new Vector3(0, 10, 0), // coordinate
			randBlock.transform.rotation // rotation
		) as GameObject;

		newBlock.name = "block(new)";
		newBlock.AddComponent<BlockController>();
		newBlock.AddComponent<ExpectDropPosViewer>();
		SetScoreToCube(newBlock);

		// notification of new block creation
		if (CreateNewBlock != null) {
			CreateNewBlock(this, EventArgs.Empty);
		}
	}

	// private methods ------------------------------

	private GameObject RandomBlock() {
		int randNum = UnityEngine.Random.Range(0, this.GetPrefabMaxNum());
		return blocks[randNum];
	}

	private void PushNextBlock(GameObject gameObject) {
		queue.Enqueue(gameObject);
	}
	private GameObject ShiftNextBlock() {
		return (GameObject)queue.Dequeue();
	}

	/// set score to menber of CubeInfo component
	private void SetScoreToCube(GameObject newBlock) {
		CubeInfo cubeInfo;
		cubeInfo = newBlock.AddComponent<CubeInfo>();
		cubeInfo.score = 20;
		foreach (Transform t in newBlock.transform) {
			cubeInfo = t.gameObject.AddComponent<CubeInfo>();
			cubeInfo.score = 20;
		}
	}
}
