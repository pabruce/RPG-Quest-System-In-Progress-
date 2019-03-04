using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EnemyAttack : MonoBehaviour
{
	public GameObject target; 
	private int weaponDamage = 10;
	public float attackTimer; 
	public float coolDown;
	private float maxMeleeDistance = 2.5f;

	// Use this for initialization
	void Start () 
	{
		attackTimer = 0;
		coolDown = 2.0f;
	}

	// Update is called once per frame
	void Update ()
	{
		if (attackTimer > 0) 
		{
			attackTimer -= Time.deltaTime;

			if (attackTimer < 0) 
			{
				attackTimer = 0;
			}
		}
			if (attackTimer == 0.0f) 
			{
				Attack ();
			}

	}

	private void Attack()
	{
		float distance = Vector3.Distance (target.transform.position, transform.position);

		if ((distance < maxMeleeDistance) && inFront(target))
		{
			PlayerHealth eh = (PlayerHealth)target.GetComponent ("PlayerHealth");

			if (eh != null) 
			{
				eh.AdjustCurrentHealth (-weaponDamage);
				attackTimer = coolDown;

			}
		}
	}

	private bool inFront(GameObject target)
	{
		Vector3 dir = (target.transform.position - transform.position);

		float direction = Vector3.Dot (dir, transform.forward);

		return (direction > 0);
	}
}
