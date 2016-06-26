using UnityEngine;
using System.Collections;
using NSState;

public class DestroyByCollision : MonoBehaviour {

	private GameController gameController;

	public GameObject explosion;

	void Start(){

		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	
	}

	void OnTriggerEnter(Collider other) {

		if (GameController.actualState == State.PLAYING) { 

			//Verifica se o colisão está sendo feita com o Boundary e se o objeto atual é um Enemy e o outro objeto é um BoltEnemy ou se o outro objeto é um Enemy se for verdadeiro a destruição por colisão deve ser ignorada
			if (other.tag != "Boundary" && !((gameObject.CompareTag("Enemy") && other.CompareTag("BoltEnemy")) || (gameObject.CompareTag("Enemy") && other.CompareTag("Enemy")))) {

				//Verifica se a colisão está sendo feita entre Enemy e Asteroid, também caso verdadeiro a colisão deve ser ignorada
				if (!((gameObject.CompareTag("Asteroid") && other.CompareTag("Asteroid")) || (gameObject.CompareTag("Enemy") && other.CompareTag("Asteroid")) || (gameObject.CompareTag("Asteroid") && other.CompareTag("Enemy")))) {

					//Verifica se a colisão está sendo feita entre Asteroid e BoltEnemy nesse caso a colisão deve ser ignorada
					if (!(gameObject.CompareTag("Asteroid") && other.CompareTag("BoltEnemy"))) {

						//Verifica se o gameObject é Player ou se o other é Player e está imune
						if (!((gameObject.CompareTag("Player") && (gameObject.GetComponent<PlayerImmunity>().immune == true)) || (other.CompareTag("Player") && (other.transform.parent.gameObject.GetComponent<PlayerImmunity>().immune == true)))){

							if (other.CompareTag("Player") || (gameObject.CompareTag("Player") && other.CompareTag("BoltEnemy"))) 
								gameController.ArisePlayer();

							if ((gameObject.CompareTag("Enemy") || gameObject.CompareTag("Asteroid")) && other.CompareTag("BoltPlayer")) { 
								gameController.UpdateScorePlayer(gameObject);
								gameController.AnimationScoreText(gameObject);
							}

							Instantiate(explosion, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
							Destroy(other.gameObject);
							Destroy(gameObject);

						}
					}
				}
			}
		}
	}

}