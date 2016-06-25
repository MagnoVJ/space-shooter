using UnityEngine;
using System.Collections;

public class EnemyShootController : MonoBehaviour {

	public float fireRate;
	public float delay;
	public GameObject shot; 

	void Start(){

		InvokeRepeating("Fire", delay, fireRate);
	
	}

	void Fire(){

		Transform shotSpawn = GetChildGameObject(gameObject.transform.parent.gameObject, "Shot_Spawner").transform;

		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

	}

	static public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
	{
		Transform[] ts = fromGameObject.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}
}
