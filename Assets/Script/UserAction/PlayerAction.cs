/// 
/// @file  PlayerAction.cs
/// @brief Action classes inherit this base class to detect user inputs
/// 

using UnityEngine;
using System.Collections;

namespace Player.Action {
	public abstract class PlayerAction : BaseAction {
		protected BlockController blockController;
		protected CameraController cameraController;
		private GameObject target; ///< A container for control target
		
		// these access modifier prevent the child script use Update()
		// but, it doesn't work.
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

		/// get component of the new block for comment
		public void ConnectWithBlock() {
			target = GameObject.Find("block(new)");
			if (!target) return;
			blockController = target.GetComponent<BlockController>();
		}

		/// get component of the _DummyBlock for disconnect
		public void DisconnectWithBlock() {
			target = GameObject.Find("_DummyBlock");
			if (!target) return;
			blockController = target.GetComponent<BlockController>();
		}
	}
}
