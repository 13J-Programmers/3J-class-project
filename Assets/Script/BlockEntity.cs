using UnityEngine;
using System.Collections;

public class BlockEntity : MonoBehaviour {
	// block prefabs
	const int prefabNum = 18;
	public GameObject[] blocks = new GameObject[prefabNum];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			CreateRandomBlock();
		}
	}

	void CreateRandomBlock () {
		System.Random rand = new System.Random();
		int randNum = rand.Next(prefabNum);
		Debug.Log(randNum);
		Instantiate(blocks[randNum]);
	}
}
