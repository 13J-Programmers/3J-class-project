using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Array3D<T> {
	private T[,,] array = new T[0, 0, 0];
	public int GetSizeX() { return this.array.GetLength(0); }
	public int GetSizeY() { return this.array.GetLength(1); }
	public int GetSizeZ() { return this.array.GetLength(2); }

	// array copy to member array
	public Array3D<T> Set(T[,,] array) {
		this.array = new T[array.GetLength(0), array.GetLength(1), array.GetLength(2)];
		for (int x = 0; x < array.GetLength(0); x++)
			for (int y = 0; y < array.GetLength(1); y++)
				for (int z = 0; z < array.GetLength(2); z++)
					this.array[x, y, z] = array[x, y, z];

		return this;
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