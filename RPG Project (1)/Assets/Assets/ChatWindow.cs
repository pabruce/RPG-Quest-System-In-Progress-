using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChatWindow : MonoBehaviour 
{
	public Font font; 
	public int fontSize = 14;

	private Text chatText;
	private InputField input;
	private Scrollbar vscroll;
	private ScrollRect scrollRect;  
	private int maxLines = 80;

	// Use this for initialization
	void Start ()
	{
		chatText = GameObject.Find ("scrollText").GetComponent<Text>();

		input = GameObject.Find ("ChatInputField").GetComponent<InputField>();
		input.text = "";
		input.onEndEdit.AddListener (delegate {InputEntered();});

		vscroll = GameObject.Find ("VScroll").GetComponent<Scrollbar>();
		vscroll.value = 0;

		scrollRect = GameObject.Find ("ScrollMask").GetComponent<ScrollRect>();
		if (scrollRect) 
		{
			if ((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.WindowsEditor)) {
				scrollRect.scrollSensitivity = 25;
			}
			else 
			{
				scrollRect.scrollSensitivity = 1;
			}
		}

		//Set font size as it may be overwritten

		Component[] components = GetComponentsInChildren<Text>();
		foreach(Text text in components)
		{
			if (! text.name.Equals("Title")) 
			{
				text.font = this.font;
				text.fontSize = this.fontSize;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyUp (KeyCode.Slash)) 
		{
			if (!input.isFocused) 
			{
				input.ActivateInputField();
				input.Select();
				input.text = "/";
			}

		}
	}

	public void InputEntered()
	{
		AddString(input.text);
		input.text = "";
	}

	public void AddString(string Message)
	{
		chatText.text += "\n" + Message;

		if (chatText.cachedTextGenerator.lineCount > maxLines) 
		{
			string oldText = chatText.text;

			chatText.text = oldText.Substring (chatText.cachedTextGenerator.lines [1].startCharIdx);
		}
	}
}
