using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour 
{
	float moveSpeed = 10f;

	Vector3 curPos, lastPos;

	// Use this for initialization
	void Start () 
	{
		if (GameManager.instance.nextSpawnPoint != "") {
			GameObject spawnPoint = GameObject.Find (GameManager.instance.nextSpawnPoint);
			transform.position = spawnPoint.transform.position;

			GameManager.instance.nextSpawnPoint = "";
		} 
		else if (GameManager.instance.lastHeroPosition != Vector3.zero) 
		{
			transform.position = GameManager.instance.lastHeroPosition;
			GameManager.instance.lastHeroPosition = Vector3.zero;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float moveX = Input.GetAxis ("Horizontal");
		float moveZ = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveX, 0.0f, moveZ);
		GetComponent<Rigidbody> ().velocity = movement * moveSpeed;

		curPos = transform.position;
		if (curPos == lastPos) 
		{
			GameManager.instance.isWalking = false;
		} 
		else
		{
			GameManager.instance.isWalking = true;
		}
		lastPos = curPos;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "teleporter") 
		{
			CollisionHandler col = other.gameObject.GetComponent<CollisionHandler> ();
			GameManager.instance.nextSpawnPoint = col.spawnPointName;
			GameManager.instance.sceneToLoad = col.sceneToLoad;
			GameManager.instance.LoadNextScene();
		}

//		if (other.tag == "EnterTown") 
//		{
//			CollisionHandler col = other.gameObject.GetComponent<CollisionHandler> ();
//			GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
//			GameManager.instance.sceneToLoad = col.sceneToLoad;
//			GameManager.instance.LoadNextScene();
//		}
//
//		if (other.tag == "LeaveTown") 
//		{
//			CollisionHandler col = other.gameObject.GetComponent<CollisionHandler> ();
//			GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
//			GameManager.instance.sceneToLoad = col.sceneToLoad;
//			GameManager.instance.LoadNextScene();
//		}

		if (other.tag == "region1") 
		{
			GameManager.instance.curRegions = 0;
		}

		if (other.tag == "region2") 
		{
			GameManager.instance.curRegions = 1;
		}
		if (other.tag == "region3") 
		{
			GameManager.instance.curRegions = 2;
		}
		if (other.tag == "region4") 
		{
			GameManager.instance.curRegions = 3;
		}
		if (other.tag == "region5") 
		{
			GameManager.instance.curRegions = 4;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "region1" || other.tag == "region2" || other.tag == "region3" || other.tag == "region4" || other.tag == "region5") 
		{
			GameManager.instance.canGetEncounter = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "region1" || other.tag == "region2" || other.tag == "region3" || other.tag == "region4" || other.tag == "region5") 
		{
			GameManager.instance.canGetEncounter = false;
		}
	}
}
