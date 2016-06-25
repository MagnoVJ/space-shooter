using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private int pointerID;
	private bool touched;
	private bool canFire;

	void Awake(){
		touched = false;
	}

	public void OnPointerDown(PointerEventData data){ 
		//Set the start point
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			canFire = true;
		}
	}

	public void OnPointerUp(PointerEventData data){   
		//Reset Everything
		if (data.pointerId == pointerID) {
			canFire = false;
			touched = false;
		}
	}

	public bool CanFire(){
		return canFire;
	}

}