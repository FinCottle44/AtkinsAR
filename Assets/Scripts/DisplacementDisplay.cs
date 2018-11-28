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
			display.transform.LookAt(cam.transform);
		}
	}

	public void Display(GameObject item, float distance)
	{
		DebugSnack.GetComponent<Text>().text = "Display CALLED";
		var loc = item.transform.position + Vector3.up;
		var display = Instantiate(textTemplate, loc, new Quaternion(0f, 0f, 0f, 0f));
		//var display = Instantiate(textTemplate, Vector3.zero + Vector3.up, new Quaternion(0f, 0f, 0f, 0f));
		display.GetComponent<TextMeshPro>().text = distance.ToString();
		displays.Add(display);
	}

	public void InstantiateTest()
	{
		var dab = Instantiate(textTemplate, Vector3.zero, Quaternion.identity);
		dab.GetComponent<TextMeshPro>().text = "testestestestestestes";
		displays.Add(dab);
	}
}
