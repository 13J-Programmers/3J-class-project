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

namespace Player.Action {
	public class LeapHandAction : PlayerAction {
		// leap motion
		private Controller controller = new Controller();
		private Frame frame;
		private HandList hands;
		private Hand hand;
		private Hand otherHand;

		private Transform mainCamera;

		// motion
		const int MOVING_DETECT_RANGE = 60;
		private float moveSpeed = 0.03f;

		// rotation
		private float upScale = 7;
		private float downScale = -7;
		private float rightScale = 7;
		private float leftScale = -7;
		private float counterClockwiseScale = 7;
		private float clockwiseScale = -7;
		public bool isRotatedX = false;
		public bool isRotatedY = false;
		public bool isRotatedZ = false;
		private float rotateScale = 10;
		private float pitch;// = hand.Direction.Pitch * rotateScale;
		private float yaw;//   = hand.Direction.Yaw   * rotateScale;
		private float roll;//  = hand.PalmNormal.Roll * rotateScale;

		// Use this for initialization
		void Start() {
			cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
		}

		private bool isFingersFolded(Hand hand) {
			Vector origin = hand.Fingers[0].TipPosition;
			float dist = 0;

			foreach (Finger finger in hand.Fingers) {
				dist += finger.TipPosition.DistanceTo(origin);
			}
			return (dist < 200) ? true : false;
		}

		override
		protected void InitPerFrame() {
			mainCamera = Camera.main.transform;
			//print(mainCamera.TransformDirection(Vector3.forward));

			frame = controller.Frame();
			hands = frame.Hands;
			hand = hands[0];

			pitch = hand.Direction.Pitch * rotateScale;
			yaw   = hand.Direction.Yaw   * rotateScale;
			roll  = hand.PalmNormal.Roll * rotateScale;

			// horizon hand
			if (!isFingersFolded(hand) && upScale > pitch && pitch > downScale) {
				isRotatedX = false;
			}
			if (!isFingersFolded(hand) && rightScale > yaw && yaw > leftScale) {
				isRotatedY = false;
			}
			if (!isFingersFolded(hand) && counterClockwiseScale > roll && roll > clockwiseScale) {
				isRotatedZ = false;
			}
		}

		override
		protected bool ValidatePerFrame() {
			return base.blockController && hand.Confidence > 0.2;
		}

		// move block with opened hand in x-axis
		override
		protected void DetectMotionX() {
			float handX = hand.PalmPosition.x;

			if (isFingersFolded(hand)) return;
			if (handX > MOVING_DETECT_RANGE) {
				Vector3 right = mainCamera.TransformDirection(Vector3.right) * moveSpeed;
				blockController.MoveBlock(right);
			} else if (handX < MOVING_DETECT_RANGE) {
				Vector3 left = mainCamera.TransformDirection(Vector3.left) * moveSpeed;
				blockController.MoveBlock(left);
			}
		}

		// Drop Block with clenched fists
		override
		protected void DetectMotionY() {
			float velocityY = hand.PalmVelocity.y;
			if (isFingersFolded(hand)) {
				if (velocityY < -400) {
					blockController.DropBlock();
				}
			}
		}

		// move block with opened hand in z-axis
		override
		protected void DetectMotionZ() {
			float handZ = -hand.PalmPosition.z;

			if (isFingersFolded(hand)) return;
			if (handZ > MOVING_DETECT_RANGE) {
				Vector3 forward = mainCamera.TransformDirection(Vector3.forward) * moveSpeed;
				blockController.MoveBlock(forward);
			} else if (handZ < MOVING_DETECT_RANGE) {
				Vector3 back = mainCamera.TransformDirection(Vector3.back) * moveSpeed;
				blockController.MoveBlock(back);
			}
		}

		// Pitch Block
		override
		protected void DetectRotationX() {
			if (isFingersFolded(hand)) return;
			if (isRotatedX) return;
			if (pitch > upScale) {
				//print("up");
				Vector3 back = mainCamera.TransformDirection(Vector3.back);
				blockController.PitchBlock(back);
			} else if (pitch < downScale) {
				//print("down");
				Vector3 forward = mainCamera.TransformDirection(Vector3.forward);
				blockController.PitchBlock(forward);
			} else {
				return;
			}
			isRotatedX = true;
		}

		// Yaw Block
		override
		protected void DetectRotationY() {
			if (isFingersFolded(hand)) return;
			if (isRotatedY) return;
			if (yaw > rightScale) {
				//print("right");
				blockController.YawBlock(1);
			} else if (yaw < leftScale) {
				//print("left");
				blockController.YawBlock(-1);
			} else {
				return;
			}
			isRotatedY = true;
		}

		// Roll Block
		override
		protected void DetectRotationZ() {
			if (isFingersFolded(hand)) return;
			if (isRotatedZ) return;
			if (roll > counterClockwiseScale) {
				//print("counter-clockwise");
				Vector3 left = mainCamera.TransformDirection(Vector3.left);
				blockController.RollBlock(left);
			} else if (roll < clockwiseScale) {
				//print("clockwise");
				Vector3 right = mainCamera.TransformDirection(Vector3.right);
				blockController.RollBlock(right);
			} else {
				return;
			}
			isRotatedZ = true;	
		}

		override
		protected void DetectRotationCamera() {

		}

	}
}


