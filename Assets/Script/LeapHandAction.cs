/// 
/// @file  LeapHandAction.cs
/// @brief 
///   This script will turn hand motion into game obj motion.
///   Implementer have to consider operability.
/// 

using UnityEngine;
using System;
using System.Collections;
using Leap;

public class LeapHandAction : UserAction {
	Controller controller = new Controller();
	private bool isRotatedX = false;
	private bool isRotatedY = false;
	private bool isRotatedZ = false;

	// Use this for initialization
	void Start() {
		if (controller.IsConnected) {
			print("INFO : Succeeded to connect with Leap");
		} else {
			print("WARN : Failed to connect with Leap");
		}
	}
	
	// Update is called once per frame
	void Update() {
		Frame frame = controller.Frame();
		//HandList hands = frame.Hands;
		//Hand hand = hands[0];
		Hand hand = frame.Hands.Rightmost;
		Vector position = hand.PalmPosition;
		//Vector velocity = hand.PalmVelocity;
		//Vector direction = hand.Direction;
		Vector handAngle = new Vector( ///< the inclination of the hand
			hand.Direction.Pitch, 
			hand.Direction.Yaw, 
			hand.PalmNormal.Roll
		);

		if (hand.Confidence < 0.2) return;

		//print(handAngle);

		/*
		string fingers_pos = "";
		foreach (Finger finger in hand.Fingers) {
			fingers_pos += finger.TipPosition + " ";
		}
		print(fingers_pos);
		*/

		// Move
		float game_scale = 0.01f;
		float handX =  position.x * game_scale;
		float handZ = -position.z * game_scale;
		//blockController.MoveBlock(handX, handZ);

		Transform camera = Camera.main.transform;

		// Move speed
		float speed = 0.08f;

		// Detectable range from +point+ to infinity
		const float point = 1f;

		// Move Block
		print("x : " + handX + ";  z : " + handZ);
		if (handZ > point) {
			Vector3 forward = camera.TransformDirection(Vector3.forward) * speed;
			blockController.MoveBlock(forward);
		}
		if (handZ < -point) {
			Vector3 back = camera.TransformDirection(Vector3.back) * speed;
			blockController.MoveBlock(back);
		}
		if (handX > point) {
			Vector3 right = camera.TransformDirection(Vector3.right) * speed;
			blockController.MoveBlock(right);
		}
		if (handX < -point) {
			Vector3 left = camera.TransformDirection(Vector3.left) * speed;
			blockController.MoveBlock(left);
		}


		// define non rotatable angle range
		const float upRange = 0.6f;
		const float downRange = -0.6f;
		const float rightRange = 0.5f;
		const float leftRange = -0.5f;
		const float clockwiseRange = -0.6f;
		const float counterClockwiseRange = 0.6f;

		// Pitch Block
		if (handAngle.x < downRange && isRotatedX == false) {
			Vector3 forward = camera.TransformDirection(Vector3.forward);
			blockController.PitchBlock(forward);
			isRotatedX = true;
		} else if (handAngle.x > upRange && isRotatedX == false) {
			Vector3 back = camera.TransformDirection(Vector3.back);
			blockController.PitchBlock(back);
			isRotatedX = true;
		}

		// Yaw Block
		if (handAngle.y < leftRange && isRotatedY == false) {
			blockController.YawBlock(-1);
			isRotatedY = true;
		} else if (handAngle.y > rightRange && isRotatedY == false) {
			blockController.YawBlock(1);
			isRotatedY = true;
		}

		// Roll Block
		if (handAngle.z < clockwiseRange && isRotatedZ == false) {
			Vector3 right = camera.TransformDirection(Vector3.right);
			blockController.RollBlock(right);
			isRotatedZ = true;
		} else if (handAngle.z > counterClockwiseRange && isRotatedZ == false) {
			Vector3 left = camera.TransformDirection(Vector3.left);
			blockController.RollBlock(left);
			isRotatedZ = true;
		}

		if (IsInside(handAngle.x, downRange, upRange)) {
			isRotatedX = false;
		}
		if (IsInside(handAngle.y, leftRange, rightRange)) {
			isRotatedY = false;
		}
		if (IsInside(handAngle.z, clockwiseRange, counterClockwiseRange)) {
			isRotatedZ = false;
		}
	}

	private bool RotateWith(Vector3 vector) {
		return true;
	}

	private bool IsInside(float var, float min, float max) {
		return min < var && var < max;
	}
}






