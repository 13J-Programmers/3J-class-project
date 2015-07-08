using UnityEngine;
using System.Collections;

public class MouseAction : MonoBehaviour {
	public GameObject soundBox;
	// Use this for initialization
	void Start () {
		//soundBox = GameObject.Find ("SoundBox");
	}
	private Ray ray;
	private RaycastHit hit;
	// Update is called once per frame
	void Update (){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		hit = new RaycastHit ();
		CommentAction ();
		TitleAction ();
		SceneTrans ();
	}
	public Animator commentAnimator; 
	public void CommentAction(){//コメントアクション
		bool f = false;
		if (Physics.Raycast (ray, out hit)) {
			//print (hit.collider.tag);
			if (hit.collider.tag == "Trigger") {
			//	print ("点滅中");
				f=true;
			}
		}
		commentAnimator.SetBool ("Touched",f);	
	}
	private Vector3 pos;
	public Animator titleAnimator; 
	public void TitleAction(){//タイトルアクション
		titleAnimator.SetBool ("Click", false);
		pos = Input.mousePosition;
		Vector3 title = GameObject.Find ("Title").GetComponent<RectTransform> ().transform.position;
		//print ("pos. x:" + title.x + "y:" + title.y);
		if (Input.GetMouseButtonDown (0)) {
			if (pos.x > title.x - 150 && pos.x < title.x + 150 && pos.y > title.y - 25 && pos.y < title.y + 25) {
				titleAnimator.SetBool ("Click", true);
				this.GetComponent<AudioSource>().volume=soundBox.GetComponent<AudioSource>().volume*1.2f;
				this.GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
				soundBox.GetComponent<SoundController>().ChangeBGM();
			}
		}
	}
	public string nextSceneName;
	public void SceneTrans(){//シーン切り替え
		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit)) {
				//print (hit.collider.tag);
				if (hit.collider.tag == GameObject.Find("EmptyObject").tag) {
					SoundSE ();
					GameObject.Find ("FedeSystem").GetComponent<Fade> ().LoadLevel (nextSceneName, 1f);
				}
			}
		}
	}
	public AudioClip startSE;
	public void SoundSE(){//シーン切り替え時のSE
		AudioSource audioSource = soundBox.GetComponent<AudioSource> ();
		audioSource.clip = startSE;
		audioSource.volume = soundBox.GetComponent<AudioSource>().volume/3f;
		audioSource.loop = false;
		audioSource.Play ();
	}
}
