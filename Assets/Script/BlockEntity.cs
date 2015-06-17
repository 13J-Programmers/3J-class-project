using UnityEngine;
using System.Collections;

public class BlockEntity : MonoBehaviour {
	// block prefabs
	const int prefabMaxNum = 18;
	int nameCount = 0;
	public GameObject[] blocks = new GameObject[prefabMaxNum];

	// Use this for initialization
	void Start() {

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

		// connect Key and block
		GameObject target = GameObject.Find("KeyAction");
		KeyAction keyAction = target.GetComponent<KeyAction>();
		keyAction.ConnectWithBlock();
	}
}
