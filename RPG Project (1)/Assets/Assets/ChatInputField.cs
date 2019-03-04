using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChatInputField : InputField
{
	protected override void LateUpdate()
	{
		base.LateUpdate ();

		if (EventSystem.current != null) 
		{
			if (this.gameObject.Equals (EventSystem.current.currentSelectedGameObject)) 
			{
				MoveTextEnd (false);
			}
		}
	}
}
