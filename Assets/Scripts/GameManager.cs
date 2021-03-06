﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using System.Reflection;
using Random = UnityEngine.Random;

//[assembly:AssemblyVersion ("1.0.*")]

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;
	public static bool isOnSceneLoadedCalled;			// prevent function OnSceneLoaded be called twice (http://www.wenyu-wu.com/blogs/unity-when-using-onlevelwasloaded-and-dontdestroyonload-together/)

	private AudioSource musicSource;
	private AudioSource sfxSource;
	private MenuController menuScript;
	private Transform towerSpots;
	//private Spawner spawnblock;
	//private UnityEngine.Object[] MusicClips;			// Music disabled due to long load times

	public int souls = 0;								// current souls the player has
	public int startSouls = 141;						// amount of souls the player starts with
	public float soulSpread = 2f;						// how large the area is when multiple souls are beeing spawned
	public int wallPercent = 7;
	public bool isDragging = false;
	public bool isTowerSelected = false;
	public bool isWallSelected = false;
	public bool isGameLost = false;
	public string musicFolder = "music by neocrey";		// Music in Folder "Resources/music by neocrey"
	[HideInInspector]
	public float startWaveTime;							// Time.time the first wave was called
	public int waveID = 0;
	public string version;
	public GameObject draggedTower;
	public GameObject selectedTower;
	public GameObject selectedWall;
	[HideInInspector]
	public AudioClip[] musicClipsPreload;
	public Dictionary<string, Tower> tower;
	public Dictionary<string, Mob> mobs;
	public Dictionary<string, int> highscore;			// "wave" -> 18, "time" -> 236
	public GameObject wallPrefab;
	public GameObject blob;
	public GameObject fastBlob;
	public GameObject airBlob;
	public GameObject armorBlob;
	public GameObject blobBoss;
	public GameObject fastBoss;
	public GameObject airBoss;
	public GameObject armorBoss;
	public Sprite bullettowerHeadWhite;
	public Sprite rocktowerHeadWhite;
	public GameObject soulPrefab;
	public Queue<GameObject> soulInstances;
	[HideInInspector]
	public GameObject homeCrystal;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		musicSource = gameObject.GetComponents<AudioSource> ()[0];
		sfxSource = gameObject.GetComponents<AudioSource> ()[1];

		SceneManager.sceneLoaded += OnSceneLoaded; 		// using a delegate here, adding our own function OnSceneLoaded to get event calles from sceneLoaded
		isOnSceneLoadedCalled = false;
	}
		
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (!isOnSceneLoadedCalled) {
			// if we start in the main menu, "MenuCanvas" and "Spawnblock" are not present
			try {
				menuScript = GameObject.FindWithTag ("MenuCanvas").GetComponent<MenuController> ();
				towerSpots = GameObject.FindWithTag ("TowerSpotContainer").transform;
				homeCrystal = GameObject.FindWithTag ("HomeCrystal");

				InitNewGame();

				isOnSceneLoadedCalled = true;
			} catch {
			} 
		}

	}


	void Start () {
		// InitMusic ();  								// this works, but music files are too large for github, skipping

		InitTowerAndMobData ();
		//Debug.Log(Assembly.GetExecutingAssembly ().GetName ().Version.ToString ());
			
	}
		
	void Update() {
		if (SceneManager.GetActiveScene ().name == "Game") {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (isDragging == true) {
					isDragging = false;
					Destroy (draggedTower);
				} else {
					menuScript.ToggleEscapeMenu ();
				}
			}
			if (Input.GetKeyDown (KeyCode.P)) {
				StopDragging ();
				menuScript.ToggleEscapeMenu ();
			}
			if (Input.GetMouseButtonDown (1) && isDragging) {
				StopDragging ();
			}	
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (isDragging) {
					StopDragging ();
				}
				menuScript.BuildTower ("bullettower1");
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (isDragging) {
					StopDragging ();
				}
				menuScript.BuildTower ("rocktower1");
			}
		}

	}

	private void InitNewGame() {
		InitWalls ();

		soulInstances = new Queue<GameObject> ();

		highscore = new Dictionary<string, int> ();
		highscore.Add ("wave", 0);
		highscore.Add ("time", 0);
		highscore.Add ("mobs", 0);

		waveID = 0;
		isGameLost = false;
		isDragging = false;
		souls = startSouls;
		isTowerSelected = false;

		menuScript.UpdateSouls (souls.ToString ());
		SpawnSouls (souls, homeCrystal.transform.position);
	}

	private void InitTowerAndMobData() {
		tower = new Dictionary<string, Tower>();
		tower.Add ("bullettower1", new Tower ("bullettower1", "Bullet Tower 1", "bullettower2",  10,  10,  3, 1,    1,    3, 0, 8, new Color32 (0, 0, 0, 255), new Vector2 (0.05f, 0.05f), bullettowerHeadWhite));
		tower.Add ("bullettower2", new Tower ("bullettower2", "Bullet Tower 2", "bullettower3",  20,  28,  6, 2, 1.5f,    3, 0, 8, new Color32 (50, 50, 220, 255), new Vector2 (0.05f, 0.05f), bullettowerHeadWhite));
		tower.Add ("bullettower3", new Tower ("bullettower3", "Bullet Tower 3", "bullettower4",  48,  58, 12, 3,   2f,    3, 0, 8, new Color32 (150, 150, 50, 255), new Vector2 (0.05f, 0.05f), bullettowerHeadWhite));
		tower.Add ("bullettower4", new Tower ("bullettower4", "Bullet Tower 4", "bullettower5", 106, 182, 24, 4, 2.5f,    3, 0, 8, new Color32 (255, 20, 20, 255), new Vector2 (0.05f, 0.05f), bullettowerHeadWhite));
		tower.Add ("bullettower5", new Tower ("bullettower5", "Bullet Tower 5", null, 			290,   0, 60, 5,   3f,    5, 0, 8, new Color32 (220, 220, 50, 255), new Vector2 (0.05f, 0.05f), bullettowerHeadWhite));
		tower.Add ("rocktower1", new Tower ("rocktower1", "Rock Tower 1", "rocktower2",  20,  16,   6, 1, 0.33f, 4,  0.5f, 4, new Color32 (0, 0, 0, 255), new Vector2 (0.15f, 0.15f), rocktowerHeadWhite));
		tower.Add ("rocktower2", new Tower ("rocktower2", "Rock Tower 2", "rocktower3",  36,  28,  12, 2, 0.33f, 4, 0.55f, 4, new Color32 (50, 50, 220, 255), new Vector2 (0.15f, 0.15f), rocktowerHeadWhite));
		tower.Add ("rocktower3", new Tower ("rocktower3", "Rock Tower 3", "rocktower4",  64,  51,  24, 3, 0.33f, 4,  0.6f, 4, new Color32 (150, 150, 50, 255), new Vector2 (0.15f, 0.15f), rocktowerHeadWhite));
		tower.Add ("rocktower4", new Tower ("rocktower4", "Rock Tower 4", "rocktower5", 116, 145,  48, 4, 0.33f, 4, 0.65f, 4, new Color32 (255, 20, 20, 255), new Vector2 (0.15f, 0.15f), rocktowerHeadWhite));
		tower.Add ("rocktower5", new Tower ("rocktower5", "Rock Tower 5", null, 		261,   0, 120, 5, 0.33f, 5,  0.8f, 4, new Color32 (220, 220, 50, 255), new Vector2 (0.15f, 0.15f), rocktowerHeadWhite));

		mobs = new Dictionary<string, Mob>();
		mobs.Add ("blob", 		new Mob ("blob", "Blob", 			 	 	0,  1,     1,  10,  10, 0, 0, 1, blob));
		mobs.Add ("groupblob", 	new Mob ("groupblob", "Group Blob", 	 	0,  1,     1,  10,  10, 0, 0, 1, blob));
		mobs.Add ("fastblob",	new Mob ("fastblob", "Fast Blob", 	 	 	0,  1,  1.5f,   8,   8, 0, 0, 1, fastBlob));
		mobs.Add ("armorblob", 	new Mob ("armorblob", "Armored Blob", 	 	0,  1, 0.7f,   10,   10, 1, 0, 1, armorBlob));
		mobs.Add ("blobboss", 	new Mob ("blobboss", "Blob Boss", 			0, 20, 0.75f, 130, 130, 0, 0, 10, blobBoss));
		mobs.Add ("fastboss", 	new Mob ("fastboss", "Fast Blob Boss",  	0, 20, 1.25f, 104, 104, 0, 0, 10, fastBoss));
		mobs.Add ("armorboss", 	new Mob ("armorboss", "Armored Blob Boss", 	0, 20, 0.6f, 130, 130, 3, 0, 10, armorBoss));
	}

	public void InitWalls() {
		//Debug.Log ("using tower coverage: " + wallPercent.ToString());
		for (int i = 0; i < towerSpots.childCount; i++) {
			if (Random.Range (0, 100) < wallPercent) {	// 
				GameObject wallInstance = Instantiate(wallPrefab,towerSpots.GetChild (i).position,Quaternion.identity);
				wallInstance.transform.SetParent(towerSpots.GetChild (i));
				AstarPath.active.UpdateGraphs (wallInstance.GetComponent<BoxCollider2D> ().bounds);

			}
		}

	}

	public void StopDragging() {
		if (isDragging) {
			isDragging = false;
			Destroy (draggedTower);
		}
	}

	public void LoadScene(string sceneName) {
		Time.timeScale = 1;
		SceneManager.LoadScene (sceneName);
	}

	public void GameOver() {
		// LoadScene ("GameOver");
	}

	public void MainMenuScene() {
		//ResetPlayerdata ();
		LoadScene ("MainMenu");
	}		
		
	public void ResetPlayerdata() {
		souls = 71;
	}

	public void UpdateSouls(int amount, GameObject position = null) {
		if (!isGameLost) {
			souls += amount;

			if (amount < 0) {
				// spending/leaking money
				if (souls <= 0) { // in case you have 5 souls but loose 10
					amount = -soulInstances.Count;
				}
				for (int i = 0; i > amount; i--) {
					//Destroy (soulInstances.Dequeue ());
					soulInstances.Dequeue().GetComponent<SoulController>().Despawn(position);
				}
			} else {
				// gaining money

				if (isTowerSelected && selectedTower.GetComponent<TowerController>().towerStats.upgradeCost < souls) {
					menuScript.upgradeButton.GetComponent<Button> ().interactable = true;
				}

				if (isWallSelected && selectedWall.GetComponent<Wall>().demolishCost < souls) {
					menuScript.demolishButton.GetComponent<Button> ().interactable = true;
				}

				if (position == null) {
					Debug.Log ("Error: position parameter of UpdateSouls can't be empty / null when increading soul amount");
				}

				SpawnSouls(amount, position.transform.position);
			}

			if (souls <= 0) {
				// Player lost the game

				GameManager.instance.highscore["time"] = (int)(Time.time - startWaveTime);

				souls = 0;
				isGameLost = true;
				menuScript.DisplayLoose ();
				menuScript.DisplayEndGamePanel ();
				menuScript.startWaveContainer.SetActive (false);
			}

			menuScript.UpdateSouls (souls.ToString ());
		}

	}

	public void SpawnSouls(int amount, Vector3 position) {
		Vector3 offsetPos = new Vector3(0,0,0);

		for (int i = 0; i < amount; i++) {
			if (amount != 1) {
				offsetPos = new Vector3 (Random.Range (-soulSpread,soulSpread), Random.Range (-soulSpread, soulSpread), 0);
			}

			GameObject soulGO = Instantiate (soulPrefab, position + offsetPos, Quaternion.identity);
			soulGO.transform.SetParent (homeCrystal.transform);
			//soulGO.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * 10;
			soulInstances.Enqueue (soulGO);
		}
	}
		
	private void InitMusic() {
		try {
			// WORKS BUT TAKES TO LONG TO LOAD
			//MusicClips = Resources.LoadAll(musicFolder, typeof(AudioClip)); 

			//if (MusicClips.Length == 0) {
			//	throw new ApplicationException("Warning: no asd music found in: "+musicFolder);
			//}

			StartCoroutine(changeMusic());
		}
		catch (Exception ex) { // catch all other exceptions
			Debug.Log("Warning: no music found in: "+musicFolder);
			Debug.Log(ex);
		}
	}
		
	// changeMusic() will wait until the current song ends and randomly play a new song
	IEnumerator changeMusic()
	{
		yield return new WaitForSeconds(musicSource.clip.length);

		musicSource.Stop ();
		musicSource.clip = (AudioClip) musicClipsPreload [Random.Range (0, musicClipsPreload.Length)];
		musicSource.Play ();

		StartCoroutine(changeMusic());
	}

	public void PlaySfx (AudioClip clip) {
		sfxSource.clip = clip;
		sfxSource.Play ();
	}

}
