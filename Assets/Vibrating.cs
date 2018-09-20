using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrating : MonoBehaviour {

	public Vector2 targetPosition = Vector2.zero;
	public Vector2 targetDeviation = Vector2.zero;

	bool reached = false;
	float speed = 1;

	public Vector2 defaultPosition = Vector2.zero;

	RectTransform rt;
	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
		defaultPosition = rt.anchoredPosition;
		SetNewDirection();
	}

	void SetNewDirection() {
		targetDeviation = new Vector2(Random.value, Random.value);
		targetDeviation.Normalize();
		targetPosition = defaultPosition + targetDeviation * 10;
	}

	void SetNewDeviation() {
		targetDeviation = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
		targetPosition = defaultPosition + targetDeviation * 12;
		rt.anchoredPosition = targetPosition;
	}
	
	// Update is called once per frame
	void Update () {
		SetNewDeviation();
	}
}
