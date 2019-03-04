using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour 
{
	

	private BattleStateMachine BSM;
	public BaseEnemy enemy;

	public enum TurnState
	{
		PROCESSING,
		CHOOSEACTION,
		WAITING,
		ACTION,
		DEAD
	}

	public TurnState currentState;
	//for progress bar 
	private float cur_cooldown = 0f;
	private float max_cooldown = 5f;

	//gameojbect
	private Vector3 startposition;
	public GameObject Selector;

	//IEnumerator function
	private bool actionStarted = false;
	public GameObject HeroToAttack;
	private float animSpeed = 5f;

	//alive
	private bool alive = true;


	// Use this for initialization
	void Start () 
	{
		currentState = TurnState.PROCESSING;
		Selector.SetActive (false);
		BSM = GameObject.Find ("BattleManager").GetComponent<BattleStateMachine> ();
		//fill info into the vector3
		startposition = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		switch (currentState) 
		{
		case(TurnState.PROCESSING):
			UpgradeProgressBar ();
			break;

		case(TurnState.CHOOSEACTION):
			ChooseAction ();
			currentState = TurnState.WAITING;
			break;

		case(TurnState.WAITING):
			//idle
			break;

		case(TurnState.ACTION):
			StartCoroutine (TimeForAction ());
			break;

		case(TurnState.DEAD):
			if (!alive)
			{
				return;
			}
			else
			{
				//change tag of enemy
				this.gameObject.tag = "DeadEnemy";
				//not attackable
				BSM.EnemiesInBattle.Remove(this.gameObject);
				//disable selector
				Selector.SetActive(false);
				//remove all inputs heroattacks

				if (BSM.HeroesInBattle.Count > 0) 
				{
					for (int i = 0; i < BSM.PerformList.Count; i++) 
					{
						if (i != 0) 
						{

							if (BSM.PerformList [i].AttackersGameObject == this.gameObject)
							{
								BSM.PerformList.Remove (BSM.PerformList [i]);
							}
							if (BSM.PerformList [i].AttackersTarget == this.gameObject)
							{
								BSM.PerformList [i].AttackersTarget = BSM.EnemiesInBattle [Random.Range (0, BSM.EnemiesInBattle.Count)];
							}
						}
					}
				}
				//change color to gray
				gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105,105,105,255);
				//set alive false
				alive = false;
				//reset enemy buttons
				BSM.EnemyButtons ();
				//check alive
				BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;

				//add loot upon death

			}
			break;
		}
	}

	void UpgradeProgressBar()
	{
		cur_cooldown = cur_cooldown + Time.deltaTime;

		if (cur_cooldown >= max_cooldown) 
		{
			currentState = TurnState.CHOOSEACTION;
		}
	}

	void ChooseAction()
	{
		HandleTurn myAttack = new HandleTurn ();
		myAttack.Attacker = enemy.theName;
		myAttack.Type = "Enemy";
		myAttack.AttackersGameObject = this.gameObject;
		myAttack.AttackersTarget = BSM.HeroesInBattle [Random.Range (0, BSM.HeroesInBattle.Count)];

		int num = Random.Range (0, enemy.attacks.Count);
		myAttack.chosenAttack = enemy.attacks [num];
		Debug.Log (this.gameObject.name + " has chosen " + myAttack.chosenAttack.attackName + " and does" + myAttack.chosenAttack.attackDamage + " damage");
		BSM.CollectActions (myAttack);
	}

	private IEnumerator TimeForAction()
	{
		if (actionStarted) 
		{
			yield break;
		}

		actionStarted = true;

		//animate enemy to attack
		Vector3 heroPosition = new Vector3 (HeroToAttack.transform.position.x + 1.5f, HeroToAttack.transform.position.y,HeroToAttack.transform.position.z);
		while (MoveTowardsEnemy (heroPosition)) 
		{
			yield return null;
		}
		//wait
		yield return new WaitForSeconds(0.5f);
		//attack
		DoDamage();
		//animate to beginning
		Vector3 firstPosition = startposition;
		while (MoveTowardsStart(firstPosition)) 
		{
			yield return null;
		}
		//remove performer in Battlestatemachine performAction
		BSM.PerformList.RemoveAt(0);
		//reset battlestate to wait
		BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
		//end routine
		actionStarted = false;
		//reset
		cur_cooldown = 0f;
		currentState = TurnState.PROCESSING;
	}

	private bool MoveTowardsEnemy(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsStart(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards (transform.position, target, animSpeed * Time.deltaTime));
	}

	void DoDamage()
	{
		float calc_damage = enemy.curATK + BSM.PerformList [0].chosenAttack.attackDamage;
		HeroToAttack.GetComponent<HeroStateMachine> ().TakeDamage(calc_damage);

	}

	public void TakeDamage(float getDamageAmount)
	{
		enemy.curHP -= getDamageAmount;

		if (enemy.curHP <= 0) 
		{
			enemy.curHP = 0;
			currentState = TurnState.DEAD;
		}
		//updates current health
	}
		
}
