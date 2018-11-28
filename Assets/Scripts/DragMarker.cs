using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class DragMarker : MonoBehaviour
{

	public Transform parent;	
	
	//GET ON DRAG N THEN MOVE PARENT OF MARKER OBJECT 
	// Use this for initialization
	void Start ()
	{
		parent = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) 
		{
			Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero
     
			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) 
			{
				// get the touch position from the screen touch to world point
				Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
				// lerp and set the position of the current object to that of the touch, but smoothly over time.
				transform.position = Vector3.Lerp(parent.position, touchedPos, Time.deltaTime);
			}
		}
	}
}
