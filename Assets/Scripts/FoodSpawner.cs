using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {


	public GameObject GoodFoodPrefab;
	public GameObject BadFoodPrefab;
	public GameObject BadFoodPrefab2;

	public GameObject SuperFoodPrefab;

	public GameObject RottenFoodPrefab;

	public RectTransform canvasTransform;

	float defaultFallingSpeed = 0;
	public float fallingSpeed = 3;
	float acceleration = 0.1f;
	
	float defaultGreenFoodSpawnRate = 0f;
	float defaultRedFoodSpawnRate = 3f;
	public float greenFoodSpawnRate = 2f;
	float greenFoodSpawnInterval = 1 / 2f;

	float redFoodSpawnInterval = 3f;

	public float redFoodSpawnRate = 0.5f;

	public float spawnRateAccelation = 0.02f;

	public RectTransform redFoodSpawnPoint;
	public RectTransform greenFoodSpawnPoint;

	public RectTransform playerTransform;

	// Use this for initialization
	void Start () {
		//InvokeRepeating("SpawnBad", 0f, 1.5f);
		//InvokeRepeating("SpawnGood", 0.5f, 1.5f);
		// badLoop = StartCoroutine(RepeatSpawnBad());
		// goodLoop = StartCoroutine(RepeatSpawnGood());
		defaultFallingSpeed = fallingSpeed;

		defaultGreenFoodSpawnRate = greenFoodSpawnRate;
		defaultRedFoodSpawnRate = redFoodSpawnRate;

		redFoodSpawnInterval = 1 / redFoodSpawnRate;
		greenFoodSpawnInterval = 1 / greenFoodSpawnRate;

		
		Reset();
	}

	// public void StopAllCoroutines() {
	// 	if (badLoop != null) StopCoroutine(badLoop);
	// 	if (goodLoop != null) StopCoroutine(goodLoop);
	// }

	public void Reset() {
		fallingSpeed = defaultFallingSpeed;
		greenFoodSpawnRate = defaultGreenFoodSpawnRate;
		redFoodSpawnRate = defaultRedFoodSpawnRate;

		redFoodSpawnInterval = 1 / redFoodSpawnRate;
		greenFoodSpawnInterval = 1 / greenFoodSpawnRate;

		StopAllCoroutines();

		StartCoroutine(RepeatSpawnBad());
		StartCoroutine(RepeatSpawnGood());
	}
	
	// Update is called once per frame
	void Update () {
		fallingSpeed += Time.deltaTime * acceleration;
		redFoodSpawnRate += Time.deltaTime * spawnRateAccelation;
		redFoodSpawnInterval = 1 / redFoodSpawnRate;
		greenFoodSpawnInterval = 1 / greenFoodSpawnRate;
	}

	// 1 / (1 - e^-(x-c))
	float sigmoidFunction(float x, float c) {
		return (1 / (1 + Mathf.Exp((x - c) * -1)));
	}

	// S curve from 0 to 1;
	// float ScaledSigmoidFunction(float x) {
	// 	float y = sigmoidFunction(x, 10);
	// }

	float playerX() {
		return playerTransform.rect.x;
	}

	void SpawnBad()
    {
		GameObject newFood = null;
        if (Mathf.FloorToInt(Time.time) % 2 == 0) 
        	newFood = Instantiate(BadFoodPrefab, canvasTransform);
		else 
			newFood = Instantiate(BadFoodPrefab2, canvasTransform);

		RectTransform rt = newFood.GetComponent<RectTransform>();
		//rt.SetParent(canvasTransform);
		Vector2 currPos = rt.anchoredPosition;
		rt.localScale = Vector3.one;
		//float newX = Mathf.Clamp(playerX() + Random.Range(-100, 100), -400, 800);
		float newX = redFoodSpawnPoint.anchoredPosition.x;
		newX = Mathf.Clamp(newX, -400, 400);
		rt.anchoredPosition = new Vector2(newX, currPos.y);
		newFood.GetComponent<FoodFalling>().fallSpeed = fallingSpeed;
    }

	void SpawnGood()
    {
		GameObject newFood = null;
		float random = Random.value;
        if (random > 0.85f) 
        	newFood = Instantiate(SuperFoodPrefab, canvasTransform);
		else if (random > 0.6f) 
			newFood = Instantiate(RottenFoodPrefab, canvasTransform);
		else 
			newFood = Instantiate(GoodFoodPrefab, canvasTransform);

        //GameObject newFood = Instantiate(GoodFoodPrefab, canvasTransform);
		RectTransform rt = newFood.GetComponent<RectTransform>();
		//rt.SetParent(canvasTransform);
		Vector2 currPos = rt.anchoredPosition;
		rt.localScale = Vector3.one;
		float newX = greenFoodSpawnPoint.anchoredPosition.x;
		newX = Mathf.Clamp(newX, -400, 400);
		//float newX = Mathf.Clamp(playerX() + Random.Range(-100, 100), -300, 300);
		rt.anchoredPosition = new Vector2(newX, currPos.y);
		newFood.GetComponent<FoodFalling>().fallSpeed = fallingSpeed;
    }


	IEnumerator RepeatSpawnBad( ) {
		while( true )
		{
			SpawnBad();
			yield return new WaitForSeconds(redFoodSpawnInterval) ;
		}
	}

	IEnumerator RepeatSpawnGood( ) {
		//yield return new WaitForSeconds(time) ;
		while( true )
		{
			SpawnGood();
			yield return new WaitForSeconds(greenFoodSpawnInterval) ;
		}
	}
}
