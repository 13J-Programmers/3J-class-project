using UnityEngine;
using System.Collections;
using Leap;

/// <summary>
/// LeapMotionで手のひらを感知
/// ジェスチャーによる操作
/// 1.片手で握るとカメラが止まる
/// 2.両手で広げる、または閉じるとカメラの位置が変わる(速度で判断)
/// </summary>
public class HandControl : MonoBehaviour {
	private Controller controller;
	private InteractionBox interactionBox;
	private Frame frame;
	private CameraSystem cameraSystem;
	public int plamCount;
	public int debugFingerCount;
	public float x, y, z;
	private GameObject plam;
	private bool existPlamFlag;
	void Start()
	{
		cameraSystem = GameObject.Find("MainCamera").GetComponent<CameraSystem>();  
		controller = new Controller();
		existPlamFlag = false;
	}

	void Update()
	{
		frame = controller.Frame();
		interactionBox = frame.InteractionBox;
		Hand hand = frame.Hands[0];
		plamCount = frame.Hands.Count;
		debugFingerCount = frame.Fingers.Count;
		Vector plamPos = interactionBox.NormalizePoint(hand.PalmPosition);
		int scale = 4;//感知する領域を広げる
		x = scale * (plamPos.x - 0.5f);
		y = 7 * plamPos.y;
		z = scale * (plamPos.z - 0.5f);
		if (plamCount <= 1)//片手の場合
		{
			if (hand.IsValid == false && existPlamFlag)
			{
				existPlamFlag = false;
				Destroy(plam);
			}
			if (!existPlamFlag)
			{
				if (hand.IsValid)
				{
					existPlamFlag = true;
					plam = (GameObject)Instantiate(Resources.Load("Title/Plam"), ToVector3(new Vector(x, y, -z)), Quaternion.identity);
					plam.transform.SetParent(GameObject.Find("MainCamera/LeapPos").transform);
					plam.GetComponent<Renderer>().enabled = hand.IsValid;
					plam.GetComponent<Collider>().enabled = hand.IsValid;
				}
			}
			else
			{
				plam.transform.localPosition = ToVector3(new Vector(x, y, -z));
			}
			HoldDecision(hand);
		}
		else if (plamCount >= 2)//両手の場合
		{
			existPlamFlag = false;
			Destroy(plam);
			BothHandsGesture();
		}
	}
	private void HoldDecision(Hand hand)//握ったかどうかの判断
	{
		if (hand.GrabStrength > 0.6f)
			cameraSystem.StopOrMove(false);
		else
			cameraSystem.StopOrMove(true);
	}
	private void BothHandsGesture()//両手のジェスチャーの判断
	{
		Hand leftMost = frame.Hands.Leftmost;
		Hand rightMost = frame.Hands.Rightmost;
		if (leftMost.Id != rightMost.Id)
		{
			//Debug.Log(leftMost.PalmVelocity + ":" + rightMost.PalmVelocity);
			if (leftMost.PalmVelocity.x <= -500 && rightMost.PalmVelocity.x >= 500)//|5*10^-3|m/s到達時//拡大
			{
				//Debug.Log("カメラ移動(下から上)" + leftMost.PalmVelocity.x);
				cameraSystem.MoveUpOrDown(1);
			}
			else if (leftMost.PalmVelocity.x >= 300 && rightMost.PalmVelocity.x <= -300)//|3*10^-3|m/s到達時//縮小
			{
				//Debug.Log("Okカメラ移動(上から下)" + leftMost.PalmVelocity.x);
				cameraSystem.MoveUpOrDown(2);
			}
		}
	}
	Vector3 ToVector3(Vector v)//座標の変換
	{
		return new UnityEngine.Vector3(v.x, v.y, v.z);
	}
}
