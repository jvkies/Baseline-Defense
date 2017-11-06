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
	private SideMenuController sideMenuScript;
	//private UnityEngine.Object[] MusicClips;

	public int health = 10;
	public float money = 25;
	public bool isDragging = false;
	public bool isTowerSelected = false;
	public string musicFolder = "music by neocrey";		// Music in Folder "Resources/music by neocrey"
	public GameObject draggedTower;
	public GameObject selectedTower;
	public AudioClip[] musicClipsPreload;
	public Dictionary<string, Tower> tower;

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

		SceneManager.sceneLoaded += OnSceneLoaded; 			// using a delegate here, adding our own function OnSceneLoaded, to get event calles from sceneLoaded

		tower = new Dictionary<string, Tower>();
		tower.Add("bullettower1",new Tower("bullettower1","Bullet Tower",5,3,1,1,2));
		tower.Add("bullettower2",new Tower("bullettower2","Bullet Tower",10,5,2,2,3));
		tower.Add("bullettower3",new Tower("bullettower3","Bullet Tower",15,7,3,3,4));

	}
		
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// if we start in the main menu, "SideMenuCanvas" is not present
		try {
			sideMenuScript = GameObject.FindWithTag ("SideMenuCanvas").GetComponent<SideMenuController>();
		} catch {}  
	}


	void Start () {
		// this works, but music files are too large for github
		// InitMusic ();  

	}
		
	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape) && isDragging == true) {
			isDragging = false;
			Destroy (draggedTower);
		}
	}

	public void LoadScene(string sceneName) {
		Time.timeScale = 1;
		SceneManager.LoadScene (sceneName);
	}

	public void GameOver() {
		LoadScene ("GameOver");
	}

	public void RestartGame() {
		//playerData = new PlayerData ();
		LoadScene ("MainMenu");
	}		

	public void DecreaseLife(int amount) {
		health -= amount;
		sideMenuScript.UpdateHealth (health.ToString());

		if (health <= 0) {
			//Debug.Log ("game over");
			// TODO: game over
		}

	}

	public void UpdateMoney() {
		sideMenuScript.UpdateMoney (money.ToString());
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
