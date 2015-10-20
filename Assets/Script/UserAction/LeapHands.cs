using UnityEngine;
using System;
using System.Collections;
using Leap;

public class LeapHands : MonoBehaviour {
	public Hand hand      { get; private set; }
	public Hand otherHand { get; private set; }
	
	void Update() {
		Controller controller = new Controller();
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
		hand = hands[0];
		otherHand = hands[1];
	}

	public static bool IsFingersFolded(Hand hand) {
		Vector origin = hand.PalmPosition;
		float dist = 0;

		// sum up fingers distance from position of palm
		foreach (Finger finger in hand.Fingers) {
			// ignore player's thumb
			if (finger.Type() == Finger.FingerType.TYPE_THUMB) continue;
			dist += finger.TipPosition.DistanceTo(origin);
		}
		return (dist < 280) ? true : false;
	}

	public bool HasTwoHands() {
		return otherHand.IsValid;
	}
}
