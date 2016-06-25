using UnityEngine;
using System.Collections;

//My namespaces
using NSState;

public class Enemy01Spawner : MonoBehaviour {

	public GameObject[] enemies;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	void Start () {

		StartCoroutine("SpawnWaves");
	
	}

	void Update() {

		if ((gameObject.GetComponent<GameController>().actualState == State.GAMEOVER) || (gameObject.GetComponent<GameController>().actualState == State.PROXIMONIVEL))
			StopCoroutine("SpawnWaves");

	}
	
	
	IEnumerator SpawnWaves(){

		yield return new WaitForSeconds(startWait);

		while (true) {

			for (int i = 0; i < hazardCount; i++) {

				GameObject hazard = enemies[Random.Range(0, enemies.Length)];

				Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

				Quaternion spawnRotation;

				spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

				Instantiate(hazard, spawnPosition, spawnRotation);

				yield return new WaitForSeconds(spawnWait);
			
			}

			yield return new WaitForSeconds(waveWait);
		
		} 
	
	}

}
