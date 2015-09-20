using UnityEngine;
using System.Collections;

public class CreateCube : MonoBehaviour {
	private int rand;
	private int i;
	private float time;
	private GameObject obj;
	public float interval = 3.0f;
	public GameObject[] cube = new GameObject[18];
	public Material material;

	// Use this for initialization
	void Start() {
		i = -1;
		time = 4;
	}
	
	// Update is called once per frame
	void Update() {
		//print(time + "秒経過");
		time += Time.deltaTime;
		if (time > interval) {
			create(cube);
			time = 0;
		}

	}
	
	public void create(GameObject[] cube) {
		while (true) {
			rand = Random.Range(0, 18); // 0~17のランダム生成
			if (rand != i) {
				i = rand;
				break;
			}
		}
		//print("生成キューブ:" + i);
		Destroy(obj);
		obj = (GameObject)Instantiate(cube[i], this.transform.localPosition, Quaternion.identity);
		int c = 0;
		foreach (Transform child in obj.transform) { // objの子オブジェクトのタグ変更
            //child.GetComponent<MeshRenderer>().material = material;
			child.tag = "Trigger";
			c++;
		}
		//obj.GetComponent<MeshRenderer>().material = material;
		obj.tag = "Trigger";
	}
}