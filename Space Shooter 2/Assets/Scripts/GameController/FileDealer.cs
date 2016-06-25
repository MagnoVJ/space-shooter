using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class FileDealer : MonoBehaviour {

	public void Save(List<int> scoreVector){

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/scoreInfo.dat");

		ScoreClassToBeSaved data = new ScoreClassToBeSaved();

		data.scores = scoreVector;

		bf.Serialize(file, data);

		file.Close();

	}

	public List<int> Load(){

		if (File.Exists(Application.persistentDataPath + "/scoreInfo.dat")){

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/scoreInfo.dat", FileMode.Open);

			ScoreClassToBeSaved data = (ScoreClassToBeSaved)bf.Deserialize(file);

			file.Close();

			return data.scores;

		}
		else
			return null;

	}
}

[Serializable]
public class ScoreClassToBeSaved{
	public List<int> scores = new List<int>(5);
}