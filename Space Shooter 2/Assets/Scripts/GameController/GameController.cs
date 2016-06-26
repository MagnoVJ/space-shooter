using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

//My namespaces
using NSState;

namespace NSState{

	public enum State { PLAYING, GAMEOVER, PROXIMONIVEL};
}

/* Classe responsável pelo gerenciamento geral do jogo*/

public class GameController : MonoBehaviour{

	public static State actualState = State.PLAYING;
	
	private Rigidbody rigidbody;
	private Camera camera;
	private GameObject canvas;
	private GameObject gameOverBackground;
	private GameObject proximoNivelBackground;
	private GameObject pausedBackground;
	private const float TIMEFORTHETEXTSCOREEXISTENCE = 0.5f;
	
	private int life;
	private int score;
	private bool pausado;

	//Vetor responsável por cada animação presente na scena
	private List<TextAnimation> textAnimationVector;

	public int maxLife;
	public float velocityBGScroller;
	public int levelDurationInSeconds;
	public GameObject bgParant;
	public GameObject lifeBar;
	public GameObject progressionTube;
	public GameObject scoreBar;
	public GameObject player;

	//Texts animation score
	public GameObject sAFAsteroid;

	void Start() {

		actualState = State.PLAYING;

		life = maxLife;
		score = 0;
		pausado = false;
		textAnimationVector = new List<TextAnimation>();

		GetChildGameObject(lifeBar, "LifeTube").GetComponent<Slider>().maxValue = maxLife;
		GetChildGameObject(lifeBar, "LifeTube").GetComponent<Slider>().value = life;

		progressionTube.GetComponent<Slider>().maxValue = levelDurationInSeconds;

		gameOverBackground = GameObject.FindGameObjectWithTag("GameOverBackground");
		proximoNivelBackground = GameObject.FindGameObjectWithTag("ProximoNivelBackground");
		pausedBackground = GameObject.FindGameObjectWithTag("PausedBackground");

		gameOverBackground.SetActive(false);
		proximoNivelBackground.SetActive(false);
		pausedBackground.SetActive(false);

		GetChildGameObject(scoreBar, "ScoreText").GetComponent<Text>().text = score.ToString().PadLeft(6, '0');

		rigidbody = bgParant.GetComponent<Rigidbody>();
		camera = Camera.main;
		canvas = GameObject.FindGameObjectWithTag("Canvas");
		rigidbody.velocity = velocityBGScroller * new Vector3(0.0f, 0.0f, 1.0f);

	}

	void Update(){

		ReplaceBackground();

		if (progressionTube.GetComponent<Slider>().value == progressionTube.GetComponent<Slider>().maxValue) {

			if(GameObject.Find("Player Ship") != null)
				GameObject.Find("Player Ship").GetComponent<PlayerImmunity>().immune = true;

			if (actualState == State.PLAYING) { 

				proximoNivelBackground.SetActive(true);

				ScoreMananger(proximoNivelBackground);

				GetChildGameObject(proximoNivelBackground, "Proximo Pontuacao").GetComponent<Text>().text = "Pontuação: " + score.ToString().PadLeft(6, '0');
				
				actualState = State.PROXIMONIVEL;

			}

		}

		if (GetLife() <= 0 && actualState == State.PLAYING) { 

			GameOverMethod();

			//Gerencia a persitencia do score do player
			ScoreMananger(gameOverBackground);

		}

		////Incremento da barra de tempo
		if (actualState != State.GAMEOVER){
			if (progressionTube.GetComponent<Slider>().value < progressionTube.GetComponent<Slider>().maxValue && Time.timeScale == 1)
				progressionTube.GetComponent<Slider>().value += Time.fixedDeltaTime * (Time.deltaTime / Time.fixedDeltaTime);
		}

	}

	void FixedUpdate() {

		for (int i = 0; i < textAnimationVector.Count; i++)
			textAnimationVector[i].safText.transform.localScale += new Vector3(0.05f, 0.05f, 0.0f);

	}

	//Essa função reposiciona o imagem do background para a ultima posicão (uiltimo filho) caso o position.z da imagem seja <= 0 
	void ReplaceBackground(){

		if(bgParant.transform.position.z <= 0){

			Transform[] transInParant = bgParant.GetComponentsInChildren<Transform>();

			int quantOfChildren = transInParant.Length - 1;

			float newZPosit = bgParant.transform.lossyScale.y * quantOfChildren;
			
			bgParant.transform.position = new Vector3(bgParant.transform.position.x, bgParant.transform.position.y, newZPosit);

		}

	}

