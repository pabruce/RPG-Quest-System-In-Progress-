using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour 
{
	public Transform target; 
	public int RotationSpeed = 3;
	public int moveSpeed = 3;

	public Transform myTransform;

	void Awake()
	{
		myTransform = transform;	//cache transform
	}
	// Use this for initialization
	void Start () 
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player");

		if (go != null) 
		{
			target = go.transform;
		} 
		else 
		{
			target = null;
		}

		moveSpeed = 3;
		RotationSpeed = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (target == null) 
		{
			return;
		}

		Debug.DrawLine (target.position, myTransform.position, Color.yellow);

		myTransform.rotation = Quaternion.Slerp (myTransform.rotation, Quaternion.LookRotation (target.position - myTransform.position),
			RotationSpeed * Time.deltaTime);

		float dist = Vector3.Distance (target.position, myTransform.position);

		if (dist > 2) 
		{
			myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
		}

	}

	public void setTargetRing(GameObject targetRing)
	{
		targetRing.transform.parent = transform;
		targetRing.transform.localPosition = new Vector3 (0, -1, 0);
		targetRing.transform.localRotation = Quaternion.Euler (90, 0, 90);
	}
}