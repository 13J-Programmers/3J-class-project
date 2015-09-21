using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// 手のひらを中央ブロックにmaxLoadTime秒間当てると切り替え
/// </summary>
public class TransByLeap : MonoBehaviour {
    private float time;//時間
    private float maxLoadTime;
    public GameObject loadBar;
    public Animator OnAndOff;
    private bool f;
    private bool transFlag;
    private GameObject mainCamera;
    private GameObject emptyObject;
    private Canvas canvas;
    void Start()
    {
        time = 0;
        f = false;
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
            if (!f)
                SceneTrans();
        }
        loadBar.GetComponent<Slider>().value = time;//LoadBarに反映
        
    }

    void OnTriggerStay(Collider collider) //ずっと
    {
        
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
    void OnTriggerExit(Collider collider) //離れた時
    {
        if (!f)
        {
            time = 0;
            canvas.enabled = false;
        }
        OnAndOff.SetBool("Touched", false);
    }
    public void SceneTrans() // シーン切り替え
    {
        f = true;
        GameObject.Find("SoundBox").GetComponent<SoundController>().SoundSE();
        GameObject.Find("FedeSystem").GetComponent<Fade>().LoadLevel("Main", 1f);
    }
}
