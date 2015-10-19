///
/// @file   KeyAction.cs
/// @brief  This script will turn pressing key into game obj motion.
///

using UnityEngine;
using System.Collections;

namespace Player.Action {
	/// KeyAction < PlayerAction < BaseAction < MonoBehaviour
	public class KeyAction : PlayerAction {
		private float moveSpeed = 0.1f;

		override
		protected void InitPerFrame() {
			
		}

		override
		protected bool ValidatePerFrame() {
			return GetBlockController();
		}

		/// Move Block in X-axis
		override
		protected void DetectMotionX() {
			if (Input.GetKey("right")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.right) * moveSpeed );
			} else if (Input.GetKey("left")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.left) * moveSpeed );
			}
		}

		/// Drop Block
		override
		protected void DetectMotionY() {
			if (Input.GetKeyDown("space")) {
				GetBlockController().DropBlock();
			}
		}

		// Move Block in Z-axis
		override
		protected void DetectMotionZ() {
			if (Input.GetKey("up")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.forward) * moveSpeed );
			} else if (Input.GetKey("down")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.back) * moveSpeed );
			}
		}

		/// Pitch Block
		override
		protected void DetectRotationX() {
			if (Input.GetKeyDown("w")) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.forward) );
			} else if (Input.GetKeyDown("s")) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.back) );
			}
		}

		/// Yaw Block
		override
		protected void DetectRotationY() {
			if (Input.GetKeyDown("e")) {
				GetBlockController().YawBlock( Vector3.right );
			} else if (Input.GetKeyDown("q")) {
				GetBlockController().YawBlock( Vector3.left );
			}
		}

		/// Roll Block
		override
		protected void DetectRotationZ() {
			if (Input.GetKeyDown("d")) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.right) );
			} else if (Input.GetKeyDown("a")) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.left) );
			}
		}

		/// Rotate Camera
		override
		protected void DetectRotationCamera() {
			if (Input.GetKey("return")) {
				GetCameraController().RotateCamera( Vector3.right );
			} else if (Input.GetKey("delete")) {
				GetCameraController().RotateCamera( Vector3.left );
			}
		}

		/// Press Block
		override
		protected bool DetectPressMotion() {
			if (!Input.GetKey("p")) return false;

			// // try to destroy every child blocks
			// ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			// if (destroyPositions.Count == 0) return false;

			return true;
		}

		/// Shake Block
		override
		protected bool DetectShakeMotion() {
			if (!Input.GetKey("o")) return false;

			// // try to destroy every child blocks
			// ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			// if (destroyPositions.Count == 0) return false;
			// 
			// // generate splash in destroyed block positions
			// foreach (Vector3 destroyPosition in destroyPositions) {
			// 	GetBlockController().GenerateSplash(destroyPosition);
			// }

			return true;
		}
	}
}
