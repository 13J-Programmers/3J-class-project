using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// 手のひらを中央ブロックにmaxLoadTime秒間当てるとMainSceneに移行
/// </summary>
public class TransByLeap : MonoBehaviour {
	private float time;//時間
	private float maxLoadTime;
	private GameObject loadBar;
	private Animator OnAndOff;
	private bool sceneTransFlag;
	private bool transFlag;
	private bool onTriggerStayFlag;
	private GameObject mainCamera;
	private GameObject emptyObject;
	private Canvas canvas;

	void Start()
	{
		time = 0;
		sceneTransFlag = false;
		onTriggerStayFlag = false;
		maxLoadTime = 1.5f;
		loadBar = GameObject.Find("LoadBar");
		loadBar.GetComponent<Slider>().maxValue = maxLoadTime;
		OnAndOff = GameObject.Find("LoadBar/LoadingText").GetComponent<Animator>();
		mainCamera = GameObject.Find("MainCamera");
		emptyObject = GameObject.Find("EmptyObject");
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		canvas.enabled = transFlag;
	}

	void Update()
	{
		if (transFlag){
			if (!sceneTransFlag)
				SceneTrans();
		}
		float checkTime = time;
		if (checkTime != loadBar.GetComponent<Slider>().value && onTriggerStayFlag)
			loadBar.GetComponent<Slider>().value = time;//LoadBarに反映
		else
		{
			onTriggerStayFlag = false;
			if (!sceneTransFlag)
			{
				time = 0;
				canvas.enabled = false;
			}
			OnAndOff.SetBool("Touched", false);
		}
	}

	void OnTriggerStay(Collider collider) //ずっと
	{
		onTriggerStayFlag = true;
		if (!transFlag)
		{
			time += Time.deltaTime;
			OnAndOff.SetBool("Touched", true);
			Vector3 screenPoint = mainCamera.GetComponent<Camera>().WorldToScreenPoint(emptyObject.transform.position);
			loadBar.transform.position = screenPoint;
		}
		else
			OnAndOff.SetBool("Touched", false);
		//print("time:" + time);
		if (time > maxLoadTime)
		{
			transFlag = true;
			GameObject loadingText = GameObject.Find("LoadingText");
			loadingText.GetComponent<Text>().text = "O K !";
		}
		else
			transFlag = false;
		canvas.enabled = true;
	}
	public void SceneTrans() // シーン切り替え
	{
		sceneTransFlag = true;
		GameObject.Find("SoundBox").GetComponent<SoundController>().SoundSE();
		GameObject.Find("FadeSystem").GetComponent<Fade>().LoadLevel("Main", 1f);
	}
}
