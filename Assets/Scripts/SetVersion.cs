using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVersion : MonoBehaviour {

	public TextAsset version;

	// Use this for initialization
	void Start () {
		GameManager.instance.version = version.text;
		GetComponent<Text> ().text = version.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
