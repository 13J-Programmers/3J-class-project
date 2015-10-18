///
/// @file   KeyAction.cs
/// @brief  This script will turn pressing key into game obj motion.
///

using UnityEngine;
using System.Collections;
using Fie.Utility;

namespace Player.Action {
	/// KeyAction < PlayerAction < BaseAction < MonoBehaviour
	public class KeyAction : PlayerAction {
		private Transform mainCamera;
		private float moveSpeed = 0.1f;

		override
		protected void InitPerFrame() {
			mainCamera = Camera.main.transform;
		}

		override
		protected bool ValidatePerFrame() {
			return GetBlockController();
		}

		/// Move Block in X-axis
		override
		protected void DetectMotionX() {
			if (Input.GetKey("right")) {
				Vector3 right = mainCamera.TransformDirection(Vector3.right) * moveSpeed;
				GetBlockController().MoveBlock(right);
			} else if (Input.GetKey("left")) {
				Vector3 left = mainCamera.TransformDirection(Vector3.left) * moveSpeed;
				GetBlockController().MoveBlock(left);
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
				Vector3 forward = mainCamera.TransformDirection(Vector3.forward) * moveSpeed;
				GetBlockController().MoveBlock(forward);
			} else if (Input.GetKey("down")) {
				Vector3 back = mainCamera.TransformDirection(Vector3.back) * moveSpeed;
				GetBlockController().MoveBlock(back);
			}
		}

		/// Pitch Block
		override
		protected void DetectRotationX() {
			if (Input.GetKeyDown("w")) {
				Vector3 forward = mainCamera.TransformDirection(Vector3.forward);
				GetBlockController().PitchBlock(forward);
			} else if (Input.GetKeyDown("s")) {
				Vector3 back = mainCamera.TransformDirection(Vector3.back);
				GetBlockController().PitchBlock(back);
			}
		}

		/// Yaw Block
		override
		protected void DetectRotationY() {
			if (Input.GetKeyDown("e")) {
				GetBlockController().YawBlock(1);
			} else if (Input.GetKeyDown("q")) {
				GetBlockController().YawBlock(-1);
			}
		}

		/// Roll Block
		override
		protected void DetectRotationZ() {
			if (Input.GetKeyDown("d")) {
				Vector3 right = mainCamera.TransformDirection(Vector3.right);
				GetBlockController().RollBlock(right);
			} else if (Input.GetKeyDown("a")) {
				Vector3 left = mainCamera.TransformDirection(Vector3.left);
				GetBlockController().RollBlock(left);
			}
		}

		/// Rotate Camera
		override
		protected void DetectRotationCamera() {
			if (Input.GetKey("return")) {
				GetCameraController().RotateCam(1);
			} else if (Input.GetKey("delete")) {
				GetCameraController().RotateCam(-1);
			}
		}

		/// Press Block
		override
		protected bool DetectPressMotion() {
			if (!Input.GetKey("p")) return false;

			// try to destroy every child blocks
			ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			if (destroyPositions.Count == 0) return false;

			return true;
		}

		/// Shake Block
		override
		protected bool DetectShakeMotion() {
			if (!Input.GetKey("o")) return false;

			// try to destroy every child blocks
			ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			if (destroyPositions.Count == 0) return false;
			
			// generate splash in destroyed block positions
			foreach (Vector3 destroyPosition in destroyPositions) {
				GetBlockController().GenerateSplash(destroyPosition);
			}

			return true;
		}
	}
}
