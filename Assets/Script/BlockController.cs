using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BlockController : MonoBehaviour {
	Rigidbody rigidbody;
	BlockPoolController blockPool;
	KeyAction keyAction;
	BlockEntity blockEntity;
	// coordinate for check if collide the wall or not
	Vector3 blockMinCoord, blockMaxCoord;

	// Use this for initialization
	void Start() {
		rigidbody = GetComponent<Rigidbody>();
		blockPool = GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
		keyAction = GameObject.Find("KeyAction").GetComponent<KeyAction>();
		blockEntity = GameObject.Find("BlockEntity").GetComponent<BlockEntity>();

		// can vary only y position
		rigidbody.constraints = (
			RigidbodyConstraints.FreezePositionX |
			RigidbodyConstraints.FreezePositionZ | 
			RigidbodyConstraints.FreezeRotation
		);
	}
	
	// Update is called once per frame
	void Update() {
		// set the block min-max coordinate
		SetMinMaxCoord();
		// fix position
		FixPosition();
	}

	// if the gameObject is out of camera range, destroy it.
	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	// move the block
	public void MoveBlock(float x, float z) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;

		Dictionary<string, float> wallPos = blockPool.GetWallPosition();
		float halfOfWidth = transform.localScale.x / 2;
		// print("Wall x min-max : " + wallPos["x-min"] + "..." + wallPos["x-max"]);
		// print("Wall z min-max : " + wallPos["z-min"] + "..." + wallPos["z-max"]);
		// print("Block x min-max : " + blockMinCoord.x + "..." + blockMaxCoord.x);
		// print("Block z min-max : " + blockMinCoord.z + "..." + blockMaxCoord.z);
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

	// MoveBlock arguments can be Vector3
	public void MoveBlock(Vector3 vector) {
		MoveBlock(vector.x, vector.z);
	}

	// // pitch the block
	// public void PitchBlock(int direct) {
	// 	if (gameObject.name.CompareTo("block(new)") != 0) return;
	// 	Rotate(direct * 90, 0, 0);
	// }

	// // yaw the block
	// public void YawBlock(int direct) {
	// 	if (gameObject.name.CompareTo("block(new)") != 0) return;
	// 	Rotate(0, direct * 90, 0);
	// }

	// // roll the block
	// public void RollBlock(int direct) {
	// 	if (gameObject.name.CompareTo("block(new)") != 0) return;
	// 	Rotate(0, 0, direct * 90);
	// }

	// pitch the block
	public void PitchBlock(Vector3 direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Vector3 newDirect = CorrectDirection(direct);
		Debug.Log(newDirect);
		if (Math.Abs(direct.x) >= Math.Abs(direct.z))
			Rotate(newDirect.x * 90, 0, 0);
		else if (Math.Abs(direct.x) < Math.Abs(direct.z))
			Rotate(0, 0, newDirect.z * 90);
	}

	// yaw the block
	public void YawBlock(int direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Rotate(0, direct * 90, 0);
	}

	// roll the block
	public void RollBlock(Vector3 direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Vector3 newDirect = CorrectDirection(direct);
			Debug.Log(newDirect);
		if (Math.Abs(direct.x) >= Math.Abs(direct.z))
			Rotate(newDirect.x * 90, 0, 0);
		else if (Math.Abs(direct.x) < Math.Abs(direct.z))
			Rotate(0, 0, newDirect.z * 90);

	}


	// drop the block
	//   change gameObject.name = "block(dropping)"
	//   correct the position
	//   add gravity to drop block
	public void DropBlock() {
		if (gameObject.name.CompareTo("block(new)") != 0) return;

		gameObject.name = "block(dropping)";
		CorrectPosition();
		rigidbody.useGravity = true;
		rigidbody.AddForce(Vector3.down * 500);
		
		// after drop, OnCollisionEnter (private method) is called when landed on BlackPool.
	}

	// return correct coordinate
	public Vector3 GetCorrectPosition() {
		Vector3 correctedPos;
		correctedPos.x = (float)Math.Round(transform.position.x);
		correctedPos.y = (float)transform.position.y;
		correctedPos.z = (float)Math.Round(transform.position.z);
		return correctedPos;
	}


	// private methods ------------------------------------------------

	// rotate myself
	private void Rotate(float x, float y, float z) {
		Vector3 v = new Vector3(x, y, z);
		transform.Rotate(v, Space.World);
	}

	// called when this collider/rigidbody has begun touching another rigidbody/collider.
	private void OnCollisionEnter(Collision col) {
		if (gameObject.name.CompareTo("block(dropping)") != 0) return;

		if (col.gameObject.tag == "BlockPool") {
			// TODO: In future, this if-sentence will be removed.
			if (transform.position.y >= 1) {
				print("GameOver");
				return;
			}
			
			// following script behaves:
			//
			//                     block
			//   BlockController --------> BlockPoolController
			//               connect
			//   keyAction -----X----> BlockController
			//
			blockPool.ControlBlock(gameObject);
			keyAction.DisconnectWithBlock();

			// TODO: following scripts will expect to be called from BlockPool

			// create new block to do next phase
			//
			//               create
			//   BlockPool ----------> BlockEntity
			//
			if (!GameObject.Find("block(new)")) {
				blockEntity.CreateRandomBlock();
			}
			//               connect
			//   keyAction ----------> BlockController
			//
			keyAction.ConnectWithBlock();

			// All jobs has finished. So destroy this script.
			Destroy(this);
		}
	}

	// called once per frame for every collider/rigidbody that is touching rigidbody/collider.
	private void OnCollisionStay(Collision col) {}

	// called when this collider/rigidbody has stopped touching another rigidbody/collider.
	private void OnCollisionExit(Collision col) {}

	// return myself correct position
	private Vector3 CorrectPosition() {
		Vector3 correctedPos;
		correctedPos.x = (float)Math.Round(transform.position.x);
		correctedPos.y = (float)transform.position.y;
		correctedPos.z = (float)Math.Round(transform.position.z);
		transform.position = correctedPos;
		return correctedPos;
	}

	// return myself correct position
	private Vector3 CorrectDirection(Vector3 currentPosition) {
		Vector3 correctedDir;
		correctedDir.x = (float)Math.Round(currentPosition.x);
		correctedDir.y = (float)currentPosition.y;
		correctedDir.z = (float)Math.Round(currentPosition.z);
		return correctedDir;
	}

	// set the block min-max x,y,z coordinate
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

	// after rotate, if part of the block into wall, fix position
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


















