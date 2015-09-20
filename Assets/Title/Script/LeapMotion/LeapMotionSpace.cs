using UnityEngine;
using System.Collections;

public class LeapMotionSpace : MonoBehaviour {
    /// <summary>
    /// LeapMotionの中心座標の設定
    /// </summary>
    private GameObject target;

	void Start () {
        target = GameObject.Find("EmptyObject");
	}

	void Update ()
    {
        Vector3 thisPos = target.transform.localPosition;
        transform.position = thisPos;
    }
}
