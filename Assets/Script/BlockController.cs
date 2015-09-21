/// 
/// @file   BlockController.cs
/// @brief  controls block position and rotation.
/// 

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Player.Action;

public class BlockController : MonoBehaviour {
	BlockPoolController blockPool;
	KeyAction keyAction;
	LeapHandAction leapHandAction;
	BlockEntity blockEntity;
	ExpectDropPosViewer expectDropPosViewer;
	// coordinate for check if collide the wall or not
	Vector3 blockMinCoord; ///< max coordinate in block
	Vector3 blockMaxCoord; ///< min coordinate in block

	/// Use this for initialization
	void Start() {
		blockPool = GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
		keyAction = GameObject.Find("KeyAction").GetComponent<KeyAction>();
		leapHandAction = GameObject.Find("LeapHandAction").GetComponent<LeapHandAction>();
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();
		expectDropPosViewer = gameObject.GetComponent<ExpectDropPosViewer>();

		// can vary only y position
		GetComponent<Rigidbody>().constraints = (
			RigidbodyConstraints.FreezePositionX |
			RigidbodyConstraints.FreezePositionZ | 
			RigidbodyConstraints.FreezeRotation
		);
	}
	
	/// Update is called once per frame
	void Update() {
		// set the block min-max coordinate
		SetMinMaxCoord();
		// after rotate, if the block is outside the Pool, then fix position
		FixPosition();
	}

	/// if the gameObject is out of camera range, destroy it.
	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	/// move the block
	/// @param x - moving x direction
	/// @param z - moving z direction
	public void MoveBlock(float x, float z) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;

		// block can move in specific range
		// 
		//   wallPos : coordinate of wall of Pool
		//   blockMinCoord : minimum x,z coordinate
		//   blockMaxCoord : maximum x,z coordinate
		//   halfOfWidth : blockWidth / 2
		// 
		//            --- << blockMaxCoord.z + halfOfWidth
		//           |   |
		//    --- --- --- 
		//   |   |   |   |
		//    --- --- --- << blockMinCoord.z - halfOfWidth
		//   ∧           ∧
		//   ∧           blockMaxCoord.x + halfOfWidth
		//   blockMinCoord.x - halfOfWidth
		// 
		// if block collides wall, it cannot move
		// 
		Dictionary<string, float> wallPos = blockPool.GetWallPosition();
		float halfOfWidth = transform.localScale.x / 2;
		if (wallPos["x-min"] > blockMinCoord.x - halfOfWidth) {
			x = (x < 0) ? 0 : x;
		} else if (wallPos["x-max"] < blockMaxCoord.x + halfOfWidth) {
			x = (x > 0) ? 0 : x;
		}
		if (wallPos["z-min"] > blockMinCoord.z - halfOfWidth) {
			z = (z < 0) ? 0 : z;
		} else if (wallPos["z-max"] < blockMaxCoord.z + halfOfWidth) {
			z = (z > 0) ? 0 : z;
		}
		
		transform.Translate(new Vector3(x, 0, z), Space.World);
	}

	/// MoveBlock arguments can be Vector3
	public void MoveBlock(Vector3 vector) {
		MoveBlock(vector.x, vector.z);
	}

	/// pitch the block
	/// this method can decide forward direction via camera.
	/// @param direct - Vector3 forward or back 
	public void PitchBlock(Vector3 direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Vector3 newDirect = roundXZ(direct);
		//Debug.Log(direct + " => " + newDirect + "; " + (Math.Abs(direct.x) >= Math.Abs(direct.z)) );
		if (Math.Abs(direct.x) >= Math.Abs(direct.z)) {
			Rotate(0, 0, -newDirect.x * 90);
		} else {
			Rotate(newDirect.z * 90, 0, 0);
		}
	}

	/// yaw the block
	public void YawBlock(int direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Rotate(0, direct * 90, 0);
	}

	/// roll the block
	/// this method can decide right direction via camera.
	/// @param direct - Vector3 right or left 
	public void RollBlock(Vector3 direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Vector3 newDirect = roundXZ(direct);
		//Debug.Log(direct + " => " + newDirect + "; " + (Math.Abs(direct.x) >= Math.Abs(direct.z)) );
		if (Math.Abs(direct.x) >= Math.Abs(direct.z)) {
			Rotate(0, 0, -newDirect.x * 90);
		} else {
			Rotate(newDirect.z * 90, 0, 0);
		}
	}


	/// drop the block
	/// steps:
	///   1. change gameObject.name = "block(dropping)"
	///   2. correct position of the block
	///   3. add gravity to drop block
	public void DropBlock() {
		if (gameObject.name.CompareTo("block(new)") != 0) return;

		gameObject.name = "block(dropping)";
		CorrectPosition();
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().AddForce(Vector3.down * 500);

		expectDropPosViewer.StopSync();
		
		// after drop, OnCollisionEnter (private method) is called when landed on BlackPool.
	}

	/// return correct coordinate
	public Vector3 GetCorrectPosition() {
		Vector3 correctedPos = roundXZ(transform.position);
		return correctedPos;
	}


	// private methods ------------------------------------------------

	/// rotate myself in world space coordinate
	/// @param x - rotate in x-axis [deg]
	/// @param y - rotate in y-axis [deg]
	/// @param z - rotate in z-axis [deg]
	private void Rotate(float x, float y, float z) {
		Vector3 v = new Vector3(x, y, z);
		transform.Rotate(v, Space.World);
	}

	/// called when this collider/rigidbody has begun touching another rigidbody/collider.
	/// @param col - another collider info
	private void OnCollisionEnter(Collision col) {
		if (gameObject.name.CompareTo("block(dropping)") != 0) return;

		if (col.gameObject.tag == "BlockPool") {
			// following script behaves:
			//
			//                     block
			//   BlockController --------> BlockPoolController
			//              disconnect
			//   Actions -------X------> BlockController
			//
			blockPool.ControlBlock(gameObject);
			keyAction.DisconnectWithBlock();
			leapHandAction.DisconnectWithBlock();

			// destory expected drop pos
			expectDropPosViewer.StopShowing();

			// create new block to do next phase
			//
			//               create
			//   BlockPool ----------> BlockEntity
			//
			if (!GameObject.Find("block(new)")) {
				blockEntity.CreateRandomBlock();
			}
			//             connect
			//   Actions ----------> BlockController
			//
			keyAction.ConnectWithBlock();
			leapHandAction.ConnectWithBlock();

			// All jobs has finished. So destroy blockControl script.
			Destroy(gameObject.GetComponent<ExpectDropPosViewer>());
			Destroy(this); // destroy BlockController component
		}
	}

	/// round x,z coordinate
	private Vector3 roundXZ(Vector3 vector) {
		Vector3 _vector;
		_vector.x = (float)Math.Round(vector.x);
		_vector.y = vector.y;
		_vector.z = (float)Math.Round(vector.z);
		return _vector;
	}

	/// move correct position and return it position
	/// @return corrected position
	private Vector3 CorrectPosition() {
		Vector3 correctedPos = roundXZ(transform.position);
		transform.position = correctedPos;
		return correctedPos;
	}

	/// return myself correct direction
	private Vector3 CorrectDirection(Vector3 currentPosition) {
		Vector3 correctedDir = roundXZ(currentPosition);
		return correctedDir;
	}

	/// set the block min-max x,y,z coordinate
	private void SetMinMaxCoord() {
		// init block min-max coordinate
		blockMinCoord = blockMaxCoord = transform.position;

		foreach (Transform child in transform) {
			// set min-max
			float childX = child.transform.position.x;
			float childY = child.transform.position.y;
			float childZ = child.transform.position.z;
			if (blockMinCoord.x > childX) blockMinCoord.x = childX;
			if (blockMinCoord.y > childY) blockMinCoord.y = childY;
			if (blockMinCoord.z > childZ) blockMinCoord.z = childZ;
			if (blockMaxCoord.x < childX) blockMaxCoord.x = childX;
			if (blockMaxCoord.y < childY) blockMaxCoord.y = childY;
			if (blockMaxCoord.z < childZ) blockMaxCoord.z = childZ;
		}
	}

	/// after rotate, if part of the block into wall, fix position
	private void FixPosition() {
		Dictionary<string, float> wallPos = blockPool.GetWallPosition();
		if (wallPos["x-min"] > blockMinCoord.x) {
			transform.Translate(Vector3.right, Space.World);
		} else if (wallPos["x-max"] < blockMaxCoord.x) {
			transform.Translate(Vector3.left, Space.World);
		}
		if (wallPos["z-min"] > blockMinCoord.z) {
			transform.Translate(Vector3.forward, Space.World);
		} else if (wallPos["z-max"] < blockMaxCoord.z) {
			transform.Translate(Vector3.back, Space.World);
		}
	}
}


















