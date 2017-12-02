using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

	private Image blendImage;

	public float fadeTime = 1f;

	// Use this for initialization
	void Start () {
		blendImage = GetComponent<Image> ();
		StartCoroutine (FadeingIn ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator FadeingIn() {
		float t = 1f;

		while (t > 0) {
			t -= Time.deltaTime * (1/fadeTime);
			blendImage.color = new Color (1, 1, 1, t);
			yield return 0;
		}
		gameObject.SetActive (false);

	}

}
