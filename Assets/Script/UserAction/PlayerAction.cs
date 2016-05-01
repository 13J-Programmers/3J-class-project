///
/// @file  PlayerAction.cs
/// @brief
///   Action classes inherit this base class to detect user inputs
///   + class name:  *Action
///   + method name: *Motion
///

using UnityEngine;
using System;
using System.Collections;

namespace Player {
	// declare motions
	public enum Motion {
		Null,
		Forward, Backward, Up, Down, Right, Left,
		PitchUp, PitchDown, YawRight, YawLeft, RollRight, RollLeft,
		RotateCameraRight, RotateCameraLeft,
		Press, Shake
	}
}

namespace Player.Action {
	/// PlayerAction < MonoBehaviour
	public abstract class PlayerAction : MonoBehaviour {
		private BlockController blockController;

		protected GameObject GetControllingBlock() {
			return GameObject.Find("block(new)");
		}

		protected BlockController GetBlockController() {
			return blockController;
		}

		protected CameraController GetCameraController() {
			return GameObject.Find("Main Camera").GetComponent<CameraController>();
		}

		void Start() {
			BlockEntity.CreateNewBlock   += new EventHandler(ConnectWithBlock);
			BlockController.StartFalling += new EventHandler(DisconnectWithBlock);
		}

		// If this Update method was overrided, everythings goes wrong.
		void Update() {
			InitPerFrame();
			if (ValidatePerFrame() == false) return;

			DetectMotion();
			DetectDropMotion();
			DetectRotation();
			DetectCameraRotation();

			// if new block is pressable, and detect press motion.
			if (Test.test(() => GameObject.Find("block(new)").tag == "Pressable")
					&& DetectPressMotion()) {
				DestroyChildBlocks();
			}

			// if new block is shakable, and detect shake motion.
			if (Test.test(() => GameObject.Find("block(new)").tag == "Shakable")
					&& DetectShakeMotion()) {
				DestroyChildBlocksAndGenerateSplash();
			}
		}

		protected virtual void InitPerFrame() {
			// init something
		}
		protected virtual bool ValidatePerFrame() {
			// validate something
			return true;
		}
		protected abstract void DetectMotion();
		protected abstract void DetectDropMotion();
		protected abstract void DetectRotation();
		protected abstract void DetectCameraRotation();
		protected abstract bool DetectPressMotion();
		protected abstract bool DetectShakeMotion();

		protected Vector3 DirectViaCamera(Vector3 direction) {
			return Camera.main.transform.TransformDirection(direction);
		}

		/// get component of the new block to connent
		private void ConnectWithBlock(object sender, EventArgs e) {
			GameObject target = GameObject.Find("block(new)");
			if (!target) return;
			blockController = target.GetComponent<BlockController>();
		}

		/// get component of the _DummyBlock to disconnect
		private void DisconnectWithBlock(object sender, EventArgs e) {
			GameObject target = GameObject.Find("_DummyBlock");
			if (!target) return;
			blockController = target.GetComponent<BlockController>();
		}

		private void DestroyChildBlocks() {
			ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			if (destroyPositions.Count == 0) return;

			GameObject.Find("GameManager").GetComponent<GameManager>().score += 50;
			GameObject.Find("sounds/press(audio)").GetComponent<Sound>().Play();
		}

		private void DestroyChildBlocksAndGenerateSplash() {
			// destroy child blocks
			ArrayList destroyPositions = GetBlockController().DestroyChildBlocks();
			if (destroyPositions.Count == 0) return;

			// generate splash
			GenerateSplash(destroyPositions);

			GameObject.Find("GameManager").GetComponent<GameManager>().score += 50;
		}

		private void GenerateSplash(ArrayList destroyPositions) {
			if (destroyPositions.Count == 0) return;

			// generate splash in destroyed block positions
			foreach (Vector3 destroyPosition in destroyPositions) {
				GetBlockController().GenerateSplash(destroyPosition);
			}
		}
	}
}
