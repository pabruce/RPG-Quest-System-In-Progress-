using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class WindowDrag : MonoBehaviour, IDragHandler
{

	private RectTransform m_transform = null;
	private float plane_distance = 0;

	// Use this for initialization
	void Start () 
	{
		m_transform = GetComponent<RectTransform>();
		Canvas canvas = GetComponentInParent<Canvas>();
		plane_distance = canvas.planeDistance;
	}
	
	// Update is called once per frame
	public void OnDrag(PointerEventData eventData) 
	{
		Vector3 screenpoint = Input.mousePosition;
		screenpoint.z = plane_distance;

		Vector3 worldPos = Camera.main.ScreenToWorldPoint (screenpoint);

		m_transform.position = worldPos;
	}
}
