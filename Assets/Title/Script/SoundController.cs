using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	public AudioClip[] BGM;
	private AudioSource _audio;
	private int i = 0;

	// Use this for initialization
	void Start() {
		if (BGM.Length != 0)
		{
			_audio = this.GetComponent<AudioSource>();
			_audio.clip = BGM[i];
			_audio.loop = true;
			_audio.Play();
		}
	}

	public void ChangeBGM(){//BGMの選択
		if (BGM.Length >= 2)//BGMが１つ以上の場合
		{
			if (i + 1 == BGM.Length)
			{
				i = 0;
			}
			else
			{
				i++;
			}
			_audio.clip = BGM[i];
			_audio.Play();
		}
	}

	public AudioClip startSE;
	public void SoundSE() { // シーン切り替え時のSE
		_audio.clip = startSE;
		_audio.loop = false;
		_audio.Play();
	}
}
