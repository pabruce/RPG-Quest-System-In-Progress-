using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison1Spell : BasicAttack 
{
	public Poison1Spell()
	{
		attackName = "Poison 1";
		attackDescription = "Stings Slightly";
		attackDamage = 5f;
		attackCost = 5f;
	}
}
