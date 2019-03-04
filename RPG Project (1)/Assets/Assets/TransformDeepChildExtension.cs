using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformDeepChildExtension 
{

	public static Transform FindDeepChild(this Transform parent, string name)
	{
		//direct child?
		Transform result = parent.Find(name);
		if (result != null)
		{
			return result;	
		}

		//search rest of tree
		foreach (Transform child in parent) 
		{
			result = child.FindDeepChild(name);
			if (result != null) 
			{
				return result;
			}
		}

		// not found
		return null;
	}
}
