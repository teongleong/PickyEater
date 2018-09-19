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

	public FoodSpawner foodSpawner;
	public Text badComboText;
	public Text goodComboText;

	public Text scoreComboText;

	void Start () {
		UpdateBadScore(sinfulCombo);
		UpdateGoodScore(healthyCombo);
	}
	
	// Update is called once per frame
	void Update () {
		
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

			if (sinfulCombo > 0 && sinfulCombo % 5 == 0) {
				foodSpawner.fallingSpeed *= 1.2f;
			}

			lastFoodType = foodType.sinful;
			UpdateBarLength(badFoodChange);
		}
		Destroy(other.gameObject);
	}

	void UpdateBarLength(Vector2 delta) {
		//goodBar.ChangeLength(delta.y);
		badBar.ChangeLength(delta.x);
	}

	void UpdateScoreText(Text textObj, string label, float value) {
		textObj.text = label + value;
	}

	void UpdateGoodScore(float value) {
		UpdateScoreText(goodComboText, "good combo:", value);
	}

	void UpdateBadScore(float value) {
		UpdateScoreText(badComboText, "bad combo:", value);
	}


}
