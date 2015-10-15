using UnityEngine;
using System;
using System.Collections;

public class Sound : MonoBehaviour {
	public void Play() {
		if (this.GetComponent<AudioSource>().isPlaying) return;
		this.GetComponent<AudioSource>().Play();
	}
}
