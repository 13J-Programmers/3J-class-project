/// 
/// @file  UserAction.cs
/// @brief Action classes inherit this base class to detect user inputs
/// 

using UnityEngine;
using System.Collections;

public class UserAction : MonoBehaviour {
	/// A container for control target
	protected GameObject target;
	protected BlockController blockController;
	protected CameraController cameraController;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// get component of the new block for comment
	public void ConnectWithBlock() {
		target = GameObject.Find("block(new)");
		if (!target) return;
		blockController = target.GetComponent<BlockController>();
	}

	/// get component of the _DummyBlock for disconnect
	public void DisconnectWithBlock() {
		target = GameObject.Find("_DummyBlock");
		if (!target) return;
		blockController = target.GetComponent<BlockController>();
	}
}
