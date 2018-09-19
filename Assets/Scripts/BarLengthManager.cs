using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLengthManager : MonoBehaviour {


	//float startY = 0;
	//float startLength = 0;
	public int maxLength = 1017;
	
	RectTransform rt;
	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
	//	startY = rt.anchoredPosition.y;
	//	startLength = rt.rect.height;
	}

	public int GetLength() {
		return (int) rt.rect.height;
	}

	public float GetPercent() {
		return GetLength() / (float) maxLength;
	}

	public void ChangeLength(float delta) {
		float newHeight = rt.rect.height + delta;
		if (newHeight < 10) newHeight = 10;
		rt.sizeDelta = new Vector2 (rt.rect.width, newHeight);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}
