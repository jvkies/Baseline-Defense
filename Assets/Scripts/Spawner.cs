using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {

//	private int waveCount;
	private float waitBetweenMobs = 0.3f;
	private float deltaSum = 0;
	//private string[] waveMobs = { "blob", "blob", "fastblob", "airblob", "blobboss", "blob", "blob", "fastblob", "airblob", "fastboss", "blob", "blob", "fastblob", "airblob", "airboss"};
	//private int[] waveMobsAmount = { 20,30,30,30,1,30,30,30,30,1,30,30,30,30,1 };
	private Vector3 spawnPos;
	private GameObject enemyContainer;
	private MenuController menuController;
	private Image buttonEffectImage;

	public Text interestGain;
	public int baseSoulInterest = 12;					// base amount of souls added each wave (interest), total amount is baseAmoun+waveid-1
	public int bonusInterestTime = 24;					// time the player has to call the wave early, calling a wave early in this seconds gives bonus interest
	public int maxWaveAmount = 50;						// maximum number of waves until you win
	public int maxSimulWaves = 5;						// maximum number of waves which can be called early
	public float healthScaleFactor = 0.3f;				// scale factor the health of the mobs increase per wave
	public Color32 highlightColor = new Color32 (255, 255, 0, 255);
	public Color32 standardColor = new Color32 (216, 18, 15, 255);
	public GameObject buttonEffect;
	public GameObject startWaveContainer;
	public AnimationCurve colorCurve;
	// TODO: store all data about a wave in one smart data structure
	public Dictionary<int, List<GameObject>> waveMob;	// waveID -> mobGameObject[], maps the waveID to all mob Instances in this wave
	public Dictionary<int, float> waveTime;				// waveID -> Time.time when this wave was started
	public Dictionary<int, float> waveSurvivedHealth;	// waveID -> total Health of mobs who survived

	// Use this for initialization
	void Start () {
		enemyContainer = GameObject.FindWithTag("EnemyContainer");
		menuController = GameObject.FindWithTag ("MenuCanvas").GetComponent<MenuController> ();
		buttonEffectImage = buttonEffect.GetComponent<Image> ();

		waveMob = new Dictionary<int, List<GameObject>>();
		waveTime = new Dictionary<int, float>();
		waveSurvivedHealth = new Dictionary<int, float>();
	}

	// Update is called once per frame
	void Update () {
		buttonEffectImage.color = Color.Lerp (standardColor, highlightColor, colorCurve.Evaluate (deltaSum));
		deltaSum += Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.Space) && startWaveContainer.activeSelf && waveMob.Count <= maxSimulWaves) {
			// TODO: only if the next wave should be able to be called..
			NextWave ();
		}
	}

	public Transform GetRandomSpawnSpot() {
		return gameObject.transform.GetChild (Random.Range (0, gameObject.transform.childCount));
	}

	public string NextMob(int _waveID) {
		if (_waveID % 10 == 4)
			return "groupblob";
		else if (_waveID % 10 == 6)
			return "fastblob";
		else if (_waveID % 10 == 8)
			return "armorblob";
		else if (_waveID % 10 == 0) {
			if (_waveID % 30 == 20)
				return "fastboss";
			else if (_waveID % 30 == 0)
				return "armorboss";
			else
				return "blobboss";
		}
		
		return "blob";
	}

	public int MobsPerWave(int _waveID, string _mobID) {
		if (_mobID == "blobboss" || _mobID == "armorboss" || _mobID == "fastboss")
			return 1;
		else
			return 15 + _waveID;
	}

	public IEnumerator StartWave(int waveID) {
		string mobID = NextMob (waveID);
		int mobPerWave = MobsPerWave(waveID, mobID); 
			
		menuController.waveDisplayer.GetComponent<Text> ().text = "Wave " + (waveID).ToString ();
		menuController.waveDisplayer.SetActive (true);
		yield return new WaitForSeconds(3);
		menuController.waveDisplayer.SetActive(false);

		if (mobID == "groupblob") {
			waitBetweenMobs = 0.07f;
			spawnPos = GetRandomSpawnSpot ().position;
		} else {
			waitBetweenMobs = 0.3f;
		}

		while (mobPerWave > 0) {
			yield return new WaitForSeconds(waitBetweenMobs);
			SpawnMob (mobID, waveID);
			mobPerWave -= 1;
		}

		while (waveMob.ContainsKey(waveID) && waveMob[waveID].Count != 0) {
			yield return new WaitForSeconds(1);
		}
			
		if (GameManager.instance.isGameLost == false) {

			// Give interest money to player after survival
			payInterest (GetInterest(waveID));
		
			if (waveID == maxWaveAmount) {
				// Player wins the game

				startWaveContainer.SetActive (false);

				GameManager.instance.highscore["time"] = (int)(Time.time - GameManager.instance.startWaveTime);

				menuController.DisplayWin ();
				menuController.DisplayEndGamePanel ();
	
			}
		}

	}

	public Mob ScaleMob(Mob _mob, int waveID) {
		_mob.health += _mob.health * healthScaleFactor * (waveID-1);
		_mob.maxHealth += _mob.maxHealth * healthScaleFactor * (waveID-1);
	//	_mob.moneyWorth += Mathf.FloorToInt( _mob.moneyWorth * 0.1f * waveID );
		_mob.mobHeartDamage += Mathf.FloorToInt( _mob.mobHeartDamage * 0.1f * waveID );

		if (waveID > 10) {
			if (_mob.armor != 0)
				_mob.armor += 1;
			_mob.health += _mob.health * 0.5f;
			_mob.maxHealth += _mob.maxHealth * 0.5f;
		}
		if (waveID > 20) {
			if (_mob.armor != 0)
				_mob.armor += 1;
			_mob.health += _mob.health * 0.6f;
			_mob.maxHealth += _mob.maxHealth * 0.6f;
		}
		if (waveID > 30) {
			if (_mob.armor != 0)
				_mob.armor += 1;
			_mob.health += _mob.health * 0.6f;
			_mob.maxHealth += _mob.maxHealth * 0.6f;
		}
		if (waveID > 40) {
			if (_mob.armor != 0)
				_mob.armor += 1;
			_mob.health += _mob.health * 0.4f;
			_mob.maxHealth += _mob.maxHealth * 0.4f;
		}
		if (waveID > 50) {
			if (_mob.armor != 0)
				_mob.armor += 1;
			_mob.health += _mob.health * 0.3f;
			_mob.maxHealth += _mob.maxHealth * 0.3f;
		}

		return _mob;
	}

	public void SpawnMob(string mobID, int waveID) {
				
		if (mobID != "groupblob") {
			spawnPos = GetRandomSpawnSpot ().position;
		}
		else
			mobID = "blob";

		//spawnPos = GetRandomSpawnSpot ().position;

		GameObject mob = Instantiate (GameManager.instance.mobs[mobID].mobPrefab, spawnPos, Quaternion.identity);
		waveMob [waveID].Add(mob); 

		mob.GetComponent<MobController> ().mobData = ScaleMob(GameManager.instance.mobs [mobID].Copy(),waveID);
		mob.GetComponent<MobController> ().mobData.waveID = waveID;

		if (waveID > 10)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (200, 50, 50,255);
		if (waveID > 20)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (82, 138, 255,255);
		if (waveID > 30)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (50, 200, 50,255);
		if (waveID > 40)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (0, 200, 200,255);
		if (waveID > 50)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (200, 50, 200,255);
		
		if (enemyContainer != null) {
			mob.transform.SetParent (enemyContainer.transform);
		}
			
	}

	private int GetInterest ( int waveID ) {
		int interest = baseSoulInterest + waveID - 1;
		return  Mathf.RoundToInt ((interest + CallWaveEarlyBonusMoney (waveID, interest)) * DamageDoneMultiplier(waveID));
	}

	private int CallWaveEarlyBonusMoney(int waveID, int interest) {
		if (waveTime.ContainsKey (waveID + 1)) {
			float percentBonus = 1 - ((waveTime [waveID+1] - waveTime [waveID]) / bonusInterestTime);
			//Debug.Log ("called early after: "+(waveTime [waveID+1] - waveTime [waveID])+" seconds");
			//Debug.Log ("percent bonus: " + percentBonus);
			//Debug.Log("bonus interest: "+Mathf.RoundToInt( interest * percentBonus)+" (of interest: "+interest+")");
			if (percentBonus <= 0) {
				return 0;
			} else {
				return Mathf.RoundToInt (interest * percentBonus);
			}
		} else {
			return 0;
		}
	}

	private float DamageDoneMultiplier( int waveID) {
		// returns a percent multiplier (e.g. 0.76) of how much damage was dealt in the last round
		return (TotalWaveHealth(waveID) - waveSurvivedHealth[waveID]) / TotalWaveHealth(waveID);
	}

	private float TotalWaveHealth( int waveID) {
		string mobID = NextMob (waveID);
		int mobPerWave = MobsPerWave(waveID, mobID); 
		Mob mobData = ScaleMob (GameManager.instance.mobs [mobID].Copy (), waveID);

		return mobPerWave * mobData.maxHealth;
	}

	public void NextWave() {
		if (waveMob.Count <= maxSimulWaves) {
			GameManager.instance.waveID += 1;
			int _waveID = GameManager.instance.waveID;

			if (_waveID == 1) {
				GameManager.instance.startWaveTime = Time.time;
			} 

			waveMob.Add (_waveID, new List<GameObject> ());
			waveTime.Add (_waveID, Time.time);
			waveSurvivedHealth.Add (_waveID, 0);

			menuController.SetWaveButtonText ((_waveID + 1).ToString ());

			//startWaveContainer.SetActive (false);
			StartCoroutine (StartWave (_waveID));
		}

	}

	private void payInterest(int interest)
	{
		if (GameManager.instance.waveID != maxWaveAmount) 
		{
			GameManager.instance.UpdateSouls (interest, GameManager.instance.yellowCrystal);
			Debug.Log("payed interest: " + interest);
			interestGain.text = "+ " + interest;
			interestGain.color = new Color(interestGain.color.r, interestGain.color.g, interestGain.color.b, 1);

			//StartCoroutine(FadeTextToFullAlpha(1f, interestGain));
			StartCoroutine(hideInterestDelayed());
		}
	}

	private IEnumerator hideInterestDelayed()
	{
		yield return new WaitForSecondsRealtime(2);
		StartCoroutine(FadeTextToZeroAlpha(1f, interestGain));
	}

	public IEnumerator FadeTextToFullAlpha(float t, Text i)
	{
		i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
		while (i.color.a < 1.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
			yield return null;
		}
	}

	public IEnumerator FadeTextToZeroAlpha(float t, Text i)
	{
		i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
		while (i.color.a > 0.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
			yield return null;
		}
	}
}
