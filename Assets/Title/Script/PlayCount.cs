using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayCount : PlayCountSystem
{
	void Start () {
		load();
		this.GetComponent<Text>().text = playCount.ToString();
	}
}
