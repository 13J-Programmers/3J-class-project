using UnityEngine;
using System.Collections;
using Leap;
/// <summary>
/// LeapMotionで手のひらを感知
/// </summary>
public class HandControl : MonoBehaviour {
    private Controller controller;
    public int debugPlamCount;//手のひらの個数
    public float x, y, z;
   
	void Start () {
        controller = new Controller();
	}

    void Update()
    {
        Frame frame = controller.Frame();
        InteractionBox interactionBox = frame.InteractionBox;
        Hand hand = frame.Hands[0];
        debugPlamCount = frame.Hands.Count;
        this.GetComponent<Renderer>().enabled = hand.IsValid;
        this.GetComponent<Collider>().enabled = hand.IsValid;
        Vector plamPos = interactionBox.NormalizePoint(hand.PalmPosition);
        int a = 4;//感知する領域を広げる
        x = a * (plamPos.x - 0.5f);
        y = a * plamPos.y;
        z = a * (plamPos.z - 0.5f);
        Vector p = new Vector(x, y, z);
        this.transform.localPosition = ToVector3(p);
        Vector3 pos = this.transform.localPosition;
        pos.z = -pos.z;
        this.transform.localPosition = pos;
    }
    Vector3 ToVector3(Vector v)//座標の変換
    {
        return new UnityEngine.Vector3(v.x, v.y, v.z);
    }
} 
