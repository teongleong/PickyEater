using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFalling : MonoBehaviour {

	public float fallSpeed = 10;
	RectTransform rectTransform;
	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 currPos = rectTransform.anchoredPosition;
		float newY = currPos.y - fallSpeed;
		rectTransform.anchoredPosition = new Vector2( currPos.x, newY);
	}



}
