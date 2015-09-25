using UnityEngine;
using System.Collections;

namespace Player.Action {
	public abstract class BaseAction : MonoBehaviour {
		protected abstract void Start();
		protected abstract void Update();
	}
}