using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	private Animator anim;

	//public Vector2 speed = new Vector2(1,1);
	public float speed = 5f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float inputX = Input.GetAxis("Horizontal");
		float inputZ = Input.GetAxis("Vertical");

		//anim.SetFloat("SpeedX", inputX);
		//anim.SetFloat("SpeedY", inputZ);

		//FINAL MOVEMENT
		Vector3 movement = new Vector3(inputX, 0 ,inputZ);
		//movement = movement.normalized * speed * Time.deltaTime;



//		float dirX = Input.GetAxisRaw("Horizontal");
//		float dirZ = Input.GetAxisRaw("Vertical");
//
//		transform.eulerAngles = (dirX > 0) ? Vector3.up * 90 : Vector3.up * -90 ;

		transform.Translate(movement* speed * Time.deltaTime,Space.World);
	}

	void FixedUpdate()
	{
		float lastInputX = Input.GetAxis("Horizontal");
		float lastInputY = Input.GetAxis("Vertical");

		if (lastInputX != 0 || lastInputY != 0)
		{
			anim.SetBool("walking", true);
		}
		else
		{
			anim.SetBool("walking", false);
		}

		//ROTATION
		Vector3 movedirection = new Vector3(lastInputX, 0, lastInputY);

		if (movedirection != Vector3.zero)
		{
			
			Quaternion newRotation = Quaternion.LookRotation(movedirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5);
		}
	}
}
