using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLengthManager2 : MonoBehaviour {


	//float startY = 0;
	//float startLength = 0;
	public int maxLength = 1017;
	public float decayRate = 0.05f;

	public EatFood eatFood;
	
	RectTransform rt;
	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
	}

	public void Reset() {
		rt.sizeDelta = new Vector2 (0, rt.rect.height);
	}

	public int GetLength() {
		return (int) rt.rect.width;
	}

	public float GetPercent() {
		return GetLength() / (float) maxLength;
	}

	public void ChangeLength(float delta) {
		float newWidth = rt.rect.width + delta;
		if (newWidth < 0) newWidth = 0;
		rt.sizeDelta = new Vector2 (newWidth, rt.rect.height);	
	}

	// deltaPercent range from 0 to 1 (1 is 100%)
	public void ChangeLengthPercent(float deltaPercent) {
		ChangeLength(deltaPercent * maxLength);
	}
	
	// Update is called once per frame
	void Update () {
		if (eatFood.gameOver) return;
		// anger decay
		ChangeLengthPercent(-1 * Time.deltaTime * decayRate);
	}
	
}
