using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	public AudioClip[] BGM;
	private AudioSource _audio;
	private int i = 0;

	// Use this for initialization
	void Start() {
		_audio = this.GetComponent<AudioSource>();
		_audio.clip = BGM[i];
		_audio.loop = true;
		_audio.Play();
	}

	// Update is called once per frame
	void Update() {

	}

	public void ChangeBGM(){
		if (i + 1 == BGM.Length) {
			i = 0;
		} else {
			i++;
		}
		_audio.clip = BGM[i];
		_audio.Play();
	}

	public AudioClip startSE;
	public void SoundSE() { // シーン切り替え時のSE
		_audio.clip = startSE;
		_audio.loop = false;
		_audio.Play();
	}
}
