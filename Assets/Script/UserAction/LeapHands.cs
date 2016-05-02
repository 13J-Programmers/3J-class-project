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

	private const float rotateScale = 10;
	private const float upScale = 7;
	private const float downScale = -7;
	private const float rightScale = 7;
	private const float leftScale = -7;
	private const float counterClockwiseScale = 7;
	private const float clockwiseScale = -7;

	void Update() {
		Controller controller = new Controller();
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
		hand = hands[0];
		otherHand = hands[1];
	}

	public bool HasTwoHands() {
		return otherHand.IsValid;
	}

	public static bool IsGrabbing(Hand hand) {
		return (hand.GrabStrength >= 0.6);
	}

	/// check if the hand is horizontal
	public static bool IsHorizontal(Hand hand) {
		return LeapHands.IsHorizontal(hand, rotateScale);
	}

	/// IsHorizontal() can accept the rotate scale (default is 10)
	public static bool IsHorizontal(Hand hand, float rotateScale) {
		float pitch = hand.Direction.Pitch * rotateScale;
		float yaw   = hand.Direction.Yaw   * rotateScale;
		float roll  = hand.PalmNormal.Roll * rotateScale;

		if (upScale > pitch && pitch > downScale
				&& rightScale > yaw && yaw > leftScale
				&& counterClockwiseScale > roll && roll > clockwiseScale) {
			return true;
		}
		return false;
	}

	/// return true if given hand is pitching down.
	public static bool IsPitchingDown(Hand hand) {
		float pitch = hand.Direction.Pitch * rotateScale;
		return (pitch < downScale);
	}

	/// return true if given hand is pitching up.
	public static bool IsPitchingUp(Hand hand) {
		float pitch = hand.Direction.Pitch * rotateScale;
		return (pitch > upScale);
	}

	/// return true if given hand is yawing right.
	public static bool IsYawingRight(Hand hand) {
		float yaw = hand.Direction.Yaw * rotateScale;
		return (yaw > rightScale);
	}

	/// return true if given hand is yawing left.
	public static bool IsYawingLeft(Hand hand) {
		float yaw = hand.Direction.Yaw * rotateScale;
		return (yaw < leftScale);
	}

	/// return true if given hand is rolling right.
	public static bool IsRollingRight(Hand hand) {
		float roll = hand.PalmNormal.Roll * rotateScale;
		return (roll < clockwiseScale);
	}

	/// return true if given hand is rolling left.
	public static bool IsRollingLeft(Hand hand) {
		float roll = hand.PalmNormal.Roll * rotateScale;
		return (roll > counterClockwiseScale);
	}
}
