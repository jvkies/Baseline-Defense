using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class Highscores : MonoBehaviour {

	public string addScoreURL;

	private float waitBetweenSteps = 0.05f;

	private string postUrl = "http://lmt.jovin.de/BD_db_connector.php";
	private string secretKey = "abc123";

	public float countTime = 3f;				// time the counter should be running
	public Text waveHighscore;
	public Text timeHighscore;
	public Text mobsHighscore;

	// Use this for initialization
	void Start () {

		//timeHighscore.text = GameManager.instance.highscore ["time"].ToString ();

//		StartCoroutine(CountTo(GameManager.instance.highscore ["wave"],waveHighscore));
//		StartCoroutine(CountTo(GameManager.instance.highscore ["time"],timeHighscore,true));
//		StartCoroutine(CountTo(GameManager.instance.highscore ["mobs"],mobsHighscore));

		GetHighscores ();
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

	public IEnumerator SendPostRequest(string url, WWWForm form, Action successCallback = null)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(url, form))
		{
			yield return www.Send();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				if (successCallback != null )
				{
					successCallback(www.downloadHandler.text);
				}
			}

		}
	}

	public IEnumerator SendHighscore(string name, Dictionary<string, int> highscore)
	{
		int wave = highscore ["wave"];
		int time = highscore ["time"];
		int mobs = highscore ["mobs"];

		string hash = Md5Sum(name + wave + time + mobs + secretKey);

		WWWForm form = new WWWForm();
		form.AddField("action", "Highscore");
		form.AddField("name", name);
		form.AddField("wave", wave);
		form.AddField("time", time);
		form.AddField("mobs", mobs);
		form.AddField("hash", hash);

		StartCoroutine (SendPostRequest(postUrl, form));
	}

	public void GetHighscores()
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "getHighscore");
		StartCoroutine (SendPostRequest(postUrl, form, processHighscoreResult));
	}

//	public void GetAllHighscore() {
//
//		// bau hash
//		string hash = "";
//		// Hash128 hash
//
//		// baue mir den post string zusammen
//		string postUrlGetHighscore = addScoreURL + "?GetHighscore" + "&hash=" + hash;
//
//		// schicke an script online auf webbseite
//		WWW www = new WWW(postUrlGetHighscore); //GET data is sent via the URL
//
//		// empfange daten
//
////		while(!www.isDone && string.IsNullOrEmpty(www.error)) {
////			gameObject.guiText.text = "Loading... " + www.Progress.ToString("0%"); //Show progress
////			yield return null;
////		}
//
////		if(string.IsNullOrEmpty(www.error)) gameObject.guiText.text = www.text;
////		else Debug.LogWarning(www.error);
//
//		// zeige daten in spietabelle an
//		AddPlayerToHighscore();
//	}

	private void AddPlayerToHighscore() {

	}

	private void processHighscoreResult(String result)
	{
		Debug.Log (result);
	}


		
}
