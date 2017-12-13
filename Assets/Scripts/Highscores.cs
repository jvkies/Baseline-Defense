using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Highscores : MonoBehaviour {

	private float waitBetweenSteps = 0.05f;

	public float countTime = 3f;				// time the counter should be running
	public Text waveHighscore;
	public Text timeHighscore;
	public Text mobsHighscore;

	// Use this for initialization
	void Start () {
		//timeHighscore.text = GameManager.instance.highscore ["time"].ToString ();

		StartCoroutine(CountTo(GameManager.instance.highscore ["wave"],waveHighscore));
		StartCoroutine(CountTo(GameManager.instance.highscore ["time"],timeHighscore,true));
		StartCoroutine(CountTo(GameManager.instance.highscore ["mobs"],mobsHighscore));

	}
	

	IEnumerator CountTo(int countTo, Text textField, bool isTime = false) {
		int currentCount = 0;

		// TODO: this is working but not fast enough / framerate dependant, maybe try this: https://answers.unity.com/questions/1124303/animate-a-count-how-to-make-a-count-to-grow-smooth.html
		waitBetweenSteps = countTime / countTo;

		while (currentCount <= countTo) {
			if (isTime) {
				TimeSpan t = TimeSpan.FromSeconds (currentCount);
				textField.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
			} else {
				textField.text = currentCount.ToString ();
			}
			currentCount += 1;
			yield return new WaitForSeconds (waitBetweenSteps);
				
		}
	}

	public void TransitionToScene ( string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void ExitApplication() {
		Application.Quit ();
	}
		
}
