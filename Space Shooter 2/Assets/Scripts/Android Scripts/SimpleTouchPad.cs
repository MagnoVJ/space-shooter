using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler{

	private int pointerID;
	private bool touched;
	private Vector2 origin;
	private Vector2 direction;
	private Vector2 smoothDirection;

	public float smoothing;

	void Awake(){

		touched = false;
		direction = Vector2.zero;

	}

	public void OnPointerDown(PointerEventData data){ 
		//Set start point
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			origin = data.position;
		}
	
	}

	public void OnDrag(PointerEventData data){ 
		//Compare the difference between our start point and current pointer position
		if (data.pointerId == pointerID) {
			Vector2 directionRaw = data.position - origin;
			direction = directionRaw.normalized;
		}
	
	}

	public void OnPointerUp(PointerEventData data){
		//Reset everything
		if (data.pointerId == pointerID) {
			direction = Vector2.zero;
			touched = false;
		}
	
	}

	public Vector2 GetDirection(){
		smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
		return smoothDirection;
	}

}