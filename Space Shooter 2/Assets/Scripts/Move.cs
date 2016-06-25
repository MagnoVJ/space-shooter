using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	
	private Rigidbody rigidbody;

	public float speed;
	public float tumble;

	void Start () {

		rigidbody = GetComponent<Rigidbody>();

		if(!gameObject.CompareTag("Enemy"))
			rigidbody.angularVelocity = Random.insideUnitSphere * tumble;

		rigidbody.velocity = speed * Vector3.forward;
	
	}

	//Somente usado para destruir outro bolt
	void OnTriggerEnter(Collider other){
		if ((gameObject.CompareTag("BoltPlayer") || gameObject.CompareTag("BoltEnemy")) && (other.CompareTag("BoltPlayer") || other.CompareTag("BoltEnemy")))
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}  
	} 
 	
}
