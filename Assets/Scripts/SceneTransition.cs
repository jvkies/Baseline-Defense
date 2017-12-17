using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			ToGameScene ();
		}
	}

	public void ToGameScene() {
		SceneManager.LoadScene ("Game");
	}

}
