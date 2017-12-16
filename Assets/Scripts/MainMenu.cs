using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public float fadeTime = 2f;
	public AudioClip newGameSfx;
	public GameObject blendImageGO;
	private Image blendImage;

	// Use this for initialization
	void Start () {
		blendImage = blendImageGO.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("escape"))
			Application.Quit ();
	}


	public void NewGame() {
		//sceneToLoad = sceneName;
		//newGameBlendImage.gameObject.SetActive(true);
		//isTransitioning = true;

		//GameManager.instance.PlaySfx (newGameSfx);

		Invoke ("LoadSceneLazy", fadeTime);
		StartCoroutine( FadeOut ());
		//GameManager.instance.LoadScene ("Game");

	}

	public void LoadSceneLazy() {
		GameManager.instance.LoadScene ("Game");
	}

	public void ToHighscores() {
		GameManager.instance.highscore = null;
		GameManager.instance.LoadScene ("Highscores");
	}
	public void ExitGame() {
		Application.Quit ();
	}

	IEnumerator FadeOut() {
		float t = 1f;
		blendImageGO.SetActive (true);

		while (t > 0) {
			t -= Time.deltaTime * (1/fadeTime);
			blendImage.color = new Color (1, 1, 1, 1-t);
			yield return 0;
		}

	}


}
