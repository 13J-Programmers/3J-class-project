using UnityEngine;
using System.Collections;

public class BlockEntity : MonoBehaviour {
	// block prefabs
	const int prefabMaxNum = 18;
	int nameCount = 0;
	public GameObject[] blocks = new GameObject[prefabMaxNum];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			// In future, this method is called by other obj.
			CreateRandomBlock();
		}
	}

	void CreateRandomBlock () {
		int randNum = Random.Range(0, prefabMaxNum);
		GameObject createBlock = Instantiate(
			blocks[randNum],       // instance object
			new Vector3(0, 5, 0),  // coordinate
			blocks[randNum].transform.rotation  // rotation
		) as GameObject;

		createBlock.name = "block";
	}
}
