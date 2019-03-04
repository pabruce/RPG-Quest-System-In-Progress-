using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn 
{
	public string Attacker; 
	public string Type;
	public GameObject AttackersGameObject; //attacker
	public GameObject AttackersTarget;	//acttacked target

	public BasicAttack chosenAttack;
}
