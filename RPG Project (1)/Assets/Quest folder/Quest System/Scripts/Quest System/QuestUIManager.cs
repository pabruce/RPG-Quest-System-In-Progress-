using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour {

	public static QuestUIManager uiManager;

	public bool questAvailable = false;
	public bool questRunning = false;
	private bool panelActive = false;
	private bool questLogPanel = false;

	//PANEL
	public GameObject QuestPanel;
	public GameObject QuestLogPanel;

	//QuestObject - NOT NEEDED
	//private QuestObject currentQuestObject;

	//QUESTLISTS
	public List<Quest> availableQuests = new List<Quest>();
	public List<Quest> activeQuests = new List<Quest>();

	//BUTTONS
	public GameObject QButton;
	public GameObject QLogButton;
	private List<GameObject> QButtons = new List<GameObject>();

	public GameObject acceptButton;
	public GameObject giveupButton;
	public GameObject completeButton;

	//SPACER
	public Transform QButtonSpacer;//availables
	public Transform QButtonSpacer2;//actives
	public Transform QLogButtonSpacer;//actives

	//ALL QUEST INFOS
	public Text questTitle;
	public Text questDescription;
	public Text questSummary;

	//ALL QUEST LOG INFOS
	public Text questLogTitle;
	public Text questLogDescription;
	public Text questLogSummary;

	//BUTTONS BOTTOM
	public QButtonScript acceptButtonScript;
	public QButtonScript giveupButtonScript;
	public QButtonScript completeButtonScript;

	void Awake()
	{
		// check if UI exists
		if (uiManager == null)
		{
			//if not set it to this
			uiManager = this;
		}
		//if UI already exists and is not this
		else if (uiManager != this)
		{
			//destroy it
			Destroy(gameObject);
		}
		//set this to not getting destroyed when loading another scene

		DontDestroyOnLoad(gameObject);
		//FIND PANEL
		QuestPanel = GameObject.FindGameObjectWithTag("QuestPanel");
		QuestLogPanel = GameObject.FindGameObjectWithTag("QuestLogPanel");
		//HIDE PANEL
		HideQuestPanel();
		HideQuestLogPanel();
	}

	void Start()
	{
		acceptButton = GameObject.Find("QuestCanvas").transform.FindChild("QuestPanel").transform.FindChild("QuestDescription").transform.FindChild("GameObject").transform.FindChild("AcceptButton").gameObject;
		acceptButtonScript = acceptButton.GetComponent<QButtonScript>();

		//giveupButton = GameObject.FindGameObjectWithTag("GiveUpButton");
		giveupButton = GameObject.Find("QuestCanvas").transform.FindChild("QuestPanel").transform.FindChild("QuestDescription").transform.FindChild("GameObject").transform.FindChild("GiveUpButton").gameObject;
		giveupButtonScript = giveupButton.GetComponent<QButtonScript>();

		//completeButton = GameObject.FindGameObjectWithTag("CompleteButton");
		completeButton = GameObject.Find("QuestCanvas").transform.FindChild("QuestPanel").transform.FindChild("QuestDescription").transform.FindChild("GameObject").transform.FindChild("CompleteButton").gameObject;
		completeButtonScript = completeButton.GetComponent<QButtonScript>();

		acceptButton.SetActive(false);
		giveupButton.SetActive(false);
		completeButton.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			questLogPanel = !questLogPanel;//switch
			ShowQuestLogPanel();
		}
	}

//------------------------------------------------CALLED FROM THE QUESTOBJECT----------------------------------------------
	public void CheckQuests(QuestObject questObject)
	{
		//currentQuestObject = questObject;//NOT NEEDED
		QuestManager.questManager.QuestRequest(questObject);


		if ((questRunning || questAvailable) && !panelActive)
		{
			ShowQuestPanel();
		}
		else
		{
			//show anything here you like, maybe a talk bubble about the weather
			Debug.Log("No Quest Available");
		}
	}
