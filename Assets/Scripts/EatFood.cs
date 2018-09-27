using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatFood : MonoBehaviour {

	//public BarLengthManager goodBar;
	//public BarLengthManager badBar;
	public BarLengthManager2 angerBar;

	public Vector2 goodFoodChange = new Vector2(10, 0);
	public Vector2 badFoodChange = new Vector2(0, 10);
	
	public float badFoodChangePercent = 1f / 20f;
	public float goodFoodChangePercent = -1f / 40f;

	enum foodType { healthy, sinful, superFood, rottenFood, none };

	float healthyCombo = 0;
	int sinfulCombo = 0;
	float score = 0;
	int multiplier = 1;

	int highScore = 0;
	int ComboHighScore = 0;

	int bestCombo = 0;

	int [] thresholds = new int [] { 5, 10, 15, 25, 40, 65, 105, 170, 275};

	public FoodSpawner foodSpawner;
	public MumFaceManager mumFaceManager;
	public Text badComboText;
	public Text bestComboText;
	public Text goodComboText;
	public Text multiplierFlashText;

	public Text scoreText;
	public Text gameOverText;
	public Text multiplierText;

	public Text highScoreText;

	public Text comboHighScoreText;

	public GameObject darkenBackground;

	public FlashNSeconds multiplierFlash;

	public bool gameOver = true;

	float elapsedTimeSinceLastCandy = 0;

	public Animator babyAnimator;

	void Start () {
		highScore = PlayerPrefs.GetInt("HighScore", 0);
		ComboHighScore = PlayerPrefs.GetInt("ComboHighScore", 0);

		UpdateHighScore(highScore);
		UpdateComboHighScore(ComboHighScore);

		UpdateBadScore(sinfulCombo);
		UpdateGoodScore(healthyCombo);
		UpdateMultiplierText(multiplier);
	}
	
	foodType lastFoodType = foodType.none;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "GoodFood") {
			GoodFoodUpdate();
		} else if (other.gameObject.tag == "BadFood") {
			BadFoodUpdate();
		} else if (other.gameObject.tag == "RottenFood") {
			GameOver();
		} else if (other.gameObject.tag == "SuperFood") {
			UpdateBarLength(-1f);
		}
		Destroy(other.gameObject);
	}

	void GoodFoodUpdate() {
		if (lastFoodType == foodType.healthy) {
			++healthyCombo;
			UpdateGoodScore(healthyCombo);
		}

		if (lastFoodType != foodType.healthy) {
			if (sinfulCombo > bestCombo) {
				bestCombo = sinfulCombo;
				UpdateBestCombo(bestCombo);
			}
			
			sinfulCombo = 0;
			UpdateBadScore(sinfulCombo);
			multiplier = 1;
			UpdateMultiplier();
			multiplierFlash.ShowAndHide();
			multiplierFlashText.text = "multiplier: " + multiplier + "X" ;
		}

		lastFoodType = foodType.healthy;
		UpdateBarLength(goodFoodChangePercent);
	}

	void BadFoodUpdate() {
		elapsedTimeSinceLastCandy = 0;
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
		if (angerBar.GetPercent() > 1) {
			GameOver();
		}
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
			multiplierFlash.ShowAndHide();
			multiplierFlashText.text = "multiplier: " + multiplier+"X";
			foodSpawner.fallingSpeed *= 1.1f;
		}
	}

	void UpdateBarLength(float deltaPercent) {
		//goodBar.ChangeLength(delta.y);
		//badBar.ChangeLength(delta.x);
		angerBar.ChangeLengthPercent(deltaPercent);
		//Debug.Log("bar " + angerBar.GetPercent());
		//Debug.Log("anger percent "+angerBar.GetPercent());
	}

	void UpdateScoreText(Text textObj, string label, float value) {
		textObj.text = label + value;
	}

	void UpdateGoodScore(float value) {
		UpdateScoreText(goodComboText, "good combo:", value);
	}

	void UpdateBadScore(float value) {
		UpdateScoreText(badComboText, "Combo:  ", value);
	}

	void UpdateScore(int value) {
		UpdateScoreText(scoreText, "Score: ", value);
	}

	void UpdateBestCombo(int value) {
		UpdateScoreText(bestComboText, "Best Combo: ", value);
	}

	void UpdateComboHighScore(int value) {
		UpdateScoreText(comboHighScoreText, "Combo HighScore: ", value);
	}

	void UpdateHighScore(float value) {
		UpdateScoreText(highScoreText, "HighScore: ", value);
	}

	void GameOver() {
		gameOverText.gameObject.SetActive(true);
		//Time.timeScale = 0;
		Animator animator = gameOverText.gameObject.GetComponent<Animator>();
		animator.enabled = true;
		gameOver = true;
		darkenBackground.SetActive(true);
		foodSpawner.StopAllCoroutines();
		UpdateMumIcon();
		if (score > highScore) highScore = (int) score;
		if (bestCombo > ComboHighScore) ComboHighScore = bestCombo;
		if (sinfulCombo > ComboHighScore) ComboHighScore = sinfulCombo;
		UpdateComboHighScore((int)Mathf.FloorToInt(ComboHighScore));
		UpdateHighScore((int)Mathf.FloorToInt(highScore));
		PlayerPrefs.SetInt("HighScore", highScore);
		PlayerPrefs.SetInt("ComboHighScore", ComboHighScore);
		babyAnimator.enabled = false;
	}

	public void Reset() {
		darkenBackground.SetActive(false);
		foodSpawner.Reset();
		score = 0;
		multiplier = 1;
		sinfulCombo = 0;
		bestCombo = 0;
		UpdateBadScore(sinfulCombo);
		UpdateGoodScore(healthyCombo);
		UpdateBestCombo(bestCombo);

		UpdateMultiplierText(multiplier);
		
		UpdateScore((int) score);
		UpdateMumIcon();
		angerBar.Reset();
		gameOverText.gameObject.SetActive(false);
		Animator animator = gameOverText.gameObject.GetComponent<Animator>();
		animator.enabled = false;

		babyAnimator.enabled = true;
		gameOver = false;
	}

	void UpdateMultiplierText(int value) {
		UpdateScoreText(multiplierText, "Multiplier:", value);
		multiplierText.text += "X";
	}

	// Update is called once per frame
	void Update () {
		if (gameOver) return;

		elapsedTimeSinceLastCandy += Time.deltaTime;

		if (elapsedTimeSinceLastCandy > 5f) {
			if (multiplier > 1) {
				sinfulCombo = 0;
				multiplier = 1;	
				UpdateMultiplier();
				multiplierFlash.ShowAndHide();
				multiplierFlashText.text = "multiplier: " + multiplier+"X";
				elapsedTimeSinceLastCandy = 0;
			}
		}
		

		score += (Time.deltaTime * 10 * multiplier);
		//score /= (float) 10;
		UpdateScore((int) score);
		UpdateMumIcon();
	}

	void UpdateMumIcon() {
		if (angerBar.GetPercent() > 1f)
			mumFaceManager.SetAngerState(4);
		else if (angerBar.GetPercent() > 0.8f)
			mumFaceManager.SetAngerState(3);
		else if (angerBar.GetPercent() > 0.6f)
			mumFaceManager.SetAngerState(2);
		else if (angerBar.GetPercent() > 0.35f)
			mumFaceManager.SetAngerState(1);
		else 
			mumFaceManager.SetAngerState(0);
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		PlayerPrefs.SetInt("HighScore", highScore);
		PlayerPrefs.SetInt("ComboHighScore", ComboHighScore);
	}

	public void ResetHighScores() {
		highScore = 0;
		ComboHighScore = 0;
		UpdateHighScore(highScore);
		UpdateComboHighScore(ComboHighScore);
		PlayerPrefs.SetInt("HighScore", highScore);
		PlayerPrefs.SetInt("ComboHighScore", ComboHighScore);
	}

}
