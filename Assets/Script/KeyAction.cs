using UnityEngine;
using System.Collections;

public class KeyAction : MonoBehaviour {
	// A container for control target
	GameObject target;
	BlockController control;
	public GameObject[] blocks = new GameObject[1];
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!control) {
			// In future, this method is called by other obj.
			ConnectWithBlock();
		}

		// Pitch Block
		if (Input.GetKeyDown("w")) {
			control.PitchBlock(1);
		}
		else if (Input.GetKeyDown("s")) {
			control.PitchBlock(-1);
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
			control.RollBlock(1);
		}
		else if (Input.GetKeyDown("a")) {
			control.RollBlock(-1);
		}

		// Move Block
		float x = 0.0f;
		float z = 0.0f;
		if (Input.GetKey("up")) {
			z = 0.1f;
			control.MoveBlock(x,z);
		}
		else if (Input.GetKey("down")) {
			z = -0.1f;
			control.MoveBlock(x,z);
		}
		else if (Input.GetKey("right")) {
			x = 0.1f;
			control.MoveBlock(x,z);
		}
		else if (Input.GetKey("left")) {
			x = -0.1f;
			control.MoveBlock(x,z);
		}
	}

	public void ConnectWithBlock() {
		target = GameObject.Find("block");
		control = target.GetComponent<BlockController>();
	}

	public void DisconnectWithBlock() {
		target = GameObject.Find("_DummyBlock");
		control = target.GetComponent<BlockController>();
	}
}
