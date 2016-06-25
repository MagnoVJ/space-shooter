using UnityEngine;
using System.Collections;

public class PlayerImmunity : MonoBehaviour {

	[HideInInspector]
	public bool immune;

	public float immunitySeconds;
	public float blinkSecons;
	public Material material;

	void Start(){

		StartCoroutine(SecondsOfImmunity());
	
	} 

	IEnumerator SecondsOfImmunity(){

		immune = true;
		bool flag = true;

		StartCoroutine("Immunity", flag);

		yield return new WaitForSeconds(immunitySeconds);

		StopCoroutine("Immunity");

		Immunity(false);
		immune = false;

	}

	IEnumerator Immunity(bool flag){

		while (true) {

			//Se flag = true Rendering Mode será transparent 
			if (flag){
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
			}
			//Se flag = false Rendering Mode será Opaque
			else{
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = -1;
			}

			flag = !flag;

			yield return new WaitForSeconds(blinkSecons); 

		} 

	}

	static public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
	{
		Transform[] ts = fromGameObject.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}

}