﻿using UnityEngine;
using System.Collections;

public class BlockEntity : MonoBehaviour {
	// block prefabs
	public const int prefabMaxNum = 18;
	public GameObject[] blocks = new GameObject[prefabMaxNum];
	// public int randNum = Random.Range(0, prefabMaxNum);
	KeyAction keyAction;
	GameInfoViewer gameInfoViewer;

	void Awake() {
		keyAction = GameObject.Find("KeyAction").GetComponent<KeyAction>();
		gameInfoViewer = GameObject.Find("GameInfoViewer").GetComponent<GameInfoViewer>();

	}

	// Use this for initialization
	void Start() {
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
	
	// Update is called once per frame
	void Update() {
		
	}

	public void CreateRandomBlock() {
		// int nextNum = Random.Range(0, prefabMaxNum);
		int randNum = Random.Range(0, prefabMaxNum);

		// create new block
		GameObject newBlock = Instantiate(
			blocks[randNum],       // instance object
			new Vector3(0, 10, 0),  // coordinate
			blocks[randNum].transform.rotation  // rotation
		) as GameObject;

		newBlock.name = "block(new)";
		newBlock.AddComponent<BlockController>();
		SetScoreToCube(newBlock);

		// connect Key and block
		keyAction.ConnectWithBlock();
		// randNum = nextNum;
	}

	// private methods ------------------------------

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
