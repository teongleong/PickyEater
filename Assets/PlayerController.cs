using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float leftLimit = -300f;
	public float rightLimit = 300f;
	public float speed = 5;
	RectTransform rt;
	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			Vector2 currentPosition = rt.anchoredPosition;
			float newX = currentPosition.x - speed;
			newX = Mathf.Clamp(newX, leftLimit, rightLimit);
			rt.anchoredPosition = new Vector2(newX, currentPosition.y);
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			Vector2 currentPosition = rt.anchoredPosition;
			float newX = currentPosition.x + speed;
			newX = Mathf.Clamp(newX, leftLimit, rightLimit);
			rt.anchoredPosition = new Vector2(newX, currentPosition.y);
		}
	}

}
