using UnityEngine;
using System;
using System.Collections;

public class BlockController : MonoBehaviour {
	public int blockNumber { get; set; }
	Rigidbody rigidbody;
	bool isCollideWall = false; // TODO: Remove

	// coordinate for check if collide the wall or not
	Vector3 blockMinCoord, blockMaxCoord;

	// Use this for initialization
	void Start() {
		rigidbody = GetComponent<Rigidbody>();

		// can vary only y position
		rigidbody.constraints = (
			RigidbodyConstraints.FreezePositionX |
			RigidbodyConstraints.FreezePositionZ | 
			RigidbodyConstraints.FreezeRotation
		);
	}
	
	// Update is called once per frame
	void Update() {
		// fix position
		Vector3 pos = transform.position;
		Vector3 minRange = new Vector3(-2, 0, -3);
		Vector3 maxRange = new Vector3(3, 0, 2);
		pos.x = Mathf.Clamp (pos.x, minRange.x, maxRange.x);
		pos.z = Mathf.Clamp (pos.z, minRange.z, maxRange.z);
		transform.position = pos;

		// TODO: if part of the block collide wall, fix position.
		SetMinMaxCoord();
	}

	// if the gameObject is out of camera range, destroy it.
	void OnBecameInvisible(){
		Destroy(gameObject);
	}

	public void MoveBlock(float x, float z) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		transform.Translate(new Vector3(x, 0, z), Space.World);
	}

	public void PitchBlock(int direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Rotate(direct * 90, 0, 0);
	}

	public void YawBlock(int direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Rotate(0, direct * 90, 0);
	}

	public void RollBlock(int direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Rotate(0, 0, direct * 90);
	}

	public void DropBlock() {
		// TODO:
		//  - change gameObject.name = "block(dropping)"
		//  - add gravity to drop block
		//  - call LeapHandAction#DisconectWithBlock()
		//  - call BlockPoolController#ControlBlock()
		//  - 

		if (gameObject.name.CompareTo("block(new)") != 0) return;

		gameObject.name = "block(dropping)";
		
		CorrectPosition();

		rigidbody.useGravity = true;
		rigidbody.AddForce(Vector3.down * 500);
		
		// after drop, OnCollisionEnter (private method) is called when landed on BlackPool.
	}


	// private methods ------------------------------------------------

	// rotate myself
	private void Rotate(float x, float y, float z) {
		Vector3 v = new Vector3(x, y, z);
		transform.Rotate(v, Space.World);
	}

	// called when this collider/rigidbody has begun touching another rigidbody/collider.
	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "BlockPool") {
			if (transform.position.y >= 1) {
				print("GameOver");
				return;
			}

			// connect Pool and block
			GameObject blockPoolObj = GameObject.Find("BlockPool");
			BlockPoolController blockPool = blockPoolObj.GetComponent<BlockPoolController>();
			blockPool.ControlBlock(gameObject);

			// disconnect Key and block
			GameObject keyActionObj = GameObject.Find("KeyAction");
			KeyAction keyAction = keyActionObj.GetComponent<KeyAction>();
			keyAction.DisconnectWithBlock();

			// create new block
			if (!GameObject.Find("block(new)")) {
				GameObject BlockEntityObj = GameObject.Find("BlockEntity");
				BlockEntity blockEntity = BlockEntityObj.GetComponent<BlockEntity>();
				blockEntity.CreateRandomBlock();
			}

			// connect Key and block
			keyAction.ConnectWithBlock();
		}
	}

	// called once per frame for every collider/rigidbody that is touching rigidbody/collider.
	private void OnCollisionStay(Collision col) {
		// TODO: Remove
		if (col.gameObject.tag == "Wall") {
			isCollideWall = true;
		}
	}

	// called when this collider/rigidbody has stopped touching another rigidbody/collider.
	private void OnCollisionExit(Collision col) {
		// TODO: Remove
		if (col.gameObject.tag == "Wall") {
			isCollideWall = false;
		}
	}

	// return myself correct position
	private Vector3 CorrectPosition() {
		Vector3 correctedPos;
		correctedPos.x = (float)Math.Round(transform.position.x);
		correctedPos.y = (float)transform.position.y;
		correctedPos.z = (float)Math.Round(transform.position.z);
		transform.position = correctedPos;
		return correctedPos;
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
		
		// print("---");
		// print("block min pos : " + blockMinCoord);
		// print("block max pos : " + blockMaxCoord);
	}
}


















