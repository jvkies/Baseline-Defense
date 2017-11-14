using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public AudioClip newGameSfx;

	// Use this for initialization
	void Start () {
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
		GameManager.instance.PlaySfx (newGameSfx);

	//	Invoke ("LoadSceneLazy", 2f);
		GameManager.instance.LoadScene ("Game");

	}

	public void LoadSceneLazy() {
		GameManager.instance.LoadScene ("Game");
	}

	public void ExitGame() {
		Application.Quit ();
	}



}
