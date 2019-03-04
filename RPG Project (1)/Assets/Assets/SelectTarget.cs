using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SelectTarget : MonoBehaviour
{
	private PlayerAttack attackScript;
	private GameObject targetRing;

	// Use this for initialization
	void Start () 
	{
		Cursor.visible = true;

		attackScript = (PlayerAttack)GetComponent ("PlayerAttack");
		if (attackScript != null) 
		{
			Debug.Log ("We have found the player attack script");
		}

		targetRing = GameObject.FindGameObjectWithTag ("TargetRing");
		if (targetRing != null) 
		{
			Debug.Log ("found target ring");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool mousePress = CrossPlatformInputManager.GetButtonUp("Mouse0");

		if (mousePress) 
		{
			RaycastHit raycastHit = new RaycastHit ();

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition),
				    out raycastHit)) 
			{
				Debug.Log ("hit " + raycastHit.collider.gameObject.name);

				if (raycastHit.collider.gameObject.CompareTag ("Enemy"))
				{
					Debug.Log ("change target!!");
					attackScript.target = raycastHit.collider.gameObject;

					EnemyAI enemy = (EnemyAI)raycastHit.collider.gameObject.GetComponent ("EnemyAI");
					if (enemy != null) 
					{
						enemy.setTargetRing (targetRing);
					}
				}
			} 
			else 
			{
				Debug.Log ("didn't hit anything");
			}
		}
	}
}
