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
	/// LeapHandAction < PlayerAction < BaseAction < MonoBehaviour
	public class LeapHandAction : PlayerAction {
		private Hand hand;
		private Hand otherHand;

		// motion
		const int MOVING_DETECT_RANGE = 60;
		private float moveSpeed = 0.03f;

		// rotation
		private float pitch;
		private float yaw;
		private float roll;
		private float upScale = 7;
		private float downScale = -7;
		private float rightScale = 7;
		private float leftScale = -7;
		private float counterClockwiseScale = 7;
		private float clockwiseScale = -7;
		public bool isRotated = false;

		private bool isFingersFolded(Hand hand) {
			Vector origin = hand.PalmPosition;
			float dist = 0;

			// sum up fingers distance from position of palm
			foreach (Finger finger in hand.Fingers) {
				// ignore player's thumb
				if (finger.Type() == Finger.FingerType.TYPE_THUMB) continue;
				dist += finger.TipPosition.DistanceTo(origin);
			}
			return (dist < 280) ? true : false;
		}

		private bool hasTwoHands() {
			return otherHand.IsValid;
		}


		override
		protected void InitPerFrame() {
			Controller controller = new Controller();
			Frame frame = controller.Frame();
			HandList hands = frame.Hands;
			hand = hands[0];
			otherHand = hands[1];

			float rotateScale = 10;
			pitch = hand.Direction.Pitch * rotateScale;
			yaw   = hand.Direction.Yaw   * rotateScale;
			roll  = hand.PalmNormal.Roll * rotateScale;

			// horizon hand
			if (!isFingersFolded(hand) 
					&& upScale > pitch && pitch > downScale
					&& rightScale > yaw && yaw > leftScale
					&& counterClockwiseScale > roll && roll > clockwiseScale) {
				isRotated = false;
			}
		}

		override
		protected bool ValidatePerFrame() {
			return (base.GetBlockController() && hand.Confidence > 0.1);
		}

		/// move block with opened hand in x-axis
		override
		protected void DetectMotionX() {
			if (isFingersFolded(hand)) return;
			if (hasTwoHands()) return;

			float handX = hand.PalmPosition.x;

			if (handX > MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.right) * moveSpeed );
			} else if (handX < -MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.left) * moveSpeed );
			}
		}

		/// Drop Block with clenched fists
		override
		protected void DetectMotionY() {
			if (!isFingersFolded(hand)) return;

			float velocityY = hand.PalmVelocity.y;

			if (velocityY < -400) {
				GetBlockController().DropBlock();
			}
		}

		/// move block with opened hand in z-axis
		override
		protected void DetectMotionZ() {
			if (isFingersFolded(hand)) return;
			if (hasTwoHands()) return;

			float handZ = -hand.PalmPosition.z;

			if (handZ > MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.forward) * moveSpeed );
			} else if (handZ < -MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.back) * moveSpeed );
			}
		}

		/// Pitch Block
		override
		protected void DetectRotationX() {
			if (isFingersFolded(hand)) return;
			if (hasTwoHands()) return;
			if (isRotated) return;

			if (pitch > upScale) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.back) );
			} else if (pitch < downScale) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.forward) );
			} else {
				return;
			}
			isRotated = true;
		}

		/// Yaw Block
		override
		protected void DetectRotationY() {
			if (isFingersFolded(hand)) return;
			if (hasTwoHands()) return;
			if (isRotated) return;

			if (yaw > rightScale) {
				GetBlockController().YawBlock( Vector3.right );
			} else if (yaw < leftScale) {
				GetBlockController().YawBlock( Vector3.left );
			} else {
				return;
			}
			isRotated = true;
		}

		/// Roll Block
		override
		protected void DetectRotationZ() {
			if (isFingersFolded(hand)) return;
			if (hasTwoHands()) return;
			if (isRotated) return;

			if (roll > counterClockwiseScale) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.left) );
			} else if (roll < clockwiseScale) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.right) );
			} else {
				return;
			}
			isRotated = true;
		}

		// Rotate camera
		override
		protected void DetectRotationCamera() {
			if (!hasTwoHands()) return;

			Hand rightHand = (hand.IsRight) ? hand : otherHand;
			Hand leftHand  = (hand.IsRight) ? otherHand : hand;

			float rightHandZ = -rightHand.PalmPosition.z;
			float leftHandZ  = -leftHand.PalmPosition.z;

			int depth = 80;

			if (leftHandZ > depth / 2 && rightHandZ < -depth) {
				GetCameraController().RotateCam(-1);
			} else if (leftHandZ < -depth && rightHandZ > depth / 2) {
				GetCameraController().RotateCam(1);
			}
		}

		/// Press Block
		override
		protected bool DetectPressMotion() {
			if (!hasTwoHands()) return false;

			Vector3 handPos = VectorUtil.ToVector3(hand.PalmPosition);
			Vector3 otherHandPos = VectorUtil.ToVector3(otherHand.PalmPosition);
			double dist = Vector3.Distance(handPos, otherHandPos);

			if (dist > 100) return false;

			// ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			// if (destroyPositions.Count == 0) return false;

			return true;
		}

		/// Shake Block
		override
		protected bool DetectShakeMotion() {
			if (!hasTwoHands()) return false;

			float handVelocityY = hand.PalmVelocity.y;
			float otherHandVelocityY = otherHand.PalmVelocity.y;

			if ( !(handVelocityY < -300 && otherHandVelocityY < -300) ) return false;

			// ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			// if (destroyPositions.Count == 0) return false;

			// // generate splash in destroyed block positions
			// foreach (Vector3 destroyPosition in destroyPositions) {
			// 	GetBlockController().GenerateSplash(destroyPosition);
			// }

			return true;
		}
	}
}
