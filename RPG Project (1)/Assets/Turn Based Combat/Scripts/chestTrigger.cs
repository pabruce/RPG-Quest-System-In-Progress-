using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chestTrigger : MonoBehaviour {

	public string text;
	public bool display = false;

	void OnTriggerEnter(Collider iCollide)
	{
		if (iCollide.transform.tag == "Player") 
		{
			display = true;
		}

	}

	void OnTriggerExit(Collider uCollide)
	{
		if (uCollide.transform.tag == "Player") 
		{
			display = false;
		}
	}

	void OnGUI()
	{
		if (display == true) 
		{
			GUI.Box (new Rect(0,50,Screen.width,Screen.height-50),text);
		}
	}
}
