using UnityEngine;
using System.Collections;
using Leap;
/// <summary>
/// LeapMotionで手のひらを感知
/// </summary>
public class HandControl : MonoBehaviour {
	private HandGesture handGesture;
	private Controller controller;
	private InteractionBox interactionBox;
	private Frame frame;
	public int debugPlamCount;//手のひらの個数
	public int debugFingerCount;
	public float x, y, z;
	public GameObject[] fingerObject;

	void Start () {
		handGesture = GetComponent<HandGesture>();
		controller = new Controller();
	}

	void Update()
	{
		frame = controller.Frame();
		interactionBox = frame.InteractionBox;
		Hand hand = frame.Hands[0];
		debugPlamCount = frame.Hands.Count;
		debugFingerCount = frame.Fingers.Count;
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
		if (hand.GrabStrength > 0.6f)
			handGesture.cameraStop(false);
		else handGesture.cameraStop(true);
	}

	Vector3 ToVector3(Vector v)//座標の変換
	{
		return new UnityEngine.Vector3(v.x, v.y, v.z);
	}
}
