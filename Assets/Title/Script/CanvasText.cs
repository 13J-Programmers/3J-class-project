using UnityEngine;
using System.Collections;

public class CanvasText : MonoBehaviour {
	private float h;
	private float w;
	public GameObject title;
	public GameObject comment;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		h = Screen.height;
		w = Screen.width;
		Vector3 cPos = comment.GetComponent<RectTransform> ().transform.position;
		cPos.y =h/6;
		cPos.x =w/2f;
		comment.GetComponent<RectTransform> ().transform.position= cPos;

		Vector3 tPos = title.GetComponent<RectTransform> ().transform.position;
		tPos.y =h-h/5f;
		tPos.x =w/2f;
		title.GetComponent<RectTransform> ().transform.position= tPos;
	}
}
