using UnityEngine;
using System.Collections;

using NSState;

/* Classe responsável pelo gerenciamento da entrada (Input) de comandos para o Player Ship. */

public class PlayerInput : MonoBehaviour {

	private Rigidbody rigidbody;
	private float nextFire;

	public float speed;
	public float tilt;
	public float fireRate;
	public Boundary boundary;
	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;
	public GameObject shot;

	void Start () {

		nextFire = 0.0f;
		rigidbody = GetComponent<Rigidbody>();
	
	}

	void Update(){

		//Caso o jogo esteja sendo executado no Editor ou em um computador Windows
		if (Application.platform == RuntimePlatform.WindowsPlayer ||
   		    Application.platform == RuntimePlatform.WindowsEditor) {

			if (Input.GetKey(KeyCode.F) && Time.time > nextFire && GameController.actualState == State.PLAYING && Time.timeScale == 1) {
				nextFire = Time.time + fireRate;
				GameObject shotSpawn = GetChildGameObject(gameObject, "Shot_Spawner");
				Instantiate(shot, shotSpawn.transform.position, shotSpawn.transform.rotation);
			}
		
		}
		else if (Application.platform == RuntimePlatform.Android) {

			if (areaButton.CanFire() && Time.time > nextFire && GameController.actualState == State.PLAYING && Time.timeScale == 1){
				nextFire = Time.time + fireRate;
				GameObject shotSpawn = GetChildGameObject(gameObject, "Shot_Spawner");
				Instantiate(shot, shotSpawn.transform.position, shotSpawn.transform.rotation);
			}
		
		}

	}

	void FixedUpdate() {

		//Caso o jogo esteja sendo executado no Editor ou em um computador Windows
		if (Application.platform == RuntimePlatform.WindowsPlayer ||
			Application.platform == RuntimePlatform.WindowsEditor) {

			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");

			rigidbody.velocity = speed * new Vector3(moveHorizontal, 0.0f, moveVertical);

			//Evita que o Player Ship saia dos valores mínimos e máximos estabelecidos
			rigidbody.position = new Vector3(Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
				0.0f, Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax));

			//Inclina o Ship de acordo com a variável tilt
			rigidbody.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
		
		}
		//Caso o jogo esteja sendo executado em um dispositivo Android
		else if (Application.platform == RuntimePlatform.Android) {

			rigidbody.velocity = speed * new Vector3(touchPad.GetDirection().x, 0.0f, touchPad.GetDirection().y);

			rigidbody.position = new Vector3(Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
				0.0f, Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax));

			//Inclina o Ship de acordo com a variável tilt
			rigidbody.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
			
		}

	}

	static public GameObject GetChildGameObject(GameObject fromGameObject, string withName){
		Transform[] ts = fromGameObject.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}

}