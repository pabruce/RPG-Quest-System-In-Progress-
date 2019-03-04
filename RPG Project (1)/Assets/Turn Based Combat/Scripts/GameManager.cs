using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	//Random Monster
	[System.Serializable]
	public class RegionData
	{
		public string regionName;
		public int maxAmountEnemies = 3;
		public string BattleScene;
		public List <GameObject> possibleEnemies = new List<GameObject> ();
	}

	public int curRegions;

	public List<RegionData> Regions = new List<RegionData> ();

	//spawnpoints
	public string nextSpawnPoint;

	//herochar
	public GameObject heroCharacter;

	//Positions
	public Vector3 nextHeroPosition;
	public Vector3 lastHeroPosition; //BATTLE

	//Scenes
	public string sceneToLoad;
	public string lastScene;//Battle

	//booleans
	public bool isWalking = false;
	public bool canGetEncounter = false;
	public bool gotAttacked = false;

	//Enumerators
	public enum GameStates
	{
		WORLD_STATE, //Represent world map 
		TOWN_STATE, //Located in the town 
		BATTLE_STATE, //Entering Battle
		IDLE
	}

	//Battle
	public int enemyAmount;
	public List<GameObject> enemiesToBattle = new List<GameObject>();

	public GameStates gameState;

	void Start ()
	{
		transform.position = GameManager.instance.nextHeroPosition;
	}

	void Awake()
	{
		//check if instance exist
		if (instance == null) 
		{
			//if not set the instance to this
			instance = this;
		} 
		//if it exist but is not this instance
		else if (instance != this) 
		{
			//destroy
			Destroy (gameObject);
		}
		//set this to be not destroyable
		DontDestroyOnLoad(gameObject);
		if (!GameObject.Find ("HeroCharacter")) 
		{
			GameObject Hero = Instantiate (heroCharacter, nextHeroPosition, Quaternion.identity) as GameObject;
			Hero.name = "HeroCharacter";
		}
	}

	void Update()
	{
		switch (gameState) 
		{
		case (GameStates.WORLD_STATE):
			if (isWalking) 
			{
				RandomEncounter ();
			}
			if (gotAttacked) 
			{
				gameState = GameStates.BATTLE_STATE;
			}
			break;
		case (GameStates.TOWN_STATE):

			break;
		case (GameStates.BATTLE_STATE):
			//load battle scene
			StartBattle ();
			gameState = GameStates.WORLD_STATE;

			//go back to idle state
			break;
		case (GameStates.IDLE):
			break;
		}
	}
		
	public void LoadNextScene()
	{
		SceneManager.LoadScene (sceneToLoad);
	}

	public void LoadSceneAfterBattle()
	{
		SceneManager.LoadScene (lastScene);
	}

	void RandomEncounter()
	{
		if (isWalking && canGetEncounter) 
		{
			if (Random.Range (0, 8500) < 10) 
			{
				//Debug.Log ("I got attacked");
				gotAttacked = true;
			}
		}
	}

	void StartBattle()
	{
		//amt of enemies
		enemyAmount = Random.Range(1,Regions[curRegions].maxAmountEnemies+1);
		//Enemy types
		for (int i = 0; i < enemyAmount; i++) 
		{
			enemiesToBattle.Add(Regions[curRegions].possibleEnemies[Random.Range(0,Regions[curRegions].possibleEnemies.Count)]);
		}
		//hero
		lastHeroPosition = GameObject.Find("HeroCharacter").gameObject.transform.position;
		nextHeroPosition = lastHeroPosition;
		lastScene = SceneManager.GetActiveScene ().name;
		//load level
		SceneManager.LoadScene(Regions[curRegions].BattleScene);
		//Reset Hero
		isWalking = false;
		gotAttacked = false;
		canGetEncounter = false;
	}
}
