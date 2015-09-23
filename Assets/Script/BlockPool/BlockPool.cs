using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

///
/// three-dimensional array behaves pool of Tetoris
/// 
public class BlockPool {
	private int POOL_X; ///< width
	private int POOL_Y; ///< height
	private int POOL_Z; ///< depth
	public GameObject[,,] blockPool; ///< for storing position of each cubes

	public BlockPool(int width, int height, int depth) {
		POOL_X = width;
		POOL_Y = height;
		POOL_Z = depth;
		blockPool = new GameObject[POOL_X, POOL_Y, POOL_Z];
	}

	public int GetSizeX() { return this.POOL_X; }
	public int GetSizeY() { return this.POOL_Y; }
	public int GetSizeZ() { return this.POOL_Z; }
	public GameObject GetGameObject(int x, int y, int z) {
		return blockPool[x, y, z];
	}
	public void SetGameObject(int x, int y, int z, GameObject gameObj) {
		blockPool[x, y, z] = gameObj;
	}

	public void Init() {
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					this.SetGameObject(x, y, z, null);
				}
			}
		}
	}

	/// TODO: need refactor
	///
	/// remove completed row
	///
	/// steps:
	///   1. Marking cubes of every completed row. (A)
	///   2. Marking cubes which above them. (B)
	///   3. Cubes (B) belong to a Dummy parent
	///   4. Add RigitBody to the Dummy parent
	///   5. Remove (A)
	///   6. Rid RigitBody from the Dummy parent
	///   7. After cubes (B) landed, 
	///      cubes (B) independent of the Dummy parent.
	///   8. jump to first.
	///
	public bool RemoveCompletedRow() {
		_DummyParent dummyParent = GameObject.Find("_DummyParent").GetComponent<_DummyParent>();
		GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		bool[,,] willBeRemovedCube = SearchCubeThatMustBeRemoved();
		int removeRowNum = CountCompletedRow(willBeRemovedCube);
		bool hasCompletedRow = (removeRowNum > 0) ? true : false;
		
		if (!hasCompletedRow) return false;

		// add num of rows to display
		gameManager.lines += removeRowNum;

		// mark cubes above completed row
		for (int z = 0; z < POOL_Z; z++) {
			for (int x = 0; x < POOL_X; x++) {
				bool hasRemoveCube = false;
				for (int y = 0; y < POOL_Y; y++) {
					if (blockPool[x, y, z] == null) continue;

					if (willBeRemovedCube[x, y, z] == true) {
						hasRemoveCube = true;
						continue;
					}

					if (hasRemoveCube) {
						blockPool[x, y, z].transform.parent = dummyParent.transform;
					}
				}
			}
		}

		// destroy completed row
		int cubeScore = 0;
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				for (int x = 0; x < POOL_X; x++) {
					if (willBeRemovedCube[x, y, z] == true) {
						cubeScore += blockPool[x, y, z].GetComponent<CubeInfo>().score;
						MonoBehaviour.Destroy(blockPool[x, y, z]);
					}
				}
			}
		}

		// add game-score to display
		gameManager.score += cubeScore * removeRowNum;

		// start to drop dummyParent
		dummyParent.StartDropping();

		return true;
	}

	private bool[,,] SearchCubeThatMustBeRemoved() {
		bool[,,] mustBeRemovedCubes = new bool[this.GetSizeX(), this.GetSizeY(), this.GetSizeZ()];

		Array3D<GameObject> pool = new Array3D<GameObject>();
		pool.SetArray3D(blockPool);
		Array3D<bool> checkedPool = new Array3D<bool>();
		checkedPool.SetArray3D(mustBeRemovedCubes);

		for (int z = 0; z < this.GetSizeZ(); z++)
			for (int y = 0; y < this.GetSizeY(); y++)
				if (pool.TakeXRow(y, z).All(e => e != null))
					checkedPool.SetXRow(y, z, true);

		for (int x = 0; x < this.GetSizeX(); x++)
			for (int y = 0; y < this.GetSizeY(); y++)
				if (pool.TakeZRow(x, y).All(e => e != null))
					checkedPool.SetZRow(x, y, true);

		return checkedPool.GetArray3D();
	}


	private int CountCompletedRow(bool[,,] checkedPool) {
		int completedRowNum = 0;

		Array3D<bool> pool = new Array3D<bool>();
		pool.SetArray3D(checkedPool);

		for (int z = 0; z < this.GetSizeZ(); z++)
			for (int y = 0; y < this.GetSizeY(); y++)
				if (pool.TakeXRow(y, z).All(e => e == true))
					completedRowNum++;

		for (int x = 0; x < this.GetSizeX(); x++)
			for (int y = 0; y < this.GetSizeY(); y++)
				if (pool.TakeZRow(x, y).All(e => e == true))
					completedRowNum++;

		return completedRowNum;
	}
}



public class Array3D<T> {
	private T[,,] array = new T[0, 0, 0];
	public int GetSizeX() { return this.array.GetLength(0); }
	public int GetSizeY() { return this.array.GetLength(1); }
	public int GetSizeZ() { return this.array.GetLength(2); }

	// array copy to member array
	public void SetArray3D(T[,,] array) {
		this.array = new T[array.GetLength(0), array.GetLength(1), array.GetLength(2)];
		for (int x = 0; x < array.GetLength(0); x++)
			for (int y = 0; y < array.GetLength(1); y++)
				for (int z = 0; z < array.GetLength(2); z++)
					this.array[x, y, z] = array[x, y, z];
	}

	public T[,,] GetArray3D() {
		return this.array;
	}

	public T[] TakeXRow(int specY, int specZ) {
		T[] reduceArray = new T[this.GetSizeX()];
		for (int x = 0; x < this.GetSizeX(); x++) {
			reduceArray[x] = array[x, specY, specZ];
		}
		return reduceArray;
	}

	public T[] TakeZRow(int specX, int specY) {
		T[] reduceArray = new T[this.GetSizeZ()];
		for (int z = 0; z < this.GetSizeZ(); z++) {
			reduceArray[z] = array[specX, specY, z];
		}
		return reduceArray;
	}

	public void SetXRow(int specY, int specZ, T value) {
		for (int x = 0; x < this.GetSizeX(); x++) {
			array[x, specY, specZ] = value;
		}
	}

	public void SetZRow(int specX, int specY, T value) {
		for (int z = 0; z < this.GetSizeZ(); z++) {
			array[specX, specY, z] = value;
		}
	}
}
