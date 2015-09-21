using UnityEngine;
using System.Collections;
/// <summary>
/// ジェスチャーによる操作(追加の可能性あり)
/// 1.手を握るとカメラが止まる
/// </summary>
public class HandGesture : MonoBehaviour {
    private CameraCircularMotion mainCameraMotion;
    private CameraCircularMotion secondCameraMotion;
    
    void Start () {
        mainCameraMotion = GameObject.Find("MainCamera").GetComponent<CameraCircularMotion>();
        secondCameraMotion = GameObject.Find("SecondCamera").GetComponent<CameraCircularMotion>();  
	}
	
	void Update () {
    }
    
    public void cameraStop(bool flag){
        mainCameraMotion.enabled = flag;
        secondCameraMotion.enabled = flag;
    }
}
