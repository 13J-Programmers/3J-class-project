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
	/// LeapHandAction < PlayerAction < MonoBehaviour
	public class LeapHandAction : PlayerAction {
		private LeapHands leapHands;

		// motion
		private float moveSpeed = 0.1f;

		// rotation
		public bool isRotated = false;

		void Awake() {
			leapHands = GameObject.Find("LeapHands").GetComponent<LeapHands>();
		}

		override
		protected void InitPerFrame() {
			if (leapHands == null || leapHands.hand == null) return;

			// horizontal hand
			if (LeapHands.IsHorizontal(leapHands.hand)) {
				isRotated = false;
			}
		}

		override
		protected bool ValidatePerFrame() {
			return (base.GetBlockController() && leapHands.hand.Confidence > 0.1);
		}

		override
		protected void DetectMotion() {
			if (leapHands.HasTwoHands()) return;
			if (LeapHands.IsGrabbing(leapHands.hand)) return;

			// int MOVING_DETECT_RANGE = 60;
			// // move block with opened hand in x-axis
			// float handX = leapHands.hand.PalmPosition.x;
			// if (handX > MOVING_DETECT_RANGE) {
			// 	GetBlockController().MoveBlock( DirectViaCamera(Vector3.right) * moveSpeed );
			// } else if (handX < -MOVING_DETECT_RANGE) {
			// 	GetBlockController().MoveBlock( DirectViaCamera(Vector3.left) * moveSpeed );
			// }
			//
			// // move block with opened hand in z-axis
			// float handZ = -leapHands.hand.PalmPosition.z;
			// if (handZ > MOVING_DETECT_RANGE) {
			// 	GetBlockController().MoveBlock( DirectViaCamera(Vector3.forward) * moveSpeed );
			// } else if (handZ < -MOVING_DETECT_RANGE) {
			// 	GetBlockController().MoveBlock( DirectViaCamera(Vector3.back) * moveSpeed );
			// }

			// hand position
			Vector3 handPos = VectorUtil.ToVector3(leapHands.hand.PalmPosition);
			handPos.z *= -1;
			handPos.y = 0;
			handPos /= 2;

			GetBlockController().MoveBlockSmoothly( DirectViaCamera(handPos), moveSpeed );
		}

		override
		protected void DetectDropMotion() {
			if (leapHands.HasTwoHands()) return;
			if (!LeapHands.IsGrabbing(leapHands.hand)) return;
			// const float rotateScale = 15;
			//if (!LeapHands.IsHorizontal(leapHands.hand, rotateScale)) return;

			// Drop Block with grabbed hand
			float velocityY = leapHands.hand.PalmVelocity.y;
			if (velocityY < -400) {
				GetBlockController().DropBlock();
			}
		}

		override
		protected void DetectRotation() {
			if (LeapHands.IsGrabbing(leapHands.hand)) return;
			if (leapHands.HasTwoHands()) return;
			if (isRotated) return;

			/// Pitch Block
			if (LeapHands.IsPitchingDown(leapHands.hand)) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.forward) );
				isRotated = true;
				return;
			} else if (LeapHands.IsPitchingUp(leapHands.hand)) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.back) );
				isRotated = true;
				return;
			}

			/// Yaw Block
			if (LeapHands.IsYawingRight(leapHands.hand)) {
				GetBlockController().YawBlock( Vector3.right );
				isRotated = true;
				return;
			} else if (LeapHands.IsYawingLeft(leapHands.hand)) {
				GetBlockController().YawBlock( Vector3.left );
				isRotated = true;
				return;
			}

			/// Roll Block
			if (LeapHands.IsRollingRight(leapHands.hand)) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.right) );
				isRotated = true;
				return;
			} else if (LeapHands.IsRollingLeft(leapHands.hand)) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.left) );
				isRotated = true;
				return;
			}
		}

		// Rotate camera
		override
		protected void DetectCameraRotation() {
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
			return true;
		}

		/// Shake Block
		override
		protected bool DetectShakeMotion() {
			if (!leapHands.HasTwoHands()) return false;

			float handVelocityY = leapHands.hand.PalmVelocity.y;
			float otherHandVelocityY = leapHands.otherHand.PalmVelocity.y;

			if ( !(handVelocityY < -300 && otherHandVelocityY < -300) ) return false;
			return true;
		}
	}
}
