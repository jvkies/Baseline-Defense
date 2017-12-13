using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscores : MonoBehaviour {

	public Text waveHighscore;
	public Text timeHighscore;
	public Text mobsHighscore;

	// Use this for initialization
	void Start () {
		waveHighscore.text = GameManager.instance.highscore ["wave"].ToString ();
		timeHighscore.text = GameManager.instance.highscore ["time"].ToString ();
		mobsHighscore.text = GameManager.instance.highscore ["mobs"].ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TransitionToScene ( string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void ExitApplication() {
		Application.Quit ();
	}
		
}
