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
		protected CameraController cameraController;

		/// these access modifier prevent the child script use Start()
		/// but, it doesn't work.
		protected sealed override void Start() {
			cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
			BlockEntity.CreateNewBlock += new EventHandler(ConnectWithBlock);
			BlockController.StopFalling += new EventHandler(DisconnectWithBlock);
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
		public void ConnectWithBlock(object sender, EventArgs e) {
			GameObject target = GameObject.Find("block(new)");
			if (!target) return;
			blockController = target.GetComponent<BlockController>();
		}

		/// get component of the _DummyBlock for disconnect
		public void DisconnectWithBlock(object sender, EventArgs e) {
			GameObject target = GameObject.Find("_DummyBlock");
			if (!target) return;
			blockController = target.GetComponent<BlockController>();
		}
	}
}
