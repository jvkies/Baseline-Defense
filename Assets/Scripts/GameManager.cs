using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;

	public int health = 20;
	public int money = 50;
	public bool isDragging = false;
	public string musicFolder = "music by neocrey";		// Music in Folder "Resources/music by neocrey"
	public GameObject draggedTower;
	public  AudioClip[] musicClipsPreload;

	private AudioSource musicSource;
	private AudioSource sfxSource;
	private SideMenuController sideMenuScript;
	//private UnityEngine.Object[] MusicClips;

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
	//	if (Input.GetButtonDown ("Fire1") && SceneManager.GetActiveScene().name == "Game") {
	//		Vector3 createPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	//		createPosition.z = 0;
	//		Instantiate (enemy, createPosition, Quaternion.identity);
	//	}
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
