using UnityEngine;
using System;
using System.Collections;

public class BlockController : MonoBehaviour {
	public int blockNumber { get; set; }
	Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();

		// can vary only y position
		rigidbody.constraints = (
			RigidbodyConstraints.FreezePositionX |
			RigidbodyConstraints.FreezePositionZ | 
			RigidbodyConstraints.FreezeRotation
		);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// if the gameObject is out of camera range, destroy it.
	void OnBecameInvisible(){
		Destroy(gameObject);
	}

	public void MoveBlock(float x, float z) {
		if (gameObject.name.CompareTo("block") != 0) return;
		transform.Translate(new Vector3(x, 0, z), Space.World);
	}

	public void PitchBlock(int direct) {
		if (gameObject.name.CompareTo("block") != 0) return;
		Rotate(direct * 90, 0, 0);
	}
	public void YawBlock(int direct) {
		if (gameObject.name.CompareTo("block") != 0) return;
		Rotate(0, direct * 90, 0);
	}
	public void RollBlock(int direct) {
		if (gameObject.name.CompareTo("block") != 0) return;
		Rotate(0, 0, direct * 90);
	}

	public void DropBlock() {
		// TODO:
		//  - change gameObject.name = "block(dropping)"
		//  - add gravity to drop block
		//  - call LeapHandAction#DisconectWithBlock()
		//  - call BlockPoolController#ControlBlock()
		//  - 

		if (gameObject.name.CompareTo("block") != 0) return;

		gameObject.name = "block(dropping)";
		
		CorrectPosition();

		rigidbody.useGravity = true;
		rigidbody.AddForce(Vector3.down * 500);
		
		// after drop, OnCollisionEnter (private method) is called when landed on BlackPool.
	}


	// private methods ------------------------------------------------

	private void Rotate(float x, float y, float z) {
		Vector3 v = new Vector3(x, y, z);
		transform.Rotate(v, Space.World);
	}

	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "BlockPool") {
			if (transform.position.y >= 1) {
				print("GameOver");
				return;
			}

			gameObject.name = "block(landed)";
			gameObject.tag = "BlockPool";

			// disconnect Key and block
			GameObject keyActionObj = GameObject.Find("KeyAction");
			KeyAction keyAction = keyActionObj.GetComponent<KeyAction>();
			keyAction.DisconnectWithBlock();

			// create new block
			if (!GameObject.Find("block")) {
				GameObject BlockEntityObj = GameObject.Find("BlockEntity");
				BlockEntity blockEntity = BlockEntityObj.GetComponent<BlockEntity>();
				blockEntity.CreateRandomBlock();
			}

			// connect Key and block
			keyAction.ConnectWithBlock();
		}
	}

	private Vector3 CorrectPosition() {
		Vector3 correctedPos;
		correctedPos.x = (float)Math.Round(transform.position.x);
		correctedPos.y = (float)transform.position.y;
		correctedPos.z = (float)Math.Round(transform.position.z);
		transform.position = correctedPos;
		return correctedPos;
 	}
}
