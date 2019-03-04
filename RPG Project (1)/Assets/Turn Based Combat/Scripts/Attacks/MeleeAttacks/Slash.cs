using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BasicAttack
{
	public Slash()
	{
		attackName = "Slash";
		attackDescription = "Slashes quickly at the enemy";
		attackDamage = 10f;
		attackCost = 0;
	}
}
