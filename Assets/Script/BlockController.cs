using UnityEngine;
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
		Vector3 v = new Vector3(x, 0, z);
		transform.Translate(v, Space.World);
	}

	public void PitchBlock(int direct) { rotate(direct * 90, 0, 0); }
	public void YawBlock  (int direct) { rotate(0, direct * 90, 0); }
	public void RollBlock (int direct) { rotate(0, 0, direct * 90); }

	public void DropBlock() {
		// TODO:
		//  - change gameObject.name = "block(dropping)"
		//  - add gravity to drop block
		//  - call LeapHandAction#DisconectWithBlock()
		//  - call BlockPoolController#ControlBlock()
		//  - 

		if (gameObject.name.CompareTo("_DummyBlock") != 0) {
			gameObject.name = "block(dropping)";
			
			Physics.gravity = new Vector3(0, -9.81f, 0);
			rigidbody.useGravity = true;
			rigidbody.AddForce(Vector3.down * 500);
		}

		// after drop, OnCollisionEnter (private method) is called when landed on BlackPool.
	}


	// private methods ------------------------------------------------

	private void rotate(float x, float y, float z) {
		Vector3 v = new Vector3(x, y, z);
		transform.Rotate(v, Space.World);
	}

	private void OnCollisionEnter(Collision col){
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
