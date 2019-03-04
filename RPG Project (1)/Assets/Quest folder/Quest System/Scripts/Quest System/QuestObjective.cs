using UnityEngine;
using System.Collections;

public class QuestObjective : MonoBehaviour {

	public string itemName;// currently the System uses strings.
	public int itemID;//currently not used, but you can easily change the system from strings to ID's

	//Once collided with an QuestObjective, it will check if it's possible to add it to the quests
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			QuestManager.questManager.AddQuestItem(itemName, 1);
			//Debug.Log("Item: " + itemName + " Collected");
			QuestObject[] NPCs = GameObject.FindObjectsOfType(typeof(QuestObject)) as QuestObject[];
			foreach (QuestObject obj in NPCs)
			{
				obj.SetQuestMarker();
			}
		}
	}
/*
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			Debug.Log("no hero nearby.");
		}
	}
*/
}
