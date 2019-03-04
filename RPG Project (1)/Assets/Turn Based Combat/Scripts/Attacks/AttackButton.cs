using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour 
{

	public BasicAttack magicAttackToPerform;

	public void CastMagicAttack()
	{
		GameObject.Find ("BattleManager").GetComponent<BattleStateMachine> ().Input4 (magicAttackToPerform);
	}
}
