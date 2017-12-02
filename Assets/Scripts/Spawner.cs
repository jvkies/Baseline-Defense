using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {

	private int waveCount;
	private float waitBetweenMobs = 0.3f;
	private float deltaSum = 0;
	//private string[] waveMobs = { "blob", "blob", "fastblob", "airblob", "blobboss", "blob", "blob", "fastblob", "airblob", "fastboss", "blob", "blob", "fastblob", "airblob", "airboss"};
	//private int[] waveMobsAmount = { 20,30,30,30,1,30,30,30,30,1,30,30,30,30,1 };
	private Vector3 spawnPos;
	private GameObject enemyContainer;
	private MenuController menuCanvas;
	private Image buttonEffectImage;

	public int maxWaveAmount = 100;				// maximum numbers of waves
	public float healthScaleFactor = 0.3f;		// scale factor the health of the mobs increase per wave
	public Color32 highlightColor = new Color32 (255, 255, 0, 255);
	public Color32 standardColor = new Color32 (216, 18, 15, 255);
	public Dictionary<int, Wave> waves;
	public GameObject buttonEffect;
	public GameObject startWaveContainer;
	public AnimationCurve colorCurve;

	// Use this for initialization
	void Start () {
		enemyContainer = GameObject.FindWithTag("EnemyContainer");
		menuCanvas = GameObject.FindWithTag ("MenuCanvas").GetComponent<MenuController> ();
		buttonEffectImage = buttonEffect.GetComponent<Image> ();
		waveCount = 0;

		waves = new Dictionary<int, Wave>();

	//	for (int waveCount = 0; waveCount < waveMobs.Length; waveCount++) {
	//		waves.Add(waveCount,new Wave(waveCount,waveMobs[waveCount],waveMobsAmount[waveCount]));
	//	}
			
	//	waves.Add(1,new Wave(1,"blob1",20));
	//	waves.Add(2,new Wave(2,"blob1",20));
	//	waves.Add(3,new Wave(3,"blob1",20));
	//	waves.Add(4,new Wave(4,"blob1",20));
	//	waves.Add(5,new Wave(5,"blobboss1",1));

	//	waves.Add(6,new Wave(6,"blob2",20));
	//	waves.Add(7,new Wave(7,"blob2",20));
	//	waves.Add(8,new Wave(8,"blob2",30));
	//	waves.Add(9,new Wave(9,"blobboss2",1));

	}

	// Update is called once per frame
	void Update () {
		buttonEffectImage.color = Color.Lerp (standardColor, highlightColor, colorCurve.Evaluate (deltaSum));
		deltaSum += Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.Space) && startWaveContainer.activeSelf) {
			// TODO: only if the next wave should be able to be called..
			NextWave ();
		}


	}

	public Transform GetRandomSpawnSpot() {
		return gameObject.transform.GetChild (Random.Range (0, gameObject.transform.childCount));
	}

	public string NextMob(int _waveID) {
		if (_waveID % 10 == 2 || _waveID % 10 == 8)
			return "groupblob";
		else if (_waveID % 10 == 4)
			return "fastblob";
		else if (_waveID % 10 == 6)
			return "airblob";
		else if (_waveID % 10 == 0) {
			if (_waveID % 30 == 20)
				return "fastboss";
			else if (_waveID % 30 == 0)
				return "airboss";
			else
				return "blobboss";
		}
		
		return "blob";
	}

	public int MobsPerWave(int _waveID, string _mobID) {
		if (_mobID == "blobboss" || _mobID == "fastboss" || _mobID == "airboss")
			return 1;
		else
			return 15 + _waveID;
	}

	public IEnumerator StartWave(int waveID) {
		string mobID = NextMob (waveID);
		int mobPerWave = MobsPerWave(waveID, mobID); 
			
		menuCanvas.waveDisplayer.GetComponent<Text> ().text = "Wave " + (waveID).ToString ();
		menuCanvas.waveDisplayer.SetActive (true);
		yield return new WaitForSeconds(3);
		menuCanvas.waveDisplayer.SetActive(false);

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

		while (enemyContainer.transform.childCount != 0) {
			yield return new WaitForSeconds(1);
		}
			
		if (GameManager.instance.isGameLost == false) {

			// Give money to player after survival
			if (waveCount != 1)
				GameManager.instance.UpdateSouls (Mathf.Ceil(10+waveCount-1));
			
			if (waveID != maxWaveAmount) {
				startWaveContainer.SetActive (true);
			} else {
				if (GameManager.instance.isGameLost != true) {
					menuCanvas.DisplayWin ();
				}
			}
		}

	}

	public Mob ScaleMob(Mob _mob, int waveID) {
		_mob.health += _mob.health * healthScaleFactor * (waveID-1);
		_mob.maxHealth += _mob.maxHealth * healthScaleFactor * (waveID-1);
	//	_mob.moneyWorth += Mathf.FloorToInt( _mob.moneyWorth * 0.1f * waveID );
		_mob.mobHeartDamage += Mathf.FloorToInt( _mob.mobHeartDamage * 0.1f * waveID );

		if (waveID > 10) {
			_mob.health += _mob.health * 0.5f;
			_mob.maxHealth += _mob.maxHealth * 0.5f;
		}
		if (waveID > 20) {
			_mob.health += _mob.health * 0.6f;
			_mob.maxHealth += _mob.maxHealth * 0.6f;
		}
		if (waveID > 30) {
			_mob.health += _mob.health * 0.7f;
			_mob.maxHealth += _mob.maxHealth * 0.7f;
		}
		if (waveID > 40) {
			_mob.health += _mob.health * 0.8f;
			_mob.maxHealth += _mob.maxHealth * 0.8f;
		}
		if (waveID > 50) {
			_mob.health += _mob.health * 0.8f;
			_mob.maxHealth += _mob.maxHealth * 0.8f;
		}

		return _mob;
	}

	public void SpawnMob(string mobID, int waveID) {
				
		if (mobID != "groupblob") {
			// TODO: to active groupblob: uncomment the next line and deleete the duplicate below
			spawnPos = GetRandomSpawnSpot ().position;
		}
		else
			mobID = "blob";

		//spawnPos = GetRandomSpawnSpot ().position;

		GameObject mob = Instantiate (GameManager.instance.mobs[mobID].mobPrefab, spawnPos, Quaternion.identity);

		mob.GetComponent<MobController> ().mobData = ScaleMob(GameManager.instance.mobs [mobID].Copy(),waveID);
		if (waveID > 10)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (255, 50, 50,255);
		if (waveID > 20)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (82, 138, 255,255);
		if (waveID > 30)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (50, 200, 50,255);
		if (waveID > 30)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (0, 200, 200,255);
		if (waveID > 40)
			mob.GetComponent<SpriteRenderer> ().color = new Color32 (200, 50, 200,255);
		
		if (enemyContainer != null) {
			mob.transform.SetParent (enemyContainer.transform);
		}
			
	}

	public void NextWave() {
		waveCount += 1;

		startWaveContainer.SetActive (false);
		StartCoroutine(StartWave(waveCount));

	}
}
