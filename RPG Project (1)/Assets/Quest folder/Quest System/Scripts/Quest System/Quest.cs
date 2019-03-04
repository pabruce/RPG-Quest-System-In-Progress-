using UnityEngine;
using System.Collections;

[System.Serializable]
public class Quest {

	public enum QuestProgress {NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETE, DONE}

	public string title;					//    String Title for the quest
	public int id;							//    ID number for the quest                
	public QuestProgress progress;			//    State of the current quest progress (enum)
	public string description;				//    String from the Quest Giver/Receiver
	public string hint;						//    String from the Quest Giver/Receiver
	public string congratulations;			//    String from the Quest Giver/Receiver
	public string summary;					//    String from the Quest Giver/Receiver
	public int nextQuest;					//    The next quest, if there is one (chain quests)
	public string questObjective;			//    Name if the item (Used to remove the item  as a doublecheck)
	public int questObjectiveCount;			//    Current number of questObjective Objects
	public int questObjectiveRequirement;	//    Required amount of questObjective Objects

	public int expReward;
	public int goldAward;
	public string itemReward;
}
