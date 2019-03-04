using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	static DontDestroy dontDestroy;

	void Awake()
	{
		if (dontDestroy == null)
		{
			//if not set it to this
			dontDestroy = this;
		}
		//if WSM already exists and is not this
		else if (dontDestroy != this)
		{
			//destroy it
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

}
