///
/// @file   BlockController.cs
/// @brief  controls block position and rotation.
///

using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class BlockController : MonoBehaviour {
	// coordinate for check if collide the wall or not
	private Vector3 blockMinCoord; ///< max coordinate in block
	private Vector3 blockMaxCoord; ///< min coordinate in block

	public static event EventHandler StartFalling; ///< when start falling
	public static event EventHandler StopFalling;  ///< when stop falling
	public static event EventHandler WhenDestroyChild; ///< when destroy child

	static public void InitStaticField() {
		StartFalling = StopFalling = WhenDestroyChild = null;
	}

	private BlockPoolController GetBlockPoolController() {
		return GameObject.Find("BlockPool").GetComponent<BlockPoolController>();
	}

	void Start() {
		// can vary only y position
		GetComponent<Rigidbody>().constraints = (
			RigidbodyConstraints.FreezePositionX |
			RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezeRotation
		);
	}

	void Update() {
		SetMinMaxCoord();
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
		Wall wall = GetBlockPoolController().GetWall();
		float halfOfWidth = transform.localScale.x / 2;
		if (blockMinCoord.x - halfOfWidth < wall.GetMinX()) {
			x = (x < 0) ? 0 : x;
		} else if (blockMaxCoord.x + halfOfWidth > wall.GetMaxX()) {
			x = (x > 0) ? 0 : x;
		}
		if (blockMinCoord.z - halfOfWidth < wall.GetMinZ()) {
			z = (z < 0) ? 0 : z;
		} else if (blockMaxCoord.z + halfOfWidth > wall.GetMaxZ()) {
			z = (z > 0) ? 0 : z;
		}

		transform.Translate(new Vector3(x, 0, z), Space.World);
	}

	/// MoveBlock arguments can be Vector3
	public void MoveBlock(Vector3 vector) {
		MoveBlock(vector.x, vector.z);
	}

	/// move the block smoothly to *toPosition*
	/// @param toPosition - position of destination
	/// @param speed      - movement speed
	public void MoveBlockSmoothly(Vector3 toPosition, float speed) {
		Vector3 fromPosition = this.transform.position;
		toPosition.y = fromPosition.y;

		Wall wall = GetBlockPoolController().GetWall();
		float halfOfWidth = transform.localScale.x / 2;
		if (blockMinCoord.x - halfOfWidth < wall.GetMinX()) {
			toPosition.x /= 100;
		} else if (blockMaxCoord.x + halfOfWidth > wall.GetMaxX()) {
			toPosition.x /= 100;
		}
		if (blockMinCoord.z - halfOfWidth < wall.GetMinZ()) {
			toPosition.z /= 100;
		} else if (blockMaxCoord.z + halfOfWidth > wall.GetMaxZ()) {
			toPosition.z /= 100;
		}

		this.transform.position = Vector3.Lerp(fromPosition, toPosition, Time.deltaTime * speed);
	}

	/// pitch the block
	/// this method can decide forward direction via camera.
	/// @param direct - Vector3 forward or back
	public void PitchBlock(Vector3 direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Vector3 newDirect = VectorUtil.RoundXZ(direct);
		//Debug.Log(direct + " => " + newDirect + "; " + (Math.Abs(direct.x) >= Math.Abs(direct.z)) );
		if (Math.Abs(direct.x) >= Math.Abs(direct.z)) {
			Rotate(0, 0, -newDirect.x * 90);
		} else {
			Rotate(newDirect.z * 90, 0, 0);
		}
	}

	/// yaw the block
	/// @param direct - 1 or -1
	public void YawBlock(int direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Rotate(0, direct * 90, 0);
	}

	/// @param direct - Vector3.right or Vector3.left
	public void YawBlock(Vector3 direct) {
		if (direct == Vector3.right) {
			YawBlock(1);
		} else if (direct == Vector3.left) {
			YawBlock(-1);
		}
	}

	/// roll the block
	/// this method can decide right direction via camera.
	/// @param direct - Vector3 right or left
	public void RollBlock(Vector3 direct) {
		if (gameObject.name.CompareTo("block(new)") != 0) return;
		Vector3 newDirect = VectorUtil.RoundXZ(direct);
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

		/// send notification
		if (StartFalling != null) {
			StartFalling(this, EventArgs.Empty);
		}

		// after drop, OnCollisionEnter (private method) is called when landed on BlackPool.
	}

	/// return correct coordinate
	public Vector3 GetCorrectPosition() {
		Vector3 correctedPos = VectorUtil.RoundXZ(transform.position);
		return correctedPos;
	}

	/// Destroy child blocks
	public ArrayList DestroyChildBlocks() {
		var childCubes = from Transform cube in this.transform
			where cube.name == "Cube"
			select cube.gameObject;

		if (childCubes == null) return new ArrayList();

		// sum up child cube score to set into parent score
		this.gameObject.GetComponent<CubeInfo>().score +=
			childCubes
			.Select(cube => cube.GetComponent<CubeInfo>().score)
			.Sum();

		// store child cubes position
		ArrayList destroyPositions = new ArrayList();
		destroyPositions.AddRange(
			childCubes
			.Select(childCube => childCube.transform.position)
			.ToList()
		);

		// destroy child cubes
		childCubes
			.ToList()
			.ForEach(MonoBehaviour.Destroy);

		// perform event
		if (WhenDestroyChild != null) {
			WhenDestroyChild(this, EventArgs.Empty);
		}

		return destroyPositions;
	}

	public void GenerateSplash(Vector3 pos) {
		GameObject prefab = (GameObject)Resources.Load("Particle/Splash");
		GameObject splash =  MonoBehaviour.Instantiate(
			prefab, pos, Quaternion.identity
		) as GameObject;
		splash.AddComponent<SmokeController>();
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
			/// send notification
			if (StopFalling != null) {
				StopFalling(this, EventArgs.Empty);
			}

			// All jobs has finished. So destroy blockControl script.
			Destroy(this);
		}
	}

	/// move correct position and return it position
	/// @return corrected position
	private Vector3 CorrectPosition() {
		Vector3 correctedPos = VectorUtil.RoundXZ(transform.position);
		transform.position = correctedPos;
		return correctedPos;
	}

	/// return myself correct direction
	private Vector3 CorrectDirection(Vector3 currentPosition) {
		Vector3 correctedDir = VectorUtil.RoundXZ(currentPosition);
		return correctedDir;
	}

	/// set the block min-max x,y,z coordinate
	private void SetMinMaxCoord() {
		// init block min-max coordinate
		blockMinCoord = blockMaxCoord = transform.position;

		// set min-max
		foreach (Transform child in transform) {
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
		Wall wall = GetBlockPoolController().GetWall();
		if (blockMinCoord.x < wall.GetMinX()) {
			transform.Translate(Vector3.right, Space.World);
		} else if (blockMaxCoord.x > wall.GetMaxX()) {
			transform.Translate(Vector3.left, Space.World);
		}
		if (blockMinCoord.z < wall.GetMinZ()) {
			transform.Translate(Vector3.forward, Space.World);
		} else if (blockMaxCoord.z > wall.GetMaxZ()) {
			transform.Translate(Vector3.back, Space.World);
		}
	}
}
