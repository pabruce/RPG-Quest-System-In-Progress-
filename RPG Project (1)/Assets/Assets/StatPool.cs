using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPool : MonoBehaviour 
{
	private Text pointsLeft;
	private int value;

	// Use this for initialization
	void Start () 
	{
		pointsLeft = transform.Find ("PointsLeft/Text").GetComponent<Text> ();

		if (pointsLeft != null) 
		{
			Debug.Log ("found points left");
		}

		value = int.Parse(pointsLeft.text);

		foreach (Transform child in transform) 
		{
			Spinner spinner = child.GetComponent<Spinner> ();

			if (spinner != null) 
			{
				Debug.Log ("assigning validator to " + spinner.name);
				spinner.validator = Adjust;
			}
		}
	}
	
	// Update is called once per frame
	public bool Adjust (int delta)
	{
		if (value - delta >= 0) 
		{
			value -= delta;
			pointsLeft.text = value.ToString ();

			return true;
		} 
		else 
		{
			return false; 	
		}
	}
}
