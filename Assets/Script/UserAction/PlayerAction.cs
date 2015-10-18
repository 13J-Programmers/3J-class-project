///
/// @file  PlayerAction.cs
/// @brief Action classes inherit this base class to detect user inputs
///

using UnityEngine;
using System;
using System.Collections;

namespace Player.Action {
	/// PlayerAction < BaseAction < MonoBehaviour
	public abstract class PlayerAction : BaseAction {
		protected BlockController blockController;

		protected GameObject GetControllingBlock() {
			return GameObject.Find("block(new)");
		}

		protected BlockController GetBlockController() {
			return blockController;
		}

		protected CameraController GetCameraController() {
			return GameObject.Find("Main Camera").GetComponent<CameraController>();
		}

		/// these access modifier prevent the child script use Start()
		/// but, it doesn't work.
		protected sealed override void Start() {
			BlockEntity.CreateNewBlock   += new EventHandler(ConnectWithBlock);
			BlockController.StartFalling += new EventHandler(DisconnectWithBlock);
		}

		/// these access modifier prevent the child script use Update()
		/// but, it doesn't work.
		protected sealed override void Update() {
			InitPerFrame();
			if (ValidatePerFrame() == false) return;
			DetectMotionX();
			DetectMotionY();
			DetectMotionZ();
			DetectRotationX();
			DetectRotationY();
			DetectRotationZ();
			DetectRotationCamera();
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
		protected abstract void DetectMotionX();
		protected abstract void DetectMotionY();
		protected abstract void DetectMotionZ();
		protected abstract void DetectRotationX();
		protected abstract void DetectRotationY();
		protected abstract void DetectRotationZ();
		protected abstract void DetectRotationCamera();
		protected abstract bool DetectPressMotion();
		protected abstract bool DetectShakeMotion();

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
