using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplacementDisplay : MonoBehaviour
{

	public GameObject textTemplate;
	public List<GameObject> displays;
	public Camera cam;
	public Text DebugSnack;
	
	// Update is called once per frame
	void Update () {
		foreach (var display in displays)
		{
			if (!display.CompareTag("cleared"))
			{
				display.transform.LookAt(cam.transform);
				display.transform.Rotate(0, 180f, 0, Space.Self);
				var dist = Vector3.Distance(display.transform.position, cam.transform.position);
				var rounded = Math.Round(dist, 2);
				display.GetComponent<TextMeshPro>().text = rounded.ToString() + "m";
			}
		}
	}

	public void Display(GameObject item, float distance)
	{
		//DebugSnack.GetComponent<Text>().text = "Display CALLED";
		var loc = item.transform.position + new Vector3(0f, 0.3f, 0);
		var display = Instantiate(textTemplate, loc, textTemplate.transform.rotation);
		//var display = Instantiate(textTemplate, Vector3.zero + Vector3.up, new Quaternion(0f, 0f, 0f, 0f));
		//display.GetComponent<TextMeshPro>().text = distance.ToString();
		display.tag = "pointText";
		displays.Add(display);
	}

	public void InstantiateTest()
	{
		var dab = Instantiate(textTemplate, Vector3.zero, Quaternion.identity);
		dab.GetComponent<TextMeshPro>().text = "testestestestestestes";
		displays.Add(dab);
	}
}
