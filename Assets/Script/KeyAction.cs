using UnityEngine;
using System.Collections;

public class KeyAction : MonoBehaviour {
	// A container for control target
	GameObject target;
	BlockController control;
	CameraController cameraController;
	public GameObject[] blocks = new GameObject[1];
	
	// Use this for initialization
	void Start() {
		cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update() {
		// Pitch Block
		if (Input.GetKeyDown("w")) {
			Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
			control.PitchBlock(right);
		}
		else if (Input.GetKeyDown("s")) {
			Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
			control.PitchBlock(-1 * right);
		}

		// Yaw Block
		else if (Input.GetKeyDown("e")) {
			control.YawBlock(1);
		}
		else if (Input.GetKeyDown("q")) {
			control.YawBlock(-1);
		}

		// Roll Block
		else if (Input.GetKeyDown("d")) {
			Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
			control.RollBlock(forward);
		}
		else if (Input.GetKeyDown("a")) {
			Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
			control.RollBlock(-1 * forward);
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
			Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward) * speed;
			control.MoveBlock(forward);
		}
		else if (Input.GetKey("down")) {
			Vector3 back = Camera.main.transform.TransformDirection(Vector3.back) * speed;
			control.MoveBlock(back);
		}
		else if (Input.GetKey("right")) {
			Vector3 right = Camera.main.transform.TransformDirection(Vector3.right) * speed;
			control.MoveBlock(right);
		}
		else if (Input.GetKey("left")) {
			Vector3 left = Camera.main.transform.TransformDirection(Vector3.left) * speed;
			control.MoveBlock(left);
		}

		// Drop Block 
		if (Input.GetKeyDown("space")) {
			control.DropBlock();
		}
	}

	public void ConnectWithBlock() {
		target = GameObject.Find("block(new)");
		if (!target) return;
		control = target.GetComponent<BlockController>();
	}

	public void DisconnectWithBlock() {
		target = GameObject.Find("_DummyBlock");
		if (!target) return;
		control = target.GetComponent<BlockController>();
	}
}
