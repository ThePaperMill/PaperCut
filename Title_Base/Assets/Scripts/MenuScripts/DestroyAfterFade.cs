/*********************************
 * DestroyAfterFade.cs
 * Troy
 * Created 9/4/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using UnityEngine.UI;

public class DestroyAfterFade : MonoBehaviour
{
	
	public Image image;
	[SerializeField] private bool m_DetachChildren = false;

	// Use this for initialization
	void Start ()
	{
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(image)
		{
			if (image.color.a == 0)
			{
				if (m_DetachChildren)
				{
					transform.DetachChildren();
				}
				
				DestroyObject(gameObject);
			}
		}
	}
}