using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MumFaceManager : MonoBehaviour {

	public GameObject [] faces;
	// Use this for initialization

	int lastState = 0;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void HideAll() {
		for (int i = 0; i < faces.Length; i++)
		{
			faces[i].SetActive(false);
		}
	}

	public void SetAngerState(int state) {
		if (lastState != state) {
			HideAll();
			faces[state].SetActive(true);
			lastState = state;
		}
	}
}
