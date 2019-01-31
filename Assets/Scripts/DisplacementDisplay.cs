using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplacementDisplay : MonoBehaviour
{

	public GameObject textTemplate;
	public List<GameObject> displays;
	public Camera cam;
	public Text DebugSnack;
	public TMP_Dropdown ddObject;
    public GoogleARCore.Examples.HelloAR.HelloARController helloAR;

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
				display.GetComponent<TextMeshPro>().text = rounded + "m";
			}
		}

	    DebugSnack.GetComponent<Text>().text = displays.ToList().Count().ToString() + "Displacement displays in action";
    }

	public void Display(GameObject item)
	{
		Vector3 loc;
		Color c;
		if (helloAR.SelectionValue == 0)//if marker NOT CONE
		{ 
			loc = item.transform.position + new Vector3(0f, 0.3f, 0);
			c = Color.white; //marker
		} 
		else
		{
			loc = item.transform.position + new Vector3(0f, 0.35f, 0);
			c = Color.red; //cone - why no orange r u mad
		} 
		var display = Instantiate(textTemplate, loc, textTemplate.transform.rotation);
		display.tag = "pointText";
		display.GetComponent<TextMeshPro>().color = c;
		displays.Add(display);
	}

	public void InstantiateTest()
	{
		var dab = Instantiate(textTemplate, Vector3.zero, Quaternion.identity);
		dab.GetComponent<TextMeshPro>().text = "testestestestestestes";
		displays.Add(dab);
	}
}
