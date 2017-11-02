using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;

	public string musicFolder = "music by neocrey";		// Music in Folder "Resources/music by neocrey"
	public  AudioClip[] musicClipsPreload;

	private AudioSource musicSource;
	private AudioSource sfxSource;
	private SideMenuController sideMenuScript;
	//private UnityEngine.Object[] MusicClips;

	public int health = 20;
	public int money = 50;

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
		sideMenuScript = GameObject.FindWithTag ("SideMenuCanvas").GetComponent<SideMenuController>();
		//SceneManager.sceneLoaded += OnLevelFinishedLoading;

	}

	// Use this for initialization
	void Start () {
		// InitMusic ();  
		// works, but music too large for github
		
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
			Debug.Log ("game over");
			// TODO: game over
		}

	}

	private void InitMusic() {

		try {
			
			//MusicClips = Resources.LoadAll(musicFolder, typeof(AudioClip)); // TAKES TO LONG

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
