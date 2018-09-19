using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatFood : MonoBehaviour {

	//public BarLengthManager goodBar;
	public BarLengthManager badBar;

	public Vector2 goodFoodChange = new Vector2(10, 0);
	public Vector2 badFoodChange = new Vector2(0, 10);
	
	enum foodType { healthy, sinful, none };

	float healthyCombo = 0;
	float sinfulCombo = 0;
	float score = 0;
	int multiplier = 1;

	int [] thresholds = new int [] { 3, 7, 11, 18, 25, 39};

	public FoodSpawner foodSpawner;
	public Text badComboText;
	public Text goodComboText;

	public Text scoreText;
	public Text gameOverText;
	public Text multiplierText;

	public Sprite eat;

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
			UpdateBarLength(goodFoodChange);

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
			UpdateBarLength(badFoodChange);
			if (badBar.GetPercent() > 1) {
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
					foodSpawner.repeatRate = foodSpawner.defaultRepeatRate / multiplier;
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

	void UpdateBarLength(Vector2 delta) {
		//goodBar.ChangeLength(delta.y);
		badBar.ChangeLength(delta.x);
		Debug.Log("bar bar " + badBar.GetPercent());
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
	}


}
