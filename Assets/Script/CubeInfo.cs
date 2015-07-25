using UnityEngine;
using System.Collections;

public class CubeInfo : MonoBehaviour {
	private int _score = 0;
	public int Score {
		get {
			return _score;
		}
		set {
			if (value >= 0) return;
			_score = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
