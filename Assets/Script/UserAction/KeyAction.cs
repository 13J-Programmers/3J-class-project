///
/// @file   KeyAction.cs
/// @brief  This script will turn pressing key into game obj motion.
///

using UnityEngine;
using System.Collections;

namespace Player.Action {
	/// KeyAction < PlayerAction < MonoBehaviour
	public class KeyAction : PlayerAction {
		private float moveSpeed = 0.1f;

		override
		protected void InitPerFrame() {

		}

		override
		protected bool ValidatePerFrame() {
			return GetBlockController();
		}

		// detect motion
		override
		protected void DetectMotion() {
			// Move Block in X-axis
			if (Input.GetKey("right")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.right) * moveSpeed );
			} else if (Input.GetKey("left")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.left) * moveSpeed );
			}

			// Move Block in Z-axis
			if (Input.GetKey("up")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.forward) * moveSpeed );
			} else if (Input.GetKey("down")) {
				GetBlockController().MoveBlock( DirectViaCamera(Vector3.back) * moveSpeed );
			}
		}

		// detect drop motion
		override
		protected void DetectDropMotion() {
			// Drop Block
			if (Input.GetKeyDown("space")) {
				GetBlockController().DropBlock();
			}
		}

		// detect rotation
		override
		protected void DetectRotation() {
			// Pitch Block
			if (Input.GetKeyDown("w")) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.forward) );
			} else if (Input.GetKeyDown("s")) {
				GetBlockController().PitchBlock( DirectViaCamera(Vector3.back) );
			}

			// Yaw Block
			if (Input.GetKeyDown("e")) {
				GetBlockController().YawBlock( Vector3.right );
			} else if (Input.GetKeyDown("q")) {
				GetBlockController().YawBlock( Vector3.left );
			}

			// Roll Block
			if (Input.GetKeyDown("d")) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.right) );
			} else if (Input.GetKeyDown("a")) {
				GetBlockController().RollBlock( DirectViaCamera(Vector3.left) );
			}
		}

		/// Rotate Camera
		override
		protected void DetectCameraRotation() {
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
			return true;
		}

		/// Shake Block
		override
		protected bool DetectShakeMotion() {
			if (!Input.GetKey("o")) return false;
			return true;
		}
	}
}
