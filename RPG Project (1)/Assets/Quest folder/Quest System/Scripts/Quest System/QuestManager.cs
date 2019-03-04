using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	public static QuestManager questManager;

	public List <Quest> questList = new List<Quest>();			// Master Quest List
	public List <Quest> currentQuestList = new List<Quest>();	// Current Quest List

	private QuestObject currentQuestObject;						// the pointer to the current quest giver being processed
	private QuestObject newQuestObject;							// the pointer to a new quest giver to be pocessed

	void Awake()
	{
		// check if WSM exists
		if (questManager == null)
		{
			//if not set it to this
			questManager = this;
		}
		//if WSM already exists and is not this
		else if (questManager != this)
		{
			//destroy it
			Destroy(gameObject);
		}
		//set this to not getting destroyed when loading another scene

		DontDestroyOnLoad(gameObject);
	}

	//public void QuestRequest(QuestObject thisQuestObject, int questGiversIDs, int questReceiversIDs)
	public void QuestRequest(QuestObject thisQuestObject)
	{
		currentQuestObject = thisQuestObject;
		//check if the incoming ids stats and show the correct data in the GUI

		//available quests
		if (currentQuestObject.availableQuestIDs.Count > 0)
		{
			for (int i = 0; i < questList.Count; i++)
			{
				for (int j = 0; j < currentQuestObject.availableQuestIDs.Count;j++)
				//if the id is available in the questlist && available
				if (questList[i].id == currentQuestObject.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.AVAILABLE)
				{
					//show that in the GUI
					Debug.Log("Quest ID: " + currentQuestObject.availableQuestIDs[j] + " " + questList[i].progress);
					//testme
					//AcceptQuest(thisQuestObject.availableQuestIDs[i]);
					QuestUIManager.uiManager.questAvailable = true;
					QuestUIManager.uiManager.availableQuests.Add(questList[i]);
				}
			}
		}
		//active quests
		for (int i = 0; i < currentQuestList.Count; i++)
		{
			for (int j = 0; j < currentQuestObject.receivableQuestIDs.Count;j++)
			//for(int j = 0; j< currentQuestObject.receivableQuestIDs)
			if (currentQuestList[i].id == currentQuestObject.receivableQuestIDs[j] && (currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED || currentQuestList[i].progress == Quest.QuestProgress.COMPLETE))//or completed?
			{
				//show active quests in the GUI too
				Debug.Log("Quest ID: "+ currentQuestObject.receivableQuestIDs[j] + " is currently " + currentQuestList[i].progress);
				//CompleteQuest(thisQuestObject.receivableQuestIDs[i]);

				QuestUIManager.uiManager.questRunning = true;
				QuestUIManager.uiManager.activeQuests.Add(currentQuestList[i]);
			}
		}
	}

	public bool CheckForAvailableQuests(QuestObject thisQuestObject)
	{
		for (int i = 0; i < questList.Count; i++)
		{
			for (int j = 0; j < thisQuestObject.availableQuestIDs.Count; j++)
			{
				//if the id is available in the questlist && available
				if (questList[i].id == thisQuestObject.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.AVAILABLE)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckForAcceptedQuests(QuestObject thisQuestObject)
	{
		for (int i = 0; i < questList.Count; i++)
		{
			for (int j = 0; j < thisQuestObject.receivableQuestIDs.Count; j++)
			{
				//if the id is available in the questlist && available
				if (questList[i].id == thisQuestObject.receivableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.ACCEPTED)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckForCompletedQuests(QuestObject thisQuestObject)
	{
		for (int i = 0; i < questList.Count; i++)
		{
			for (int j = 0; j < thisQuestObject.receivableQuestIDs.Count; j++)
			{
				//if the id is available in the questlist && available
				if (questList[i].id == thisQuestObject.receivableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.COMPLETE)
				{
					return true;
				}
			}
		}
		return false;
	}



	public void AcceptQuest(int questID)// on button click
	{
		for (int i = 0; i < questList.Count; i++)
		{
			if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE)
			{
				currentQuestList.Add(questList[i]);
				questList[i].progress = Quest.QuestProgress.ACCEPTED;
			}
		}
	}

	public void GiveUpQuest(int questID)//giveup the quest
	{
		for (int i = 0; i < currentQuestList.Count; i++)
		{
			if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED)
			{
				currentQuestList[i].progress = Quest.QuestProgress.AVAILABLE;
				currentQuestList[i].questObjectiveCount = 0;
				currentQuestList.Remove(currentQuestList[i]);
			}
		}
	}

	public void CompleteQuest(int questID)
	{
		for (int i = 0; i < currentQuestList.Count; i++)
		{
			if (currentQuestList[i].id == questID &&  currentQuestList[i].progress == Quest.QuestProgress.COMPLETE)
			{
				currentQuestList[i].progress = Quest.QuestProgress.DONE;
				currentQuestList.Remove(currentQuestList[i]);

				//give rewards later here
			}
		}
		//ACTIVATE A CHAIN QUEST - IF THERE IS ANY
		CheckChainQuest(questID);
	}

	private void CheckChainQuest(int questID)
	{
		int num = 0;
		for (int i = 0; i < questList.Count; i++)
		{
			if (questList[i].id == questID && questList[i].nextQuest > 0)
			{
				num = questList[i].nextQuest;
			}
		}

		if (num > 0)
		{
			for (int i = 0; i < questList.Count; i++)
			{
				if (questList[i].id == num && questList[i].progress == Quest.QuestProgress.NOT_AVAILABLE)
				{
					questList[i].progress = Quest.QuestProgress.AVAILABLE;
				}
			}
		}

	}


	public void AddQuestItem(string questObjective,int itemAmount)// add items or kill amount or whatever  // name of the item, amount to add
	{
		//loop through the existing quests and check if there is any active one with this item requested
		for (int i = 0; i < currentQuestList.Count; i++)
		{
			if (currentQuestList[i].questObjective == questObjective && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) // if is a searched item && quest is on accepted state
			{
				currentQuestList[i].questObjectiveCount += itemAmount;//increase item by passed amount
				//later also add it to an inventory if needed
			}

			//check if the quest is now completed
			if (currentQuestList[i].questObjectiveCount >= currentQuestList[i].questObjectiveRequirement && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED)
			{
				currentQuestList[i].progress = Quest.QuestProgress.COMPLETE;
			}

		}
	}


	public void RemoveQuestItem()//remove an item from inventory later
	{
		//Iterate through your Inventory and remove the correct item and item amount if its there.
	}

	public void ShowQuestLog(int questID)
	{
		for (int i = 0; i < currentQuestList.Count; i++)
		{

			if (currentQuestList[i].id == questID)
			{
				QuestUIManager.uiManager.showQuestLog(currentQuestList[i]);
			}
		}
	}


//------------------------------------------------------------------BOOLS FOR BUTTONS-------------------------------------------------------------
	public bool RequestAvailableQuest(int questID)
	{
		for (int i = 0; i < questList.Count; i++)
		{
		//if the id is available in the questlist && available
		if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE)
			{
				return true;
			}
		}
		return false;
	}

	public bool RequestAcceptedQuest(int questID)
	{
		for (int i = 0; i < questList.Count; i++)
		{
			//if the id is available in the questlist && available
			if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.ACCEPTED)
			{
				return true;
			}
		}
		return false;
	}

	public bool RequestCompletedQuest(int questID)
	{
		for (int i = 0; i < questList.Count; i++)
		{
			//if the id is available in the questlist && available
			if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.COMPLETE)
			{
				return true;
			}
		}
		return false;
	}
}
