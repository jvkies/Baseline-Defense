using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulRepeller : MonoBehaviour {

	private PointEffector2D pe2d;

	public float intervall = 1f;

	// Use this for initialization
	void Start () {
		pe2d = GetComponent<PointEffector2D> ();
		StartCoroutine (OnOffSwitch ());
	}
	
	// Update is called once per frame
	void Update () {
	}

	private IEnumerator OnOffSwitch() {
		while (true) {
			yield return new WaitForSeconds (intervall);
			pe2d.enabled = !pe2d.enabled;
		}
	}
}
