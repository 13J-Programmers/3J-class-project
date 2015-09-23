using UnityEngine;
using System.Collections;

///
/// three-dimensional array behaves pool of Tetoris
/// 
public class BlockPool {
	public int POOL_X; ///< width
	public int POOL_Y; ///< height
	public int POOL_Z; ///< depth
	/// for storing position of each cubes
	public GameObject[,,] blockPool;// = new GameObject[POOL_X, POOL_Y, POOL_Z];

	public BlockPool(int width, int height, int depth) {
		POOL_X = width;
		POOL_Y = height;
		POOL_Z = depth;
		blockPool = new GameObject[POOL_X, POOL_Y, POOL_Z];
	}

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

		bool hasCompletedRow = false;
		int removeRowNum = 0;
		bool[,,] willRemoveCube = new bool[POOL_X, POOL_Y, POOL_Z];
		bool[,,] onRemoveCube   = new bool[POOL_X, POOL_Y, POOL_Z];

		// check completed row
		for (int z = 0; z < POOL_Z; z++) {
			for (int y = 0; y < POOL_Y; y++) {
				bool isCompleted = true;
				for (int x = 0; x < POOL_X; x++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
				}
				if (!isCompleted) continue;

				hasCompletedRow = true;
				removeRowNum++;

				// check completed row
				for (int x = 0; x < POOL_X; x++) {
					willRemoveCube[x, y, z] = true;
				}
			}
		}
		for (int x = 0; x < POOL_X; x++) {
			for (int y = 0; y < POOL_Y; y++) {
				bool isCompleted = true;
				for (int z = 0; z < POOL_Z; z++) {
					if (blockPool[x, y, z] == null) isCompleted = false;
				}
				if (!isCompleted) continue;

				hasCompletedRow = true;
				removeRowNum++;

				// check completed row
				for (int z = 0; z < POOL_Z; z++) {
					willRemoveCube[x, y, z] = true;
				}
			}
		}
		
		if (!hasCompletedRow) return false;

		// add num of rows to display
		gameManager.lines += removeRowNum;

		// mark cubes above completed row
		for (int z = 0; z < POOL_Z; z++) {
			for (int x = 0; x < POOL_X; x++) {
				bool hasRemoveCube = false;
				for (int y = 0; y < POOL_Y; y++) {
					if (blockPool[x, y, z] == null) continue;

					if (willRemoveCube[x, y, z] == true) {
						hasRemoveCube = true;
						continue;
					}

					if (hasRemoveCube) {
						onRemoveCube[x, y, z] = true;
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
					if (willRemoveCube[x, y, z] == true) {
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
}
