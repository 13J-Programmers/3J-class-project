/// 
/// @file   KeyAction.cs
/// @brief  This script will turn pressing key into game obj motion.
/// 

using UnityEngine;
using System.Collections;

// KeyAction extend UserAction
public class KeyAction : UserAction {
	/// Use this for initialization
	void Start() {
		cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}
	
	/// Update is called once per frame
	void Update() {
		Transform camera = Camera.main.transform;

		if (blockController == null) return;

		// Pitch Block
		if (Input.GetKeyDown("w")) {
			Vector3 forward = camera.TransformDirection(Vector3.forward);
			blockController.PitchBlock(forward);
		}
		else if (Input.GetKeyDown("s")) {
			Vector3 back = camera.TransformDirection(Vector3.back);
			blockController.PitchBlock(back);
		}
		
		// Yaw Block
		else if (Input.GetKeyDown("e")) {
			blockController.YawBlock(1);
		}
		else if (Input.GetKeyDown("q")) {
			blockController.YawBlock(-1);
		}

		// Roll Block
		else if (Input.GetKeyDown("d")) {
			Vector3 right = camera.TransformDirection(Vector3.right);
			blockController.RollBlock(right);
		}
		else if (Input.GetKeyDown("a")) {
			Vector3 left = camera.TransformDirection(Vector3.left);
			blockController.RollBlock(left);
		}

		// Rotate Camera 
		if (Input.GetKey("return")) {
			cameraController.RotateCam(1);
		}
		else if (Input.GetKey("delete")) {
			cameraController.RotateCam(-1);
		}

		// Move speed
		float speed = 0.1f;
		
		// Move Block
		if (Input.GetKey("up")) {
			Vector3 forward = camera.TransformDirection(Vector3.forward) * speed;
			blockController.MoveBlock(forward);
		}
		else if (Input.GetKey("down")) {
			Vector3 back = camera.TransformDirection(Vector3.back) * speed;
			blockController.MoveBlock(back);
		}
		else if (Input.GetKey("right")) {
			Vector3 right = camera.TransformDirection(Vector3.right) * speed;
			blockController.MoveBlock(right);
		}
		else if (Input.GetKey("left")) {
			Vector3 left = camera.TransformDirection(Vector3.left) * speed;
			blockController.MoveBlock(left);
		}

		// Drop Block 
		if (Input.GetKeyDown("space")) {
			blockController.DropBlock();
		}
	}
}
