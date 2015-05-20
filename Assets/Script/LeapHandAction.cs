using UnityEngine;
using System.Collections;
using Leap;

public class LeapHandAction : MonoBehaviour {
	Controller controller = new Controller();
	BlockController block_controller;
	private GameObject block;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
		Hand hand = hands[0];

		if (hand.Confidence < 0.2) return;

		if (!block_controller) {
			ConnectWithBlock();
		}

		// Move
		Vector handCenter = hand.PalmPosition;
		float position_scale = 1000;
		float hand_x = handCenter.x / position_scale;
		float hand_z = -handCenter.z / position_scale;
		block_controller.Move(hand_x, hand_z);
	}

	void ConnectWithBlock() {
		block = GameObject.Find("block");
		if (!block) return;
		block_controller = block.GetComponent<BlockController>();
	}
}
