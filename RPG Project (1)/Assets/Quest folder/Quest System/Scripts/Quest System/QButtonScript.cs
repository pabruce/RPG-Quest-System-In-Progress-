using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QButtonScript : MonoBehaviour {

	public int questID;
	//public string questText;
	public Text myText;

	void Awake()
	{

	}
		
	void Start()
	{

	}

	public void ShowAllInfos()// on click event for the left buttons
	{
		QuestUIManager.uiManager.showSelectedQuest(questID);
		// pass over data to: 
		//accept button
		if (QuestManager.questManager.RequestAvailableQuest(questID))
		{
			QuestUIManager.uiManager.acceptButton.SetActive(true);
			//Debug.Log("Accept Button visible = " + acceptButton.activeSelf);
			QuestUIManager.uiManager.acceptButtonScript.questID = questID;

			Debug.Log("Quest: " + questID + " can  be accepted");
		}
		else
		{
			QuestUIManager.uiManager.acceptButton.SetActive(false);
		}
		// GIVE UP BUTTON
		if (QuestManager.questManager.RequestAcceptedQuest(questID))
		{
			QuestUIManager.uiManager.giveupButton.SetActive(true);
			QuestUIManager.uiManager.giveupButtonScript.questID = questID;
		}
		else
		{
			QuestUIManager.uiManager.giveupButton.SetActive(false);
		}

		// COMPLETE BUTTON
		if (QuestManager.questManager.RequestCompletedQuest(questID))
		{
			QuestUIManager.uiManager.completeButton.SetActive(true);
			QuestUIManager.uiManager.completeButtonScript.questID = questID;
		}
		else
		{
			QuestUIManager.uiManager.completeButton.SetActive(false);
		}
	}

	public void acceptQuest()
	{
		QuestManager.questManager.AcceptQuest(questID);
		QuestUIManager.uiManager.HideQuestPanel();
		//UPDATE QUEST GIVERS EVENTS
		QuestObject[] currentQuestGuys = FindObjectsOfType(typeof(QuestObject)) as QuestObject[];
		foreach (QuestObject obj in currentQuestGuys)
		{
			obj.SetQuestMarker();
		}

		QuestUIManager.uiManager.acceptButton.SetActive(false);

	}

	public void giveUpQuest()
	{
		QuestManager.questManager.GiveUpQuest(questID);
		QuestUIManager.uiManager.HideQuestPanel();
		//UPDATE QUEST GIVERS EVENTS
		QuestObject[] currentQuestGuys = FindObjectsOfType(typeof(QuestObject)) as QuestObject[];
		foreach (QuestObject obj in currentQuestGuys)
		{
			obj.SetQuestMarker();
		}

		QuestUIManager.uiManager.giveupButton.SetActive(false);
	}

	public void completeQuest()
	{
		QuestManager.questManager.CompleteQuest(questID);
		QuestUIManager.uiManager.HideQuestPanel();
		//UPDATE QUEST GIVERS EVENTS
		QuestObject[] currentQuestGuys = FindObjectsOfType(typeof(QuestObject)) as QuestObject[];
		foreach (QuestObject obj in currentQuestGuys)
		{
			obj.SetQuestMarker();
		}

		QuestUIManager.uiManager.completeButton.SetActive(false);
	}

	public void ClosePanel()
	{
		QuestUIManager.uiManager.HideQuestPanel();
		//UPDATE QUEST GIVERS EVENTS
		QuestObject[] currentQuestGuys = FindObjectsOfType(typeof(QuestObject)) as QuestObject[];
		foreach (QuestObject obj in currentQuestGuys)
		{
			obj.SetQuestMarker();
		}

		QuestUIManager.uiManager.acceptButton.SetActive(false);
		QuestUIManager.uiManager.giveupButton.SetActive(false);
		QuestUIManager.uiManager.completeButton.SetActive(false);
	}
}
