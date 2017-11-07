using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	private int waveCount;
	private float deltaSum = 0;
	private GameObject enemyContainer;
	private GameObject menuCanvas;
	private Color32 highlightColor = new Color32 (255, 255, 0, 255);
	private Color32 standardColor = new Color32 (216, 18, 15, 255);

	public Dictionary<int, Wave> waves;
	public GameObject spawner;
	public GameObject buttonEffect;
	public GameObject startWaveContainer;
	public AnimationCurve colorCurve;

	// Use this for initialization
	void Start () {
		enemyContainer = GameObject.FindWithTag("EnemyContainer");
		menuCanvas = GameObject.FindWithTag ("MenuCanvas");
		waveCount = 0;

		waves = new Dictionary<int, Wave>();
		waves.Add(1,new Wave(1,"blob1",10));
		waves.Add(2,new Wave(2,"blob1",20));
		//waves.Add(3,new Wave(3,"blob1",30));
		waves.Add(3,new Wave(3,"blobboss1",1));
		waves.Add(4,new Wave(4,"blob2",10));
		waves.Add(5,new Wave(5,"blob2",20));
		//waves.Add(7,new Wave(7,"blob2",30));
		waves.Add(6,new Wave(6,"blobboss2",1));

	}

	// Update is called once per frame
	void Update () {
		buttonEffect.GetComponent<Image> ().color = Color.Lerp (standardColor, highlightColor, colorCurve.Evaluate (deltaSum));
		deltaSum += Time.deltaTime;
	}

	public IEnumerator StartWave(int waveID) {
		menuCanvas.GetComponent<MenuController> ().waveDisplayer.GetComponent<Text> ().text = "Wave " + waveID.ToString ();
		menuCanvas.GetComponent<MenuController> ().waveDisplayer.SetActive (true);
		yield return new WaitForSeconds(3);
		menuCanvas.GetComponent<MenuController>().waveDisplayer.SetActive(false);

		while (waves [waveID].mobAmount > 0) {
			yield return new WaitForSeconds(1);
			SpawnMob (waves [waveID].mobID);
			waves [waveID].mobAmount -= 1;
		}
		if (waves.ContainsKey (waveID + 1)) {
			if (GameManager.instance.isGameLost == false) {
				startWaveContainer.SetActive (true);
			}
		} else {
			// we are in the final wave
			while (enemyContainer.transform.childCount != 0) {
				yield return new WaitForSeconds(1);

			}

			if (GameManager.instance.isGameLost != true) {
				menuCanvas.GetComponent<MenuController> ().DisplayWin ();
			}

		}
	}

	public void SpawnMob(string mobType) {

		GameObject mob = Instantiate (GameManager.instance.mobs[mobType].mobPrefab, spawner.transform.position, Quaternion.identity);

		// we are using a copy of the stored instance of Mob
		mob.GetComponent<MobController> ().mobData = GameManager.instance.mobs [mobType].Copy();

		if (enemyContainer != null) {
			mob.transform.SetParent (enemyContainer.transform);
		}

		//yield return new WaitForSeconds(1);


	}

	public void NextWave() {
		waveCount += 1;

		startWaveContainer.SetActive (false);
		StartCoroutine(StartWave(waveCount));
		//InvokeRepeating ("SpawnMob", 1,1);

	}
}
