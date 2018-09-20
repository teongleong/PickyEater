using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashNSeconds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowAndHide() {
		gameObject.SetActive(true);
		StartCoroutine(DelayedHide());
	}

	IEnumerator DelayedHide() {
		yield return new WaitForSeconds(1) ;
		gameObject.SetActive(false);
	}
}
