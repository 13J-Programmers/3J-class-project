/// 
/// @file   KeyAction.cs
/// @brief  This script will turn pressing key into game obj motion.
/// 

using UnityEngine;
using System.Collections;

namespace Player.Action {
	// KeyAction extend UserAction
	public class KeyAction : PlayerAction {
		private Transform mainCamera;
		private float moveSpeed = 0.1f;

		// Use this for initialization
		void Start() {
			cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
		}

		override
		protected void InitPerFrame() {
			mainCamera = Camera.main.transform;
		}

		override
		protected bool ValidatePerFrame() {
			return base.blockController;
		}

		/// Move Block in X-axis
		override
		protected void DetectMotionX() {
			if (Input.GetKey("right")) {
				Vector3 right = mainCamera.TransformDirection(Vector3.right) * moveSpeed;
				blockController.MoveBlock(right);
			} else if (Input.GetKey("left")) {
				Vector3 left = mainCamera.TransformDirection(Vector3.left) * moveSpeed;
				blockController.MoveBlock(left);
			}
		}

		/// Drop Block 
		override
		protected void DetectMotionY() {
			if (Input.GetKeyDown("space")) {
				blockController.DropBlock();
			}
		}

		// Move Block in Z-axis
		override
		protected void DetectMotionZ() {
			if (Input.GetKey("up")) {
				Vector3 forward = mainCamera.TransformDirection(Vector3.forward) * moveSpeed;
				blockController.MoveBlock(forward);
			} else if (Input.GetKey("down")) {
				Vector3 back = mainCamera.TransformDirection(Vector3.back) * moveSpeed;
				blockController.MoveBlock(back);
			}
		}

		/// Pitch Block
		override
		protected void DetectRotationX() {
			if (Input.GetKeyDown("w")) {
				Vector3 forward = mainCamera.TransformDirection(Vector3.forward);
				blockController.PitchBlock(forward);
			} else if (Input.GetKeyDown("s")) {
				Vector3 back = mainCamera.TransformDirection(Vector3.back);
				blockController.PitchBlock(back);
			}
		}

		/// Yaw Block
		override
		protected void DetectRotationY() {
			if (Input.GetKeyDown("e")) {
				blockController.YawBlock(1);
			} else if (Input.GetKeyDown("q")) {
				blockController.YawBlock(-1);
			}
		}

		/// Roll Block
		override
		protected void DetectRotationZ() {
			if (Input.GetKeyDown("d")) {
				Vector3 right = mainCamera.TransformDirection(Vector3.right);
				blockController.RollBlock(right);
			} else if (Input.GetKeyDown("a")) {
				Vector3 left = mainCamera.TransformDirection(Vector3.left);
				blockController.RollBlock(left);
			}
		}

		/// Rotate Camera 
		override
		protected void DetectRotationCamera() {
			if (Input.GetKey("return")) {
				cameraController.RotateCam(1);
			} else if (Input.GetKey("delete")) {
				cameraController.RotateCam(-1);
			}
		}
	}
}


