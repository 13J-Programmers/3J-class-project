///
/// @file  LeapHands.cs
/// @brief LeapMotion's helper methods
///

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

	public static bool IsGrabbing(Hand hand) {
		return (hand.GrabStrength >= 0.99);
	}

	public bool HasTwoHands() {
		return otherHand.IsValid;
	}
}
