using UnityEngine;
using System.Collections;

public class BlockEntity : MonoBehaviour {
	// block prefabs
	const int prefabMaxNum = 18;
	int nameCount = 0;
	public GameObject[] blocks = new GameObject[prefabMaxNum];

	// Use this for initialization
	void Start() {
		// change every cube of block
		BoxCollider bc;
		Vector3 colliderSize = new Vector3(0.8f, 1f, 0.8f);
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
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("c")) {
			// In future, this method is called by other obj.
			CreateRandomBlock();
		}
	}

	public void CreateRandomBlock () {
		int randNum = Random.Range(0, prefabMaxNum);
		// create new block
		GameObject newBlock = Instantiate(
			blocks[randNum],       // instance object
			new Vector3(0, 5, 0),  // coordinate
			blocks[randNum].transform.rotation  // rotation
		) as GameObject;

		newBlock.name = "block(new)";
		newBlock.AddComponent<BlockController>();

		// connect Key and block
		GameObject target = GameObject.Find("KeyAction");
		KeyAction keyAction = target.GetComponent<KeyAction>();
		keyAction.ConnectWithBlock();
	}
}
