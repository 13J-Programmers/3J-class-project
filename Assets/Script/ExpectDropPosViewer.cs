using UnityEngine;
using System.Collections;

public class ExpectDropPosViewer : MonoBehaviour {
	int POOL_X;
	int POOL_Y;
	int POOL_Z;
	BlockPoolController blockPoolControl;
	bool[,,] pool;
	GameObject controllingBlock;
	GameObject showDropPosBlock;

	void Awake() {
		blockPoolControl = GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
		controllingBlock = gameObject;
	}

	// Use this for initialization
	void Start() {
		Destroy(controllingBlock.GetComponent<BlockPoolController>());
		Destroy(controllingBlock.GetComponent<ExpectDropPosViewer>());
		CloneSkeltonBlock();

		POOL_X = BlockPoolController.POOL_X;
		POOL_Y = BlockPoolController.POOL_Y;
		POOL_Z = BlockPoolController.POOL_Z;
		pool = new bool[POOL_X, POOL_Y, POOL_Z];

		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					if (blockPoolControl.blockPool[x, y, z] == null) continue;
					pool[x, y, z] = true;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	// private methods --------------------------------

	// clone block to create skelton it
	private void CloneSkeltonBlock() {
		Vector3 originBlockPos = controllingBlock.transform.position;
		Vector3 cloneBlockPos = new Vector3(
			originBlockPos.x,
			originBlockPos.y + 5,
			originBlockPos.z
		);

		showDropPosBlock = Instantiate(
			controllingBlock, 
			cloneBlockPos,
			controllingBlock.transform.rotation
		) as GameObject;
	}

	private void ShowExpectDropPos() {

	}
}
