using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// 手のひらを中央ブロックにmaxLoadTime秒間当てると切り替え
/// </summary>
public class TransByLeap : MonoBehaviour {
    private float time;//時間
    private float maxLoadTime;
    public Slider loadBar;
    public Animator OnAndOff;
    private bool f;
    private bool transFlag;
    void Start()
    {
        time = 0;
        f = false;
        maxLoadTime = 2;
        loadBar = GameObject.Find("LoadBar").GetComponent<Slider>();
        loadBar.maxValue = maxLoadTime;
        OnAndOff = GameObject.Find("LoadBar/LoadingText").GetComponent<Animator>();
    }

    void Update()
    {
        if (transFlag){
            if (!f)
                SceneTrans();
        }
        loadBar.value = time;//LoadBarに反映
    }

    void OnTriggerStay(Collider collider) //ずっと
    {
        if (!transFlag)
        {
            time += Time.deltaTime;
            OnAndOff.SetBool("Touched", true);
        }
        else
            OnAndOff.SetBool("Touched", false);
        //print("time:" + time);
        if (time > maxLoadTime)
            transFlag = true;
        else
            transFlag = false;
            
    }
    void OnTriggerExit(Collider collider) //離れた時
    {
        if (!f)
            time = 0;
        OnAndOff.SetBool("Touched", false);
    }
    public void SceneTrans() // シーン切り替え
    {
        f = true;
        GameObject.Find("SoundBox").GetComponent<SoundController>().SoundSE();
        GameObject.Find("FedeSystem").GetComponent<Fade>().LoadLevel("Main", 1f);
    }
}
