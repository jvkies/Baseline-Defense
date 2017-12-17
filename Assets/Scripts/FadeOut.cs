using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

	// TODO: There is a similar class 'FadeIn' which basically does the same..

	// Use this for initialization
	void Start () {
		StartCoroutine (FadingOut (0.2f));
	}
		
	private IEnumerator FadingOut(float duration) {
		SpriteRenderer beSR = GetComponent<SpriteRenderer> ();
		beSR.color = new Color(beSR.color.r, beSR.color.g, beSR.color.b, 0.8f);

		while (beSR.color.a > 0.0f)
		{
			beSR.color = new Color(beSR.color.r, beSR.color.g, beSR.color.b, beSR.color.a - (Time.deltaTime / duration));
			yield return null;
		}

		Destroy (gameObject);
	}
}
