using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorPicker : MonoBehaviour
{

	public Color col;

	private Transform transform;
	
	// Use this for initialization
	void Start () {
		RandColor();
		transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void RandColor()
	{
	    //generate random RGB values depending on position, therefore all colors different
		float r, g, b;
		r = Random.Range(0f, 1f);
		g = Random.Range(0f, 1f);
		b = Random.Range(0f, 1f);

		col = new Color(r, g, b);
//		foreach (Transform child in transform.GetComponentsInChildren<Transform>())
//		{
//			GameObject childGO = child.gameObject;
//			childGO.GetComponent<Renderer>().material.color = col;
//		}		
//		//gameObject.GetComponent<MeshRenderer>().material.color = col;
	}
}
