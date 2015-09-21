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
		
	}
	
	// Update is called once per frame
	void Update() {
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
		Hand hand = hands[0];

		if (hand.Confidence < 0.2) return;

		if (!blockController) {
			ConnectWithBlock();
			return;
		}

		//print(isFingersFolded(hand));
	
		// Variables
		Vector handCenter = hand.PalmPosition;
		float handX = handCenter.x;
		float handZ = -handCenter.z;

		Transform camera = Camera.main.transform;

		//
		// Move Block
		//
		float speed = 0.03f; // Move speed

		// Move Block
		const int MOVING_DETECT_RANGE = 60;
		if (!isFingersFolded(hand)) {
			if (handZ > MOVING_DETECT_RANGE) {
				Vector3 forward = camera.TransformDirection(Vector3.forward) * speed;
				blockController.MoveBlock(forward);
			}
			if (handZ < MOVING_DETECT_RANGE) {
				Vector3 back = camera.TransformDirection(Vector3.back) * speed;
				blockController.MoveBlock(back);
			}
			if (handX > MOVING_DETECT_RANGE) {
				Vector3 right = camera.TransformDirection(Vector3.right) * speed;
				blockController.MoveBlock(right);
			}
			if (handX < MOVING_DETECT_RANGE) {
				Vector3 left = camera.TransformDirection(Vector3.left) * speed;
				blockController.MoveBlock(left);
			}
		}

		//
		// Rotate Block
		//
		float rotateScale = 10;
		float pitch = hand.Direction.Pitch * rotateScale;
		float yaw   = hand.Direction.Yaw   * rotateScale;
		float roll  = hand.PalmNormal.Roll * rotateScale;
		// print("pitch: " + pitch + " yaw: " + yaw + " roll: " + roll);
		float upScale = 7;
		float downScale = -upScale;
		float rightScale = 7;
		float leftScale = -rightScale;
		float counterClockwiseScale = 7;
		float clockwiseScale = -counterClockwiseScale;

		// Rotate Block
		if (!isFingersFolded(hand)) {
			// Pitch Block
			if (isRotatedX == false) {
				if (pitch > upScale) {
					//print("up");
					Vector3 back = camera.TransformDirection(Vector3.back);
					blockController.PitchBlock(back);
				} else if (pitch < downScale) {
					//print("down");
					Vector3 forward = camera.TransformDirection(Vector3.forward);
					blockController.PitchBlock(forward);
				}
				isRotatedX = true;
			}

			// Yaw Block
			if (isRotatedY == false) {
				if (yaw > rightScale) {
					//print("right");
					blockController.YawBlock(1);
				} else if (yaw < leftScale) {
					//print("left");
					blockController.YawBlock(-1);
				}
				isRotatedY = true;
			}

			// Roll Block
			if (isRotatedZ == false) {
				if (roll > counterClockwiseScale) {
					//print("counter-clockwise");
					Vector3 left = camera.TransformDirection(Vector3.left);
					blockController.RollBlock(left);
				} else if (roll < clockwiseScale) {
					//print("clockwise");
					Vector3 right = camera.TransformDirection(Vector3.right);
					blockController.RollBlock(right);
				}
				isRotatedZ = true;
			}

			// horizon hand
			if (isRotatedX == true && upScale > pitch && pitch > downScale) {
				isRotatedX = false;
			}
			if (isRotatedY == true && rightScale > yaw && yaw > leftScale) {
				isRotatedY = false;
			}
			if (isRotatedZ == true && counterClockwiseScale > roll && roll > clockwiseScale) {
				isRotatedZ = false;
			}
		}

		//
		// Drop block
		//
		float velocityY = hand.PalmVelocity.y;
		if (isFingersFolded(hand)) {
			if (velocityY < -400) {
				blockController.DropBlock();
			}
		}
	}

	private bool isFingersFolded(Hand hand) {
		Vector origin = hand.Fingers[0].TipPosition;
		float dist = 0;

		foreach (Finger finger in hand.Fingers) {
			dist += finger.TipPosition.DistanceTo(origin);
		}
		return (dist < 200) ? true : false;
	}
}
