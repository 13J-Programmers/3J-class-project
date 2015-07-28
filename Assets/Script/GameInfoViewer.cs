using UnityEngine;
using System.Collections;

public class GameInfoViewer : MonoBehaviour {
	GameManager gameManager;
	BlockEntity blockEntity;
	GUIStyle guiStyle;
	GUIStyleState guiStyleState;

	// Use this for initialization
	void Start() {
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update() {
		guiStyle = new GUIStyle();
		guiStyle.fontSize = 16;
		guiStyleState = new GUIStyleState();
		guiStyleState.textColor = Color.white;
		guiStyle.normal = guiStyleState;
	}

	// show following item
	//   * lines
	//   * score
	//   * game time
	private void OnGUI() {
		GUI.Label(new Rect(10, Screen.height - 60, 100, 60), "Lines : " + gameManager.lines, guiStyle);
		GUI.Label(new Rect(10, Screen.height - 40, 100, 60), "Score : " + gameManager.score, guiStyle);
		int timeLeft = (int)gameManager.remainingTime;
		GUI.Label(new Rect(10, Screen.height - 20, 100, 60), "Time Left : " + timeLeft, guiStyle);
	}

	public void ShowNextBlock(int nextNum) {
		Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
		GameObject next = Instantiate(
			blockEntity.blocks[nextNum],  //Next Block
			pos,        //Pos
			blockEntity.blocks[nextNum].transform.rotation
		) as  GameObject;
		next.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
	}
}
