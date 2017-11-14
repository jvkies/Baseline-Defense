using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;

	private AudioSource musicSource;
	private AudioSource sfxSource;
	private MenuController menuScript;
	private Transform towerSpots;
	//private Spawner spawnblock;
	//private UnityEngine.Object[] MusicClips;			// Music disabled due to long load times

	//public int health = 10;
	public float souls = 61;
//	public float money = 25;
	public int wallPercent = 7;
	public bool isDragging = false;
	public bool isTowerSelected = false;
	public bool isGameLost = false;
	public string musicFolder = "music by neocrey";		// Music in Folder "Resources/music by neocrey"
	public GameObject draggedTower;
	public GameObject selectedTower;
	public AudioClip[] musicClipsPreload;
	public Dictionary<string, Tower> tower;
	public Dictionary<string, Mob> mobs;
	public GameObject wallPrefab;
	public GameObject blob;
	public GameObject fastBlob;
	public GameObject airBlob;
	public GameObject blobBoss;
	public GameObject fastBoss;
	public GameObject airBoss;
	public Sprite bullettower1Head;
	public Sprite bullettower2Head;
	public Sprite bullettower3Head;
	public Sprite bullettower4Head;
	public Sprite bullettower5Head;
	public Sprite rocktower1Head;
	public Sprite rocktower2Head;
	public Sprite rocktower3Head;
	public Sprite rocktower4Head;
	public Sprite rocktower5Head;


	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		musicSource = gameObject.GetComponents<AudioSource> ()[0];
		sfxSource = gameObject.GetComponents<AudioSource> ()[1];

		SceneManager.sceneLoaded += OnSceneLoaded; 		// using a delegate here, adding our own function OnSceneLoaded to get event calles from sceneLoaded
	}
		
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// if we start in the main menu, "MenuCanvas" and "Spawnblock" are not present
		try {
			menuScript = GameObject.FindWithTag ("MenuCanvas").GetComponent<MenuController>();
			towerSpots = GameObject.FindWithTag ("TowerSpotContainer").transform;
			//spawnblock = GameObject.FindWithTag ("Spawnblock").GetComponent<Spawner>();

			InitWalls ();

			//menuScript.UpdateHealth(health.ToString());
			menuScript.UpdateSouls(souls.ToString());
		//	menuScript.UpdateMoney(money.ToString());
		} catch {} 

	}


	void Start () {
		// InitMusic ();  								// this works, but music files are too large for github, skipping

		InitData ();
	}
		
	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (isDragging == true) {
				isDragging = false;
				Destroy (draggedTower);
			} else {
				menuScript.ToggleEscapeMenu();
			}
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			menuScript.ToggleEscapeMenu();
		}
		if (Input.GetMouseButtonDown (1) && isDragging) {
			isDragging = false;
			Destroy (draggedTower);
		}	
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			menuScript.BuildTower ("bullettower1");
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			menuScript.BuildTower ("rocktower1");
		}

			
	}

	private void InitData() {
		tower = new Dictionary<string, Tower>();
		tower.Add("bullettower1",new Tower("bullettower1","Bullet Tower 1","bullettower2",5,10,3,1,1,2,0,8,new Vector2(0.05f,0.05f),bullettower1Head));
		tower.Add("bullettower2",new Tower("bullettower2","Bullet Tower 2","bullettower3",15,15,5,2,2,3,0,8,new Vector2(0.05f,0.05f),bullettower2Head));
		tower.Add("bullettower3",new Tower("bullettower3","Bullet Tower 3","bullettower4",30,25,7,3,2.5f,4,0,8,new Vector2(0.05f,0.05f),bullettower3Head));
		tower.Add("bullettower4",new Tower("bullettower4","Bullet Tower 4","bullettower5",55,40,10,4,3,4.5f,0,8,new Vector2(0.05f,0.05f),bullettower4Head));
		tower.Add("bullettower5",new Tower("bullettower5","Bullet Tower 5",null,90,0,13,5,3.5f,5,0,8,new Vector2(0.05f,0.05f),bullettower5Head));
		tower.Add("rocktower1",new Tower("rocktower1","Rock Tower 1","rocktower2",10,25,10,1,0.4f,4,0.7f,4,new Vector2(0.15f,0.15f),rocktower1Head));
		tower.Add("rocktower2",new Tower("rocktower2","Rock Tower 2","rocktower3",35,50,15,2,0.5f,4.5f,0.8f,4,new Vector2(0.15f,0.15f),rocktower2Head));
		tower.Add("rocktower3",new Tower("rocktower3","Rock Tower 3","rocktower4",85,80,20,3,0.6f,5,0.8f,4,new Vector2(0.15f,0.15f),rocktower3Head));
		tower.Add("rocktower4",new Tower("rocktower4","Rock Tower 4","rocktower5",165,110,30,4,0.65f,5.5f,1,4,new Vector2(0.15f,0.15f),rocktower4Head));
		tower.Add("rocktower5",new Tower("rocktower5","Rock Tower 5",null,275,0,40,5,0.7f,6,1.2f,4,new Vector2(0.15f,0.15f),rocktower5Head));

		mobs = new Dictionary<string, Mob>();
		mobs.Add("blob",new Mob("blob","Blob",1,1,10,10,0,1,blob));
		mobs.Add("fastblob",new Mob("fastblob","Fast Blob",1,1.5f,8,8,0,1,fastBlob));
		mobs.Add("airblob",new Mob("airblob","Flying Blob",1,0.75f,10,10,0,1,airBlob));
		mobs.Add("blobboss",new Mob("blobboss","Blob Boss",20,0.75f,130,130,0,5,blobBoss));
		mobs.Add("fastboss",new Mob("fastboss","Fast Blob Boss",20,1.25f,140,140,0,5,fastBoss));
		mobs.Add("airboss",new Mob("airboss","Air Blob Boss",20,0.75f,140,140,0,5,airBoss));
	}

	public void InitWalls() {
		Debug.Log ("using tower coverage: " + wallPercent.ToString());
		for (int i = 0; i < towerSpots.childCount; i++) {
			if (Random.Range (0, 100) < wallPercent) {	// 
				GameObject wallInstance = Instantiate(wallPrefab,towerSpots.GetChild (i).position,Quaternion.identity);
				wallInstance.transform.SetParent(towerSpots.GetChild (i));
				AstarPath.active.UpdateGraphs (wallInstance.GetComponent<BoxCollider2D> ().bounds);

			}
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
		ResetPlayerdata ();
		LoadScene ("MainMenu");
	}		
		
	public void ResetPlayerdata() {
		// TODO: defined starting amount
		souls = 61;
		//health = 10;
		//money = 25;
	}

	public void UpdateSouls(float amount) {
		souls += amount;

		if (souls <= 0) {
			souls = 0;
			isGameLost = true;
			menuScript.DisplayLoose();
			menuScript.startWaveContainer.SetActive (false);
		}

		menuScript.UpdateSouls (souls.ToString());

	}

	// TODO: deprecated
//	public void UpdateMoney(float changeAmount=0) {
//		money += changeAmount;
//		menuScript.UpdateMoney (money.ToString());
//	}

	private void InitMusic() {
		try {
			// WORKS BUT TAKES TO LONG
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