	//Esse metodo é chamdo por DestroyByCollision quando o player é destruido
	//A vida do player deverá ser decrementada
	//O player deverá ser revivido no origem do mundo (caso não seja gameover)
	public void ArisePlayer() {

		life--;

		GetChildGameObject(lifeBar, "LifeTube").GetComponent<Slider>().value = life;

		if (life > 0) { 

			GameObject playerOB = (GameObject)Instantiate(player, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f));

			playerOB.GetComponent<PlayerInput>().touchPad = GetChildGameObject(canvas, "Movement Zone").GetComponent<SimpleTouchPad>();
			playerOB.GetComponent<PlayerInput>().areaButton = GetChildGameObject(canvas, "Fire Zone").GetComponent<SimpleTouchAreaButton>();

		}
	
	}

	//Esse metodo é chamado por DestroyByCollision quanto o player atinge um hazard
	//Caso o hazard seja um Asteroid o score deve ser incrementado em 50
	//Caso o hazard seja um EnemyShip o score deve ser incrementado em 60
	//Caso o hazard seja um BoltEnemy o score deve ser incrementado em 75
	public void UpdateScorePlayer(GameObject hazard){

		if (hazard.CompareTag("Asteroid"))
			score += 50;
		else if (hazard.CompareTag("Enemy"))
			score += 60;
		else if (hazard.CompareTag("BoltEnemy"))
			score += 75;

		if (score > 999999)
			score = 999999;

		GetChildGameObject(scoreBar, "ScoreText").GetComponent<Text>().text = score.ToString().PadLeft(6, '0');

	
	}

	public void AnimationScoreText(GameObject hazard) {

		if (hazard.CompareTag("Asteroid")){

			TextAnimation tmpTA = new TextAnimation();

			tmpTA.safText = (GameObject)Instantiate(sAFAsteroid, camera.WorldToScreenPoint(new Vector3(hazard.transform.position.x + 20 * Time.fixedDeltaTime, hazard.transform.position.y, hazard.transform.position.z + 20 * Time.fixedDeltaTime)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
			tmpTA.safText.GetComponent<Text>().text = "+50";
			tmpTA.safText.GetComponent<Text>().color = new Color(231.0f / 255.0f, 231.0f / 255.0f, 231.0f / 255.0f);
			tmpTA.safText.transform.SetParent(canvas.transform);

			StartCoroutine("LifeOfTextAnimationCoroutine", tmpTA);
			
		}
		else if (hazard.CompareTag("Enemy")) {

			TextAnimation tmpTA = new TextAnimation();

			tmpTA.safText = (GameObject)Instantiate(sAFAsteroid, camera.WorldToScreenPoint(new Vector3(hazard.transform.position.x + 20 * Time.fixedDeltaTime, hazard.transform.position.y, hazard.transform.position.z + 20 * Time.fixedDeltaTime)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
			tmpTA.safText.GetComponent<Text>().text = "+60";
			tmpTA.safText.GetComponent<Text>().color = new Color(123.0f / 255.0f, 203.0f / 255.0f, 121.0f / 255.0f);
			tmpTA.safText.transform.SetParent(canvas.transform);

			StartCoroutine("LifeOfTextAnimationCoroutine", tmpTA);
		
		}
		else if (hazard.CompareTag("BoltEnemy")) {

			TextAnimation tmpTA = new TextAnimation();

			tmpTA.safText = (GameObject)Instantiate(sAFAsteroid, camera.WorldToScreenPoint(new Vector3(hazard.transform.position.x + 20 * Time.fixedDeltaTime, hazard.transform.position.y, hazard.transform.position.z + 20 * Time.fixedDeltaTime)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
			tmpTA.safText.GetComponent<Text>().text = "+75";
			tmpTA.safText.GetComponent<Text>().color = new Color(150.0f / 255.0f, 78.0f / 255.0f, 240.0f / 255.0f);
			tmpTA.safText.transform.SetParent(canvas.transform);

			StartCoroutine("LifeOfTextAnimationCoroutine", tmpTA);
		
		}

	}

	public void GameOverMethod() {

		actualState = State.GAMEOVER;

		gameOverBackground.SetActive(true);

		GameObject lifeTube = GetChildGameObject(lifeBar, "LifeTube");
		GetChildGameObject(lifeTube, "Fill Area").SetActive(false);

		GetChildGameObject(gameOverBackground, "Game Over Pontuacao").GetComponent<Text>().text = "Pontuação: " + score.ToString().PadLeft(6, '0');
	}

	private void ScoreMananger(GameObject backgroundInterno) {

		//backgroundInterno vai ser gameOverBackground ou proximoNivelBackground
		
		//Gerenciamento do salvamento da pontuacao
		FileDealer fileDealer = new FileDealer();

		if (fileDealer.Load() == null){

			List<int> scoreVector = new List<int>(5);

			scoreVector.Insert(0, score);

			//Art star e mensagem
			if (score > 0){
				GetChildGameObject(backgroundInterno, "StarRecord").GetComponent<Image>().enabled = true;
				GetChildGameObject(backgroundInterno, "PontuacaoRecord").GetComponent<Text>().enabled = true;
			}
			else{
				GetChildGameObject(backgroundInterno, "StarRecord").GetComponent<Image>().enabled = false;
				GetChildGameObject(backgroundInterno, "PontuacaoRecord").GetComponent<Text>().enabled = false;
			}

			//Impressão do vetor para teste
			//for (int i = 0; i < scoreVector.Count; i++)
			//	print("[" + i + "] " + scoreVector[i]);

			fileDealer.Save(scoreVector);

		}
		else{

			List<int> scoreVector = fileDealer.Load();


			if (score > scoreVector[scoreVector.Count - 1]){

				//Art star e mensagem
				GetChildGameObject(backgroundInterno, "StarRecord").GetComponent<Image>().enabled = false;
				GetChildGameObject(backgroundInterno, "PontuacaoRecord").GetComponent<Text>().enabled = false;

				if (scoreVector.Count == 5)
					scoreVector.RemoveAt(scoreVector.Count - 1);

				scoreVector.Insert(scoreVector.Count, score);

				if (score > scoreVector[0]){
					GetChildGameObject(backgroundInterno, "StarRecord").GetComponent<Image>().enabled = true;
					GetChildGameObject(backgroundInterno, "PontuacaoRecord").GetComponent<Text>().enabled = true;
				}

				//Vector antes
				//Debug.Log("Antes");
				//for (int i = 0; i < scoreVector.Count; i++)
				//	print("[" + i + "] " + scoreVector[i]);

				scoreVector = InsertionSort(scoreVector);

				//Vector depois
				//Debug.Log("Depois");
				//for (int i = 0; i < scoreVector.Count; i++)
				//	print("[" + i + "] " + scoreVector[i]);


				fileDealer.Save(scoreVector);

			}
			else{

				//Impressão do vetor para teste
				//for (int i = 0; i < scoreVector.Count; i++)
				//	print("[" + i + "] " + scoreVector[i]);

				GetChildGameObject(backgroundInterno, "StarRecord").GetComponent<Image>().enabled = false;
				GetChildGameObject(backgroundInterno, "PontuacaoRecord").GetComponent<Text>().enabled = false;
			}

		}

	}

	IEnumerator LifeOfTextAnimationCoroutine(TextAnimation tmpTA) {

		textAnimationVector.Add(tmpTA);

		yield return new WaitForSeconds(TextAnimation.time);

		Destroy(textAnimationVector[0].safText);
		textAnimationVector.RemoveAt(0);

	}

	static public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
	{
		Transform[] ts = fromGameObject.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}

	private class TextAnimation{
		public GameObject safText;
		public const float time = TIMEFORTHETEXTSCOREEXISTENCE;
	}

	public int GetLife(){
		return this.life;
	}

	public void RestartGame() {

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

	public void PausarButton(){

		if (actualState == State.PLAYING) { 

			pausado = !pausado;

			if (pausado){
				Time.timeScale = 0;
				pausedBackground.SetActive(true);
			}
			else { 
				Time.timeScale = 1;
				pausedBackground.SetActive(false);
			}

		}
	
	}

	public List<int> InsertionSort(List<int> lista){

		int i, j, tmp;

		for (i = 1; i < lista.Count; i++) {

			j = i;

			while (j > 0 && lista[j - 1] < lista[j]) {
				tmp = lista[j];
				lista[j] = lista[j - 1];
				lista[j - 1] = tmp;
				j--;
			}
		
		}

		return lista;

	}

}
