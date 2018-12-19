using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetManipulation : MonoBehaviour
{

	int TapCount;
	public float MaxDubTapTime;
	float NewTime;

	
	
	void Start () {
		TapCount = 0;
	}


	void LateUpdate()
	{
		float pinchAmount = 0;
		Quaternion desiredRotation = transform.rotation;

		DetectTouchMovement.Calculate();

		if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
		{
			// zoom
			pinchAmount = DetectTouchMovement.pinchDistanceDelta;
		}

		if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
		{
			// rotate
			Vector3 rotationDeg = Vector3.zero;
			rotationDeg.y = -DetectTouchMovement.turnAngleDelta;
			desiredRotation *= Quaternion.Euler(rotationDeg);
		}


		// not so sure those will work:
		transform.rotation = desiredRotation;
		//transform.position += Vector3.forward * pinchAmount;

		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Ended)
			{
				TapCount += 1;
			}

			if (TapCount == 1)
			{
				NewTime = Time.time + MaxDubTapTime;
			}
			else if (TapCount == 2 && Time.time <= NewTime)
			{

				//Whatever you want after a dubble tap    
				print("Dubble tap");
				gameObject.SetActive(false);

				TapCount = 0;
			}
		}

	}
}
