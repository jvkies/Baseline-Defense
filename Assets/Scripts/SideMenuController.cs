﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SideMenuController : MonoBehaviour {

	public Text health;
	public Text money;
	public GameObject Tower1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.isDragging) {
			Vector3 objectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			objectPos.z = 0;
			GameManager.instance.draggedTower.transform.position = objectPos;
		//	GameManager.instance.draggedTower.transform.position = Input.mousePosition;
		}
	}

	public void UpdateHealth(string amount) {
		health.text = amount;
	}

	public void BuildTower(string tower) {
		if (GameManager.instance.isDragging == false) {
			Vector3 objectPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			GameManager.instance.draggedTower = Instantiate (Tower1, objectPos, Quaternion.identity);
			GameManager.instance.isDragging = true;
		}
	}


}
