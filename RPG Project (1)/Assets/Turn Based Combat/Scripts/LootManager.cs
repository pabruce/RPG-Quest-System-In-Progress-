using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour 
{
	[System.Serializable]
	public class Loot
	{
		public GameObject lootObject;
		public float spawnChance;
	}

	public List<Loot> loot = new List<Loot> ();
	public List<GameObject> spawnPoints = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < loot.Count; i++)
		{
			if(Random.value * 100 < loot[i].spawnChance)
			{
				Instantiate(loot[i].lootObject, spawnPoints[Random.Range(0,spawnPoints.Count)].transform.position,Quaternion.identity);
				Debug.Log(loot[i].lootObject.name + " has spawned in the location: " + loot[i].lootObject.transform.position);
				Debug.Log ("spawn chance is: " + loot [i].spawnChance + "%");
			}
		}	
	}
}
