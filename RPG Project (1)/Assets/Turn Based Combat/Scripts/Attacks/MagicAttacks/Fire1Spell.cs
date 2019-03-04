using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1Spell : BasicAttack 
{
	public Fire1Spell()
	{
		attackName = "Fire 1";
		attackDescription = "Unimpressive Fireball";
		attackDamage = 20f;
		attackCost = 10f;
	}
}
