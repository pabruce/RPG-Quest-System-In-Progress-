using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlCtl : MonoBehaviour
{
	public Animator anim;

	private float inputH;
	private float inputV;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			anim.Play ("WAIT01", 0);
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) 
		{
			anim.Play ("WAIT02", 0);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			anim.Play ("WAIT03", 0);
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) 
		{
			anim.Play ("WAIT04", 0);
		}

		inputH = Input.GetAxis ("Horizontal");
		inputV = Input.GetAxis ("Vertical");

		anim.SetFloat ("inputH", inputH);
		anim.SetFloat ("inputV", inputV);
	}
}