//------------------------------------------------------------------SHOW PANELS---------------------------------------------------------
	public void ShowQuestPanel()
	{
		panelActive = true;

		QuestPanel.SetActive(panelActive);
		//fill in data
		fillQuestButtons();
	}

	public void ShowQuestLogPanel()
	{
		QuestLogPanel.SetActive(questLogPanel);

		if (questLogPanel && !panelActive)
		{
			foreach (Quest curQuest in QuestManager.questManager.currentQuestList)
			{
				GameObject questButton = Instantiate(QLogButton);
				QLogButtonScript qButton = questButton.GetComponent<QLogButtonScript>();

				qButton.questID = curQuest.id;
				qButton.myText.text = curQuest.title;

				questButton.transform.SetParent(QLogButtonSpacer, false);
				QButtons.Add(questButton);
			}
		}
		else if (!questLogPanel && !panelActive)
		{
			HideQuestLogPanel();
		}
	}

	//-------------------------------------------------------------CLOSE AND RESET ALL PANELS----------------------------------------
	public void HideQuestLogPanel()
	{
		questLogPanel = false;

		questLogTitle.text = "";
		questLogDescription.text = "";
		questLogSummary.text = "";
		//clear buttons list
		for (int i = 0; i < QButtons.Count; i++)
		{
			Destroy(QButtons[i]);
		}

		QButtons.Clear();
		QuestLogPanel.SetActive(questLogPanel);
	}

	public void HideQuestPanel()
	{
		panelActive = false;
		questAvailable = false;
		questRunning = false;

		//clear text
		questTitle.text = "";
		questDescription.text = "";
		questSummary.text = "";
		//clear all lists
		availableQuests.Clear();
		activeQuests.Clear();
		//clear buttons list
		for (int i = 0; i < QButtons.Count; i++)
		{
			Destroy(QButtons[i]);
		}
			

		QButtons.Clear();
		//hide panel
		QuestPanel.SetActive(panelActive);
	}
	//-------------------------------------------------------------FILL BUTTONS FOR QUEST PANELS----------------------------------------
	void fillQuestButtons()
	{
		foreach (Quest availableQuest in availableQuests)
		{
			GameObject questButton = Instantiate(QButton);
			QButtonScript qButton = questButton.GetComponent<QButtonScript>();

			qButton.questID = availableQuest.id;
			qButton.myText.text = availableQuest.title;

			questButton.transform.SetParent(QButtonSpacer, false);
			QButtons.Add(questButton);
		}

		foreach (Quest activeQuest in activeQuests)
		{
			GameObject questButton = Instantiate(QButton);
			QButtonScript qButton = questButton.GetComponent<QButtonScript>();

			qButton.questID = activeQuest.id;
			qButton.myText.text = activeQuest.title;

			questButton.transform.SetParent(QButtonSpacer2, false);
			QButtons.Add(questButton);
		}
	}

	//---------------------------------------------------------SHOW QUEST ON BUTTON PRESS IN QUEST PANEL----------------------------------------------
	public void showSelectedQuest(int questID)
	{
		for (int i = 0; i < availableQuests.Count; i++)
		{
			if (availableQuests[i].id == questID)
			{
				questTitle.text = availableQuests[i].title;

				if(availableQuests[i].progress == Quest.QuestProgress.AVAILABLE)
				{
					questDescription.text = availableQuests[i].description;
					questSummary.text = availableQuests[i].questObjective + ": " + availableQuests[i].questObjectiveCount + " / " + availableQuests[i].questObjectiveRequirement;
				}
			}
		}

		for (int i = 0; i < activeQuests.Count; i++)
		{
			if (activeQuests[i].id == questID)
			{
				questTitle.text = activeQuests[i].title;
				if(activeQuests[i].progress == Quest.QuestProgress.ACCEPTED)
				{
					questDescription.text = activeQuests[i].hint;
					questSummary.text = activeQuests[i].questObjective + ": " + activeQuests[i].questObjectiveCount + " / " + activeQuests[i].questObjectiveRequirement;
				}
				else if(activeQuests[i].progress == Quest.QuestProgress.COMPLETE)
				{
					questDescription.text = activeQuests[i].congratulations;
					questSummary.text = activeQuests[i].questObjective + ": " + activeQuests[i].questObjectiveCount + " / " + activeQuests[i].questObjectiveRequirement;
				}
			}
		}
	}

	//---------------------------------------------------------SHOW QUEST LOG FOR A RUNNING SELECTED QUEST----------------------------------------------

	public void showQuestLog(Quest activeQuest)
	{
		questLogTitle.text = activeQuest.title;

		if (activeQuest.progress == Quest.QuestProgress.ACCEPTED)
			{
				questLogDescription.text = activeQuest.hint;
				questLogSummary.text = activeQuest.questObjective + ": " + activeQuest.questObjectiveCount + " / " + activeQuest.questObjectiveRequirement;
			}
		else if (activeQuest.progress == Quest.QuestProgress.COMPLETE)
			{
				questLogDescription.text = activeQuest.congratulations;
				questLogSummary.text = activeQuest.questObjective + ": " + activeQuest.questObjectiveCount + " / " + activeQuest.questObjectiveRequirement;
			}
	}
}
