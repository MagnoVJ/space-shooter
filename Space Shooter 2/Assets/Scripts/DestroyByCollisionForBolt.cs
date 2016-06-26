using UnityEngine;
using System.Collections;
using NSState;

//Essa classe é utilizada exclusivamente por BoltEnemy para indentificar a colisão
//entre BoltEnemy e BoltPlayer, caso aja essa colisão é chamado o UpdateScore para 
//dar 65 em pontuação para o player
public class DestroyByCollisionForBolt : MonoBehaviour {

	private GameController gameController;

	void Start(){

		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

	}

	void OnTriggerEnter(Collider other){

		if (other.CompareTag("BoltPlayer") && GameController.actualState == State.PLAYING) { 
			gameController.UpdateScorePlayer(gameObject);
			gameController.AnimationScoreText(gameObject);
		}

	}
	
}
