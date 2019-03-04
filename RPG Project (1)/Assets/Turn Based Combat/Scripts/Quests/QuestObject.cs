//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class QuestObject : MonoBehaviour {
//
//	private bool inTrigger = false;
//
//	public List<int> availableQuestIDs = new List<int>();
//	public List<int> receivableQuestIDs = new List<int>();
//
//	public GameObject questMarker;
//	public Image theImage;
//
//	public Sprite questAvailableSprite;
//	public Sprite questReceivableSprite;
//
//	// Use this for initialization
//	void Start ()
//	{
//		SetQuestMarker ();
//	}
//
//	void SetQuestMarker()
//	{
//		if (QuestManager.questManager.CheckCompletedQuests (this)) {
//			questMarker.SetActive (true);
//			theImage.sprite = questReceivableSprite;
//			theImage.color = Color.yellow;
//		} else if (QuestManager.questManager.CheckAvailableQuests (this)) {
//			questMarker.SetActive (true);
//			theImage.sprite = questAvailableSprite;
//			theImage.color = Color.yellow;
//		} else if (QuestManager.questManager.CheckAcceptedQuests (this)) {
//			questMarker.SetActive (true);
//			theImage.sprite = questReceivableSprite;
//			theImage.color = Color.gray;
//		} else {
//			questMarker.SetActive (false);
//		}
//	}
//
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		if (inTrigger && Input.GetKeyDown (KeyCode.Space)) 
//		{
//			//quest ui manager
//			QuestManager.questManager.QuestRequest(this);
//		}
//	}
//
//	void OnTriggerEnter(Collider other)
//	{
//		if (other.tag == "Player") 
//		{
//			inTrigger = true;
//		}
//	}
//
//	void OnTriggerExit(Collider other)
//	{
//		if (other.tag == "Player") 
//		{
//			inTrigger = false;
//		}
//	}
//}
