using UnityEngine;
using System.Collections;

public class CameraSystem  : MonoBehaviour {
	public GameObject target;
	public float rad; // 半径
	public float hight; // 高さ
	public float angle; // ラジアン値
	public float speed;
	private bool circularMotionFlag;
	// Use this for initialization
	void Start() {
		circularMotionFlag = true;
	}

	// Update is called once per frame
	void Update() {
		if (circularMotionFlag)
		{
			float time = Time.deltaTime;
			Vector3 CameraPos = target.transform.position;
			// オブジェクトの周りをカメラが円運動する
			transform.position = new Vector3(
				CameraPos.x + Mathf.Cos(angle) * rad,
				CameraPos.y + hight,
				CameraPos.z + Mathf.Sin(angle) * rad
			);
			angle += speed * time;
			//print("MainCamera x:" + this.transform.position.x);
			//print("MainCamera y:" + this.transform.position.y);
			//print("MainCamera z:" + this.transform.position.z);
			transform.LookAt(CameraPos); // カメラをtargetへ向かせる
		}
	}
	public void StopOrMove(bool moveFlag)//カメラを停止または動かす
	{
		circularMotionFlag = moveFlag;
	}
	public void MoveUpOrDown(int moveMode)//カメラを上または下に移動
	{
		Animator animator = GetComponent<Animator>();
		switch (moveMode)
		{
			case 1://FromTopToBottom
				if (animator.GetInteger("MoveMode") == 2 || animator.GetInteger("MoveMode") == 0)
					animator.SetInteger("MoveMode", moveMode);
				break;
			case 2://FromBottomToTop
				if (animator.GetInteger("MoveMode") == 1 || animator.GetInteger("MoveMode") == 0)
					animator.SetInteger("MoveMode", moveMode);
				break;
		}
	}
}
