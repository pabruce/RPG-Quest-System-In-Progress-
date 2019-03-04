using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicAttack : MonoBehaviour
{
	public string attackName;
	public string attackDescription;//att description
	public float attackDamage;
	public float attackCost;//for mp
	public bool magicAttack;
}
