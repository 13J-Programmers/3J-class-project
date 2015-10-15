using UnityEngine;
using System.Collections;
using System.IO;
using System;
public class PlayCountSystem : MonoBehaviour
{
	public int playCount;
	private FileInfo file;

	public void increasePlayNum()
	{
		StreamWriter write = file.CreateText();//上書きモード
		write.Write(++playCount);
		write.Flush();
		write.Close();
	}
	public void load(){//ファイルからデータをロード
		file = new FileInfo(Application.dataPath + "/" + "PlayCount.csv");
		StreamReader read = file.OpenText();
		string count = read.ReadLine().ToString();
		playCount = int.Parse(count);
		Debug.Log("Load:" + playCount);
		read.Close();
	}
}
