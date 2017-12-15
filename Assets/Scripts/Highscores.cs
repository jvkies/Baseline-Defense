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

	// TODO: This should probably be in a config file
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


//		Example SendHighscore Call
//		Dictionary<string, int> hs = new Dictionary<string, int> ();
//		hs.Add ("wave", 10);
//		hs.Add ("time", 1500);
//		hs.Add ("mobs", 500);
//		SendHighscore ("cray", hs);

//		Example GetHighscore Call
		GetHighscores (processHighscoreResult);
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

	// TODO: Do we want to return something on error which the callback may handle, like null?
	private IEnumerator SendPostRequest(string url, WWWForm form, Action<string> successCallback = null)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(url, form))
		{
			yield return www.Send();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
			} else if (successCallback != null) {
				successCallback (www.downloadHandler.text);
			} else {
				Debug.Log (www.downloadHandler.text);
			}
		}
	}

	public void GetHighscores(Action<string> successCallback)
	{
		WWWForm form = new WWWForm();
		form.AddField("action", "getHighscore");
		StartCoroutine (SendPostRequest(postUrl, form, successCallback));
	}

	public void SendHighscore(string name, Dictionary<string, int> highscore)
	{
		int wave = highscore ["wave"];
		int time = highscore ["time"];
		int mobs = highscore ["mobs"];
		string hash = Md5Sum(name + wave + time + mobs + secretKey);

		WWWForm form = new WWWForm();
		form.AddField("action", "setHighscore");
		form.AddField("name", name);
		form.AddField("wave", wave);
		form.AddField("time", time);
		form.AddField("mobs", mobs);
		form.AddField("hash", hash);

		StartCoroutine (SendPostRequest(postUrl, form));
	}

	private void processHighscoreResult(String result)
	{
		Debug.Log (result);
	}


	private void AddPlayerToHighscore() {

	}
		
	// TODO: Util function - can be moved?
	public  string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}
}
