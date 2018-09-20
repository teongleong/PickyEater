using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatFood : MonoBehaviour {

	//public BarLengthManager goodBar;
	public BarLengthManager badBar;
	public BarLengthManager2 angerBar;

	public Vector2 goodFoodChange = new Vector2(10, 0);
	public Vector2 badFoodChange = new Vector2(0, 10);
	
	public float badFoodChangePercent = 1f / 20f;

	enum foodType { healthy, sinful, superFood, rottenFood, none };

	float healthyCombo = 0;
	float sinfulCombo = 0;
	float score = 0;
	int multiplier = 1;

	int [] thresholds = new int [] { 5, 10, 15, 25, 40, };

	public FoodSpawner foodSpawner;
	public MumFaceManager mumFaceManager;
	public Text badComboText;
	public Text goodComboText;

	public Text scoreText;
	public Text gameOverText;
	public Text multiplierText;

	public Sprite babyEat;

	public GameObject retryButton;

	void Start () {
		UpdateBadScore(sinfulCombo);
		UpdateGoodScore(healthyCombo);
		UpdateMultiplierText(multiplier);
	}
	
	foodType lastFoodType = foodType.none;

	void OnTriggerEnter2D(Collider2D other)
	{
		
		if (other.gameObject.tag == "GoodFood") {
			Debug.Log("Yuck");

			if (lastFoodType == foodType.healthy) {
				++healthyCombo;
				UpdateGoodScore(healthyCombo);
			}

			if (lastFoodType != foodType.healthy) {
				sinfulCombo = 0;
				UpdateBadScore(sinfulCombo);
				multiplier = 1;
				UpdateMultiplier();
			}

			lastFoodType = foodType.healthy;
			//UpdateBarLength(goodFoodChange);

		} else if (other.gameObject.tag == "BadFood") {
			Debug.Log("YOLO");

			if (lastFoodType == foodType.sinful) {
				++sinfulCombo;
				UpdateBadScore(sinfulCombo);
			}

			if (lastFoodType != foodType.sinful) {
				healthyCombo = 0;
				UpdateGoodScore(healthyCombo);
			}

			UpdateMultiplier();

			lastFoodType = foodType.sinful;
			UpdateBarLength(badFoodChangePercent);
			if (badBar.GetPercent() > 1) {
				gameOverText.gameObject.SetActive(true);
				Time.timeScale = 0;
			}
			if (angerBar.GetPercent() > 1) {
				gameOverText.gameObject.SetActive(true);
				Time.timeScale = 0;
			}
		} 
		Destroy(other.gameObject);
	}

	void UpdateMultiplier() {

		int multiplierTemp = multiplier;

		if (sinfulCombo > 0 ) {
			for (int i = 0; i < thresholds.Length; i++)
			{
				if (sinfulCombo > thresholds[i]) {
					multiplier = i + 1;
					//foodSpawner.redFoodSpawnRate = foodSpawner.defaultSpawnRate / multiplier;
					UpdateMultiplierText(multiplier);
				}
			}
		} else {
			UpdateMultiplierText(multiplier);
		}

		if (multiplierTemp != multiplier) {
			foodSpawner.fallingSpeed *= 1.1f;
		}
	}

	void UpdateBarLength(float deltaPercent) {
		//goodBar.ChangeLength(delta.y);
		//badBar.ChangeLength(delta.x);
		angerBar.ChangeLengthPercent(deltaPercent);
		Debug.Log("bar " + angerBar.GetPercent());
		Debug.Log("anger percent "+angerBar.GetPercent());
	}

	void UpdateScoreText(Text textObj, string label, float value) {
		textObj.text = label + value;
	}

	void UpdateGoodScore(float value) {
		UpdateScoreText(goodComboText, "good combo:", value);
	}

	void UpdateBadScore(float value) {
		UpdateScoreText(badComboText, "Combo:", value);
	}

	void UpdateScore(int value) {
		UpdateScoreText(scoreText, "Score: ", value);
	}

	void Reset() {
		UpdateBadScore(sinfulCombo);
		UpdateGoodScore(healthyCombo);
		gameOverText.gameObject.SetActive(false);
		Time.timeScale = 1;
	}

	void UpdateMultiplierText(int value) {
		UpdateScoreText(multiplierText, "Multiplier:", value);
		multiplierText.text += "X";
	}

	// Update is called once per frame
	void Update () {
		score += (Time.deltaTime * 10 * multiplier);
		//score /= (float) 10;
		UpdateScore((int) score);
		UpdateMumIcon();
	}

	void UpdateMumIcon() {
		if (angerBar.GetPercent() > 0.8f)
			mumFaceManager.SetAngerState(3);
		else if (angerBar.GetPercent() > 0.6f)
			mumFaceManager.SetAngerState(2);
		else if (angerBar.GetPercent() > 0.35f)
			mumFaceManager.SetAngerState(1);
		else 
			mumFaceManager.SetAngerState(0);
	}


}
