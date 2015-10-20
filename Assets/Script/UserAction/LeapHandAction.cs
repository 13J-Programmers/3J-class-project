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
		private LeapHands leapHands;

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

		void Awake() {
			leapHands = GameObject.Find("LeapHands").GetComponent<LeapHands>();
		}

		override
		protected void InitPerFrame() {
			float rotateScale = 10;
			pitch = leapHands.hand.Direction.Pitch * rotateScale;
			yaw   = leapHands.hand.Direction.Yaw   * rotateScale;
			roll  = leapHands.hand.PalmNormal.Roll * rotateScale;

			// horizon hand
			if (!LeapHands.IsFingersFolded(leapHands.hand) 
					&& upScale > pitch && pitch > downScale
					&& rightScale > yaw && yaw > leftScale
					&& counterClockwiseScale > roll && roll > clockwiseScale) {
				isRotated = false;
			}
		}

		override
		protected bool ValidatePerFrame() {
			return (base.GetBlockController() && leapHands.hand.Confidence > 0.1);
		}

		/// move block with opened hand in x-axis
		override
		protected void DetectMotionX() {
			if (LeapHands.IsFingersFolded(leapHands.hand)) return;
			if (leapHands.HasTwoHands()) return;

			float handX = leapHands.hand.PalmPosition.x;

			if (handX > MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.right) * moveSpeed );
			} else if (handX < -MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.left) * moveSpeed );
			}
		}

		/// Drop Block with clenched fists
		override
		protected void DetectMotionY() {
			if (!LeapHands.IsFingersFolded(leapHands.hand)) return;

			float velocityY = leapHands.hand.PalmVelocity.y;

			if (velocityY < -400) {
				GetBlockController().DropBlock();
			}
		}

		/// move block with opened hand in z-axis
		override
		protected void DetectMotionZ() {
			if (LeapHands.IsFingersFolded(leapHands.hand)) return;
			if (leapHands.HasTwoHands()) return;

			float handZ = -leapHands.hand.PalmPosition.z;

			if (handZ > MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.forward) * moveSpeed );
			} else if (handZ < -MOVING_DETECT_RANGE) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.back) * moveSpeed );
			}
		}

		/// Pitch Block
		override
		protected void DetectRotationX() {
			if (LeapHands.IsFingersFolded(leapHands.hand)) return;
			if (leapHands.HasTwoHands()) return;
			if (isRotated) return;

			if (pitch < downScale) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.forward) );
			} else if (pitch > upScale) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.back) );
			} else {
				return;
			}
			isRotated = true;
		}

		/// Yaw Block
		override
		protected void DetectRotationY() {
			if (LeapHands.IsFingersFolded(leapHands.hand)) return;
			if (leapHands.HasTwoHands()) return;
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
			if (LeapHands.IsFingersFolded(leapHands.hand)) return;
			if (leapHands.HasTwoHands()) return;
			if (isRotated) return;

			if (roll < clockwiseScale) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.right) );
			} else if (roll > counterClockwiseScale) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.left) );
			} else {
				return;
			}
			isRotated = true;
		}

		// Rotate camera
		override
		protected void DetectRotationCamera() {
			if (!leapHands.HasTwoHands()) return;

			Hand rightHand = (leapHands.hand.IsRight) ? leapHands.hand : leapHands.otherHand;
			Hand leftHand  = (leapHands.hand.IsRight) ? leapHands.otherHand : leapHands.hand;

			float rightHandZ = -rightHand.PalmPosition.z;
			float leftHandZ  = -leftHand.PalmPosition.z;

			int depth = 80;

			if (leftHandZ < -depth && rightHandZ > depth / 2) {
				GetCameraController().RotateCamera( Vector3.right );
			} else if (leftHandZ > depth / 2 && rightHandZ < -depth) {
				GetCameraController().RotateCamera( Vector3.left );
			}
		}

		/// Press Block
		override
		protected bool DetectPressMotion() {
			if (!leapHands.HasTwoHands()) return false;

			Vector3 handPos = VectorUtil.ToVector3(leapHands.hand.PalmPosition);
			Vector3 otherHandPos = VectorUtil.ToVector3(leapHands.otherHand.PalmPosition);
			double dist = Vector3.Distance(handPos, otherHandPos);

			if (dist > 100) return false;

			// ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			// if (destroyPositions.Count == 0) return false;

			return true;
		}

		/// Shake Block
		override
		protected bool DetectShakeMotion() {
			if (!leapHands.HasTwoHands()) return false;

			float handVelocityY = leapHands.hand.PalmVelocity.y;
			float otherHandVelocityY = leapHands.otherHand.PalmVelocity.y;

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
