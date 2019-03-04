using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Text associatedText;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (associatedText != null)
		{
			associatedText.enabled = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(associatedText != null)
		{
			associatedText.enabled = false;
		}
	}
}
