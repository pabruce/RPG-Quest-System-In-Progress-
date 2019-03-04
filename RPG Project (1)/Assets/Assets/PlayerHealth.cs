using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{
	public int maxHealth = 100;
	public int curHealth = 100;

	private int xStart = 10;
	private int yStart = 10;
	private int barHeight = 20;

	public float healthBarLength;

	// Use this for initialization
	void Start () 
	{
		healthBarLength = Screen.width / 2;		//halfway across the screen
	}
	
	// Update is called once per frame
	void Update () 
	{
		AdjustCurrentHealth (0);
	}

	void OnGUI()
	{
		GUI.Box (new Rect (xStart, yStart, healthBarLength, barHeight), curHealth + "/" + maxHealth);
	}

	public void AdjustCurrentHealth(int adj)
	{
		curHealth += adj;

		if (curHealth < 0)
		{
			curHealth = 0;
		}

		if (curHealth > maxHealth) 
		{
			curHealth = maxHealth;
		}

		if (maxHealth < 1) 
		{
			maxHealth = 1;
		}

		healthBarLength = (Screen.width / 2) * (curHealth / (float) maxHealth);
	}	
}
