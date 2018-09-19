using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {


	public GameObject GoodFoodPrefab;
	public GameObject BadFoodPrefab;

	public RectTransform canvasTransform;

	public float fallingSpeed = 3;
	float acceleration = 0.1f;

	public RectTransform playerTransform;


	// Use this for initialization
	void Start () {
		//InvokeRepeating("SpawnBad", 0f, 1.5f);
		//InvokeRepeating("SpawnGood", 0.5f, 1.5f);
		StartCoroutine(RepeatSpawnBad());
		StartCoroutine(RepeatSpawnGood());
	}
	
	// Update is called once per frame
	void Update () {
		fallingSpeed += Time.deltaTime * acceleration;
	}

	float playerX() {
		return playerTransform.rect.x;
	}

	void SpawnBad()
    {
        GameObject newFood = Instantiate(BadFoodPrefab, canvasTransform);
		RectTransform rt = newFood.GetComponent<RectTransform>();
		//rt.SetParent(canvasTransform);
		Vector2 currPos = rt.anchoredPosition;
		rt.localScale = Vector3.one;
		float newX = Mathf.Clamp(playerX() + Random.Range(-100, 100), -400, 800);
		rt.anchoredPosition = new Vector2(newX, currPos.y);
		newFood.GetComponent<FoodFalling>().fallSpeed = fallingSpeed;
    }

	void SpawnGood()
    {
        GameObject newFood = Instantiate(GoodFoodPrefab, canvasTransform);
		RectTransform rt = newFood.GetComponent<RectTransform>();
		//rt.SetParent(canvasTransform);
		Vector2 currPos = rt.anchoredPosition;
		rt.localScale = Vector3.one;
		float newX = Mathf.Clamp(playerX() + Random.Range(-100, 100), -300, 300);
		rt.anchoredPosition = new Vector2(newX, currPos.y);
		newFood.GetComponent<FoodFalling>().fallSpeed = fallingSpeed;
    }

	public float defaultRepeatRate = 3f;
	public float repeatRate = 3f;

	IEnumerator RepeatSpawnBad( ) {
		yield return new WaitForSeconds(1.5f) ;
		while( true )
		{
			SpawnBad();
			yield return new WaitForSeconds(repeatRate) ;
		}
	}

	IEnumerator RepeatSpawnGood( ) {
		//yield return new WaitForSeconds(time) ;
		while( true )
		{
			SpawnGood();
			yield return new WaitForSeconds(repeatRate) ;
		}
	}
}
