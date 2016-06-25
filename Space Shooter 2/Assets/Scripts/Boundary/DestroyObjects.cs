using UnityEngine;
using System.Collections;

public class DestroyObjects : MonoBehaviour{

	void OnTriggerExit(Collider other){
		Destroy(other.gameObject);
	}

}
