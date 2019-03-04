using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;

//	private bool smooth = true;
//	private float smoothSpeed = 0.125f;
//	private Vector3 offset = new Vector3(0, 0, -10f);

	public float distance = 10f;

	public float height = 5f;

	public float heightDamping = 2f;


	void Start()
	{
		//target = GameObject.Find("HeroCharacter").transform;
	}


	void LateUpdate()
	{
//		Vector3 desiredPosition = target.transform.position + offset;
//
//		transform.position = desiredPosition;



		if (!target)
		{
			return;
		}

		float wantedHeight = target.position.y + height;
		float currentHeight = transform.position.y;

		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		Vector3 pos = target.position;
		pos -= Vector3.forward * distance;
		pos.y = currentHeight;
		transform.position = pos;

		transform.LookAt(target);
	}
}
