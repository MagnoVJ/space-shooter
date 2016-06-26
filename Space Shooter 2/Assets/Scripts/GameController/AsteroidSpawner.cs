using UnityEngine;
using System.Collections;

//My namespaces
using NSState;

public class AsteroidSpawner : MonoBehaviour {

	public GameObject[] asteroids;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	void Start(){

		StartCoroutine("SpawnWaves");
	
	}

	void Update() {

		if ((GameController.actualState == State.GAMEOVER) || (GameController.actualState == State.PROXIMONIVEL))
			StopCoroutine("SpawnWaves");

	}

    IEnumerator SpawnWaves(){

		yield return new WaitForSeconds(startWait);

		while (true)
		{

			for (int i = 0; i < hazardCount; i++)
			{

				GameObject hazard = asteroids[Random.Range(0, asteroids.Length)];

				Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

				Quaternion spawnRotation;

				spawnRotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));

				Instantiate(hazard, spawnPosition, spawnRotation);

				yield return new WaitForSeconds(spawnWait);

			}

			yield return new WaitForSeconds(waveWait);

		}  
	
	}

}