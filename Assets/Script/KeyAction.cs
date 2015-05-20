using UnityEngine;
using System.Collections;

public class KeyAction : MonoBehaviour {
	//A container for control target
	GameObject target;
	float x = 0.0f,z = 0.0f;
	BlockController control;
	
	// Use this for initialization
	void Start () {
		GameObject[] blocks = new GameObject[1];
		GameObject obj = Instantiate(blocks[1]);
		obj.name = "block";
		ConnectWithBlock();
		BlockController control = obj.GetComponent<BlockController>();
		
	}
	
	// Update is called once per frame
	void Update () {
		//Pitch Block
		if(Input.GetKeyDown("w")){
			control.PitchBlock(1);
		}
		else if(Input.GetKeyDown("s")){
			control.PitchBlock(-1);
		}

		//Yaw Block
		else if(Input.GetKeyDown("e")){
			control.YawBlock(1);
		}
		else if(Input.GetKeyDown("q")){
			control.YawBlock(-1);
		}

		//Roll Block
		else if(Input.GetKeyDown("d")){
			control.RollBlock(1);
		}
		else if(Input.GetKeyDown("a")){
			control.RollBlock(-1);
		}

		//Move Block
		if(Input.GetKey("up")){
			z+=0.5f;
			control.MoveBlock(x,z);
		}
		else if(Input.GetKey("down")){
			z-=0.5f;
			control.MoveBlock(x,z);
		}
		else if(Input.GetKey("right")){
			x+=0.5f;
			control.MoveBlock(x,z);
		}
		else if(Input.GetKey("left")){
			x-=0.5f;
			control.MoveBlock(x,z);
		}
	}

	public void ConnectWithBlock() {
		target = GameObject.Find("block");
	}
}