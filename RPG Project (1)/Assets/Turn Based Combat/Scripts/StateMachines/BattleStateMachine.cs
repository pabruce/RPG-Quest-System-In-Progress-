using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour 
{
	public enum PerformAction
	{
		WAIT,
		TAKEACTION,//hero
		PERFORMACTION,
		CHECKALIVE,
		WIN,
		LOSE,
		LOOT
	}



	public PerformAction battleStates;

	public List<HandleTurn> PerformList = new List<HandleTurn>();

	public List<GameObject> HeroesInBattle = new List<GameObject>();
	public List<GameObject> EnemiesInBattle= new List<GameObject>();

	//Loot System
	public List <DropCurrency> LootTable = new List<DropCurrency>();
	public int dropChance;

	public enum HeroGUI
	{
		ACTIVATE,
		WAITING,
		INPUT1,
		INPUT2,
		INPUT3,
		INPUT4,
		DONE
	}

	public HeroGUI HeroInput;

	public List<GameObject> HeroesToManage = new List<GameObject>();
	private HandleTurn HeroChoice;

	public GameObject enemyButton;
	public Transform Spacer;

	public GameObject ActionPanel;
	public GameObject EnemySelectPanel;
	public GameObject MagicPanel;
	public GameObject LootPanel;

	//attack of heroes
	public Transform actionSpacer;
	public Transform magicSpacer;
	public GameObject actionButton;
	public GameObject magicButton;

	private List<GameObject> atkBtns = new List<GameObject>();

	//enemy buttons
	private List<GameObject> enemyBtns = new List<GameObject>();

	//SpawnPoints
	public List<Transform> spawnPoints = new List<Transform>();

	void Awake()
	{
		for (int i = 0; i < GameManager.instance.enemyAmount; i++) 
		{
			GameObject NewEnemy = Instantiate (GameManager.instance.enemiesToBattle [i], spawnPoints [i].position, Quaternion.identity) as GameObject;
			NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine> ().enemy.theName + "_" + (i + 1);
			NewEnemy.GetComponent<EnemyStateMachine> ().enemy.theName = NewEnemy.name;
			EnemiesInBattle.Add (NewEnemy);
		}
	}

	// Use this for initialization
	void Start () 
	{
		battleStates = PerformAction.WAIT;
		//EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag ("Hero"));
		HeroInput = HeroGUI.ACTIVATE;

		ActionPanel.SetActive (false);
		EnemySelectPanel.SetActive (false);
		MagicPanel.SetActive (false);
		LootPanel.SetActive (false);

		EnemyButtons ();
	}

	// Update is called once per frame
	void Update () 
	{
		switch (battleStates) 
		{
		case(PerformAction.WAIT):
			if (PerformList.Count > 0) 
			{
				battleStates = PerformAction.TAKEACTION;
			}
			break;

		case(PerformAction.TAKEACTION):
			GameObject performer = GameObject.Find (PerformList [0].Attacker);
			if (PerformList [0].Type == "Enemy") 
			{
				EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine> ();
				for (int i = 0; i < HeroesInBattle.Count;i++)
					{
						if (PerformList [0].AttackersTarget == HeroesInBattle [i]) 
						{
							ESM.HeroToAttack = PerformList [0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
							break;
						} 
						else 
						{
							PerformList [0].AttackersTarget = HeroesInBattle [Random.Range(0,HeroesInBattle.Count)];
							ESM.HeroToAttack = PerformList [0].AttackersTarget;
							ESM.currentState = EnemyStateMachine.TurnState.ACTION;
						}
					}
			}
			if (PerformList [0].Type == "Hero") 
			{
				HeroStateMachine HSM = performer.GetComponent<HeroStateMachine> ();

				HSM.EnemyToAttack = PerformList [0].AttackersTarget;
				HSM.currentState = HeroStateMachine.TurnState.ACTION;
			}
			battleStates = PerformAction.PERFORMACTION;
			break;
		case(PerformAction.PERFORMACTION):
			break;
		case(PerformAction.CHECKALIVE):
			if (HeroesInBattle.Count < 1) 
			{
				//loss
				battleStates = PerformAction.LOSE;
			} 
			else if (EnemiesInBattle.Count < 1) 
			{
				//try to set loot state

				//won
				battleStates = PerformAction.LOOT;
			} 
			else 
			{
				//call function
				clearActionPanel();
				HeroInput = HeroGUI.ACTIVATE;
			}
			break;
		case(PerformAction.LOOT):
			calcLoot();
			LootPanel.SetActive (true);

			for (int i = 0; i < HeroesInBattle.Count; i++) 
			{
				HeroesInBattle [i].GetComponent<HeroStateMachine> ().currentState = HeroStateMachine.TurnState.WAITING;
			}

			GameManager.instance.LoadSceneAfterBattle ();

			GameManager.instance.enemiesToBattle.Clear ();
			Debug.Log("You won the battle");
			break;

		case(PerformAction.WIN):
			{
				
				GameManager.instance.gameState = GameManager.GameStates.WORLD_STATE;
			}
			break;
		case(PerformAction.LOSE):
			{
				Debug.Log("You lost the battle");
			}
			break;
		}

		switch (HeroInput) 
		{
		case(HeroGUI.ACTIVATE):
			if (HeroesToManage.Count > 0) 
			{
				HeroesToManage [0].transform.FindChild ("Selector").gameObject.SetActive (true);
				//create new turn handler
				HeroChoice = new HandleTurn ();

				ActionPanel.SetActive (true);
				//populate action btns
				CreateAttackButtons();
				HeroInput = HeroGUI.WAITING;
			}
			break;
		case(HeroGUI.WAITING):
			break;
		case(HeroGUI.DONE):
			HeroInputDone ();
			break;

		}
	}

	public void CollectActions(HandleTurn input)
	{
		PerformList.Add (input);
	}

	//creates automatic buttons for enemies
	public void EnemyButtons()
	{
		//clean
		foreach (GameObject enemyBtn in enemyBtns) 
		{
			Destroy (enemyBtn);
		}
		enemyBtns.Clear ();

		//create buttons
		foreach (GameObject enemy in EnemiesInBattle) 
		{
			GameObject newButton = Instantiate (enemyButton) as GameObject;
			EnemySelectButton button = newButton.GetComponent<EnemySelectButton> ();

			EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine> ();

			Text buttonText = newButton.transform.FindChild ("Text").gameObject.GetComponent<Text> ();
			buttonText.text = cur_enemy.enemy.theName;

			button.EnemyPrefab = enemy;

			newButton.transform.SetParent (Spacer,false);
			enemyBtns.Add (newButton);
		}
	}

	//attack button
	public void Input1()
	{
		HeroChoice.Attacker = HeroesToManage [0].name;
		HeroChoice.AttackersGameObject = HeroesToManage [0];
		HeroChoice.Type = "Hero";
		HeroChoice.chosenAttack = HeroesToManage [0].GetComponent<HeroStateMachine>().hero.attacks[0];

		ActionPanel.SetActive (false);
		EnemySelectPanel.SetActive (true);
	}

	public void Input2(GameObject chosenEnemy)
	{
		HeroChoice.AttackersTarget = chosenEnemy;
		HeroInput = HeroGUI.DONE;
	}

	void HeroInputDone()
	{
		PerformList.Add (HeroChoice);
		clearActionPanel ();

		HeroesToManage [0].transform.FindChild ("Selector").gameObject.SetActive (false);
		HeroesToManage.RemoveAt (0);
		HeroInput = HeroGUI.ACTIVATE;
	}

	void clearActionPanel()
	{
		EnemySelectPanel.SetActive (false);
		ActionPanel.SetActive (false);
		MagicPanel.SetActive (false);
		LootPanel.SetActive (false);

		//clean action panel 
		foreach (GameObject atkBtn in atkBtns) 
		{
			Destroy (atkBtn);	
		}
		atkBtns.Clear ();
	}

	//create action buttons
	void CreateAttackButtons()
	{
		GameObject AttackButton = Instantiate (actionButton) as GameObject;
		Text ActionButtonText = AttackButton.transform.FindChild ("Text").gameObject.GetComponent<Text>();
		ActionButtonText.text = "Attack";
		AttackButton.GetComponent<Button>().onClick.AddListener(()=>Input1());
		AttackButton.transform.SetParent (actionSpacer, false);
		atkBtns.Add (AttackButton);

		GameObject MagicAttackButton = Instantiate (actionButton) as GameObject;
		Text MagicAttackButtonText = MagicAttackButton.transform.FindChild ("Text").gameObject.GetComponent<Text>();
		ActionButtonText.text = "Magic";
		AttackButton.GetComponent<Button>().onClick.AddListener(()=>Input3());
		MagicAttackButton.transform.SetParent (actionSpacer, false);
		atkBtns.Add (MagicAttackButton);

		if (HeroesToManage [0].GetComponent<HeroStateMachine> ().hero.MagicAttacks.Count > 0) 
		{
			foreach (BasicAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks) {
				GameObject MagicButton = Instantiate (magicButton) as GameObject;
				Text MagicButtonText = MagicButton.transform.FindChild ("Text").gameObject.GetComponent<Text> ();
				MagicButtonText.text = magicAtk.attackName;
				AttackButton ATB = MagicButton.GetComponent<AttackButton> ();
				ATB.magicAttackToPerform = magicAtk;
				MagicButton.transform.SetParent (magicSpacer, false);
				atkBtns.Add (MagicButton);
			}
		} 
		else 
		{
			MagicAttackButton.GetComponent<Button> ().interactable = false;
		}
	}

	public void Input4(BasicAttack chosenMagic)//chosen magic atk
	{
		HeroChoice.Attacker = HeroesToManage [0].name;
		HeroChoice.AttackersGameObject = HeroesToManage [0];
		HeroChoice.Type = "Hero";

		HeroChoice.chosenAttack = chosenMagic;
		MagicPanel.SetActive (false);
		EnemySelectPanel.SetActive (true);
	}

	public void Input3()
	{
		ActionPanel.SetActive (false);
		MagicPanel.SetActive (true);
	}

	[System.Serializable]
	public class DropCurrency
	{
		public string name; 
		public GameObject item;
		public int dropRarity; 
	}

	public void calcLoot()
	{
		int calc_dropChance = Random.Range (0, 101);

		if (calc_dropChance > dropChance) 
		{
			Debug.Log ("No Loot");
			return;
		}

		if (calc_dropChance <= dropChance) 
		{
			int itemWeight = 0;

			for (int i = 0; i < LootTable.Count; i++) 
			{
				itemWeight += LootTable [i].dropRarity;
			}
				
			int randomValue = Random.Range(0, itemWeight);

			for (int j = 0; j < LootTable.Count; j++) 
			{
				if (randomValue <= LootTable [j].dropRarity) 
				{
					Instantiate(LootTable[j].item,transform.position,Quaternion.identity);
					return;
				}
				randomValue -= LootTable [j].dropRarity;
				Debug.Log ("Random Value decreased" + randomValue);
			}
		}
	}
}
