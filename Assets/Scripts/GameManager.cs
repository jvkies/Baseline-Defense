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
	private Spawner spawnblock;
	//private UnityEngine.Object[] MusicClips;

	public int health = 10;
	public float money = 250;
	public bool isDragging = false;
	public bool isTowerSelected = false;
	public bool isGameLost = false;
	public string musicFolder = "music by neocrey";		// Music in Folder "Resources/music by neocrey"
	public GameObject draggedTower;
	public GameObject selectedTower;
	public AudioClip[] musicClipsPreload;
	public Dictionary<string, Tower> tower;
	public Dictionary<string, Mob> mobs;
	public GameObject blob1;
	public GameObject blob2;
	public GameObject blobBoss1;
	public GameObject blobBoss2;
	public Sprite bullettower1Head;
	public Sprite bullettower2Head;
	public Sprite bullettower3Head;


	void Awake () {
		if (instance == null) {
			instance = this;

			//playerData = new PlayerData ();
		}
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		musicSource = gameObject.GetComponents<AudioSource> ()[0];
		sfxSource = gameObject.GetComponents<AudioSource> ()[1];

		SceneManager.sceneLoaded += OnSceneLoaded; 			// using a delegate here, adding our own function OnSceneLoaded to get event calles from sceneLoaded

		tower = new Dictionary<string, Tower>();
		tower.Add("bullettower1",new Tower("bullettower1","Bullet Tower 1","bullettower2",5,10,3,1,1,2,bullettower1Head));
		tower.Add("bullettower2",new Tower("bullettower2","Bullet Tower 2","bullettower3",15,15,5,2,2,3,bullettower2Head));
		tower.Add("bullettower3",new Tower("bullettower3","Bullet Tower 3",null,30,0,7,3,3,4,bullettower3Head));

		mobs = new Dictionary<string, Mob>();
		mobs.Add("blob1",new Mob("blob1","Yellow Blob",1,100,10,10,0,1,blob1));
		mobs.Add("blob2",new Mob("blob2","Red Blob",2,100,30,30,0,2,blob2));
		mobs.Add("blobboss1",new Mob("blobboss1","Yellow Blob Boss",20,80,150,150,0,5,blobBoss1));
		mobs.Add("blobboss2",new Mob("blobboss1","Red Blob Boss",40,80,600,600,0,9,blobBoss2));

	}
		
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// if we start in the main menu, "MenuCanvas" is not present
		try {
			menuScript = GameObject.FindWithTag ("MenuCanvas").GetComponent<MenuController>();
			spawnblock = GameObject.FindWithTag ("Spawnblock").GetComponent<Spawner>();

			menuScript.UpdateHealth(health.ToString());
			menuScript.UpdateMoney(money.ToString());
		} catch {}  
	}


	void Start () {
		// this works, but music files are too large for github
		// InitMusic ();  

	}
		
	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (isDragging == true) {
				isDragging = false;
				Destroy (draggedTower);
			} else {
				//escapeMenu. SetActive (true);
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


	}

	public void LoadScene(string sceneName) {
		Time.timeScale = 1;
		SceneManager.LoadScene (sceneName);
	}

	public void GameOver() {
		LoadScene ("GameOver");
	}

	public void MainMenuScene() {
		//playerData = new PlayerData ();
		LoadScene ("MainMenu");
	}		
		
	public void DecreaseLife(int amount) {
		health -= amount;
		menuScript.UpdateHealth (health.ToString());

		if (health <= 0) {
			isGameLost = true;
			menuScript.DisplayLoose();
			spawnblock.startWaveContainer.SetActive (false);

			// TODO: game over
		}

	}

	public void UpdateMoney(float changeAmount=0) {
		if (changeAmount != 0) {
			money += changeAmount;
		}
		menuScript.UpdateMoney (money.ToString());
	}

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
