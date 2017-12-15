using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class Highscores : MonoBehaviour {

	//public string addScoreURL;

	private float waitBetweenSteps = 0.05f;
	// TODO: This should probably be in a config file
	private string postUrl = "http://www.jovin.de/gaming/BaselineDefenseHighscores.php";
	private string secretKey = "abc123";

	public float countTime = 3f;				// time the counting animation should be running
	public string errorText = "Sorry!  Could  not  retrieve  highscores.";
	public Text waveHighscore;
	public Text timeHighscore;
	public Text mobsHighscore;
	public GameObject statusText;
	public GameObject playerStatHeadline;
	public GameObject playerStatPanel;
	public GameObject highscoreContainer;
	public Button submitButton;
	public Text submitButtonText;
	public Text playernameInput;
	public Dictionary<string, int> test;			// "wave" -> 18, "time" -> 236

	// Use this for initialization
	void Start () {

		if (GameObject.FindWithTag ("GameManager") != null) {
			StartCoroutine (CountTo (GameManager.instance.highscore ["wave"], waveHighscore));
			StartCoroutine (CountTo (GameManager.instance.highscore ["time"], timeHighscore, true));
			StartCoroutine (CountTo (GameManager.instance.highscore ["mobs"], mobsHighscore));
		}
			
		GetHighscores (processHighscoreResult);
	}

	IEnumerator CountTo(int countTo, Text textField, bool isTime = false) {
		int currentCount = 0;

		// TODO: this is working but not fast enough / framerate dependant, maybe try this: https://answers.unity.com/questions/1124303/animate-a-count-how-to-make-a-count-to-grow-smooth.html
		waitBetweenSteps = countTime / countTo;

		while (currentCount <= countTo) {
			if (isTime) {
				textField.text = FormatSecondsToReadableTime(currentCount);
			} else {
				textField.text = currentCount.ToString ();
			}
			currentCount += 1;
			yield return new WaitForSeconds (waitBetweenSteps);
		}
	}

	private string FormatSecondsToReadableTime (int seconds) {
		TimeSpan t = TimeSpan.FromSeconds (seconds);
		return string.Format ("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);

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
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError) {
				statusText.SetActive (true);
				statusText.GetComponentInChildren<Text> ().text = errorText;
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

	public void SendHighscore()
	{
		submitButton.interactable = false;
		submitButtonText.text = "... sending ...";

		string name = playernameInput.text;
		int wave = GameManager.instance.highscore ["wave"];
		int time = GameManager.instance.highscore ["time"];
		int mobs = GameManager.instance.highscore ["mobs"];
		string hash = Md5Sum(name + wave + time + mobs + secretKey);

		WWWForm form = new WWWForm();
		form.AddField("action", "setHighscore");
		form.AddField("name", name);
		form.AddField("wave", wave);
		form.AddField("time", time);
		form.AddField("mobs", mobs);
		form.AddField("version", GameManager.instance.version);
		form.AddField("hash", hash);

		StartCoroutine (SendPostRequest(postUrl, form, sendHighscoreCallback));
	}

	private void processHighscoreResult(String result)
	{
		ClearHighscoreContainer ();

		Debug.Log (fixJson(result));
		highscoreData[] allPlayerHighscores = JsonHelper.FromJson<highscoreData>(fixJson(result));

		statusText.SetActive (false);
		Instantiate (playerStatHeadline, highscoreContainer.transform);

		foreach (highscoreData playerData in allPlayerHighscores) {
			AddPlayerToHighscore (playerData.playername, playerData.wave, playerData.time, playerData.mobs);
		}
	}

	private void sendHighscoreCallback(string result) {
		Debug.Log (result);
		if (result == "1") {
			submitButtonText.text = "Success!";
		} else {
			submitButtonText.text = "Error";
		}
		GetHighscores (processHighscoreResult);

	}

	private void ClearHighscoreContainer() {
		for (int i=0; i<highscoreContainer.transform.childCount; i++) {
			if (highscoreContainer.transform.GetChild (i).name != "StatusText") {
				Destroy (highscoreContainer.transform.GetChild (i).gameObject);
			}
		}
	}

	private void AddPlayerToHighscore(string name, string wave, string time, string mobs) {
		
		GameObject playerScore = Instantiate (playerStatPanel, highscoreContainer.transform);
		Text[] playerStats = playerScore.GetComponentsInChildren<Text> ();		// TODO: there need to be a better way to access these
		playerStats[0].text = name;
		playerStats[1].text = mobs;
		playerStats[2].text = FormatSecondsToReadableTime(int.Parse(time));
		playerStats[3].text = wave;
	}

	string fixJson(string value)
	{
		// stupid JsonHelper workaround with items...
		value = "{\"Items\":" + value + "}";
		return value;
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
