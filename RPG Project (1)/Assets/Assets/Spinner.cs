using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate bool ValidateFunc(int delta);

public class Spinner : MonoBehaviour 
{
	public int min = 0;
	public int max = 100;

	public InputField field;
	private Button up_button;
	private Button down_button;

	public ValidateFunc validator;

	// Use this for initialization
	void Start ()
	{
		field = GetComponent<InputField> ();

		up_button = transform.Find ("SpinUp").GetComponent<Button> ();
		down_button = transform.Find ("SpinDown").GetComponent<Button> ();

		if ((up_button == null) || (down_button == null))
		{
				Debug.Log ("cannot find buttons");
				return;
		}

		if (field == null) 
		{
			Debug.Log ("cannot find input field");
			return;
		}

		if (validator == null) 
		{
			Debug.Log ("no validator seen");
		}


		up_button.onClick.AddListener (delegate {ButtonHandler (1);});
		down_button.onClick.AddListener (delegate {ButtonHandler (-1);});

	}
	
	// Update is called once per frame
	void ButtonHandler(int delta) 
	{
		int value = 0;

		if (field != null) 
		{
			value = int.Parse (field.text);
		}

		value += delta;

		if ((value >= min) && (value <= max)) 
		{
			if (validator (delta)) 
			{
				field.text = value.ToString ();
			}
		}
	}
}
