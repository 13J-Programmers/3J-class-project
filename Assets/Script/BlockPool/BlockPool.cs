using UnityEngine;
using System.Collections;

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
		for (int z = 0; z < this.GetSizeZ(); z++) {
			for (int y = 0; y < this.GetSizeY(); y++) {
				bool isCompleted = true;
				for (int x = 0; x < this.GetSizeX(); x++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
				}
				if (!isCompleted) continue;

				// check completed row
				for (int x = 0; x < this.GetSizeX(); x++) {
					mustBeRemovedCubes[x, y, z] = true;
				}
			}
		}
		for (int x = 0; x < this.GetSizeX(); x++) {
			for (int y = 0; y < this.GetSizeY(); y++) {
				bool isCompleted = true;
				for (int z = 0; z < this.GetSizeZ(); z++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
				}
				if (!isCompleted) continue;

				// check completed row
				for (int z = 0; z < this.GetSizeZ(); z++) {
					mustBeRemovedCubes[x, y, z] = true;
				}
			}
		}
		return mustBeRemovedCubes;
	}


	private int CountCompletedRow(bool[,,] checkedPool) {
		int completedRowNum = 0;
		for (int z = 0; z < this.GetSizeZ(); z++) {
			for (int y = 0; y < this.GetSizeY(); y++) {
				bool isCompleted = true;
				for (int x = 0; x < this.GetSizeX(); x++) {
					if (checkedPool[x, y, z] == false) isCompleted = false;
				}
				if (!isCompleted) continue;

				completedRowNum++;
			}
		}
		for (int x = 0; x < this.GetSizeX(); x++) {
			for (int y = 0; y < this.GetSizeY(); y++) {
				bool isCompleted = true;
				for (int z = 0; z < this.GetSizeZ(); z++) {
					if (checkedPool[x, y, z] == false) isCompleted = false;
				}
				if (!isCompleted) continue;

				completedRowNum++;
			}
		}
		return completedRowNum;
	}
}
