using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour 
{
	public Camera m_Camera;

	void Awake () 
	{
		m_Camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
			m_Camera.transform.rotation * Vector3.up);
	}
}
