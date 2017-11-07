﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public Text health;
	public Text money;
	public GameObject towerName;
	public GameObject sellButton;
	public GameObject upgradeButton;
	public GameObject bulletTower1Prefab;
	public GameObject bulletTower1Button;
	public GameObject waveDisplayer;
	public GameObject towerMenuPanel;
	public GameObject escapeMenuPanel;
	public Text damageValue;
	public Text attackSpeedValue;
	public Text rangeValue;
	public Text costValue;

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

	public void UpdateMoney(string amount) {
		money.text = amount;
	}

	public void InstantiateTower(string towerID) {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		GameManager.instance.draggedTower = Instantiate (bulletTower1Prefab, mousePos, Quaternion.identity);
		GameManager.instance.draggedTower.GetComponent<TowerController> ().towerStats = GameManager.instance.tower [towerID].Copy();	// TowerController knows what tower its holding
		GameManager.instance.isDragging = true;

	}

	public void BuildTower(string towerID) {
		if (GameManager.instance.isDragging == false && GameManager.instance.money >= GameManager.instance.tower[towerID].towerCost) {
			InstantiateTower (towerID);
		}
	}

	public void SellTower() {
		if (GameManager.instance.isTowerSelected) {
			GameManager.instance.selectedTower.GetComponent<TowerController> ().SellTower ();
		}
	}

	public void UpgradeTower() {
		if (GameManager.instance.isTowerSelected) {
			GameManager.instance.selectedTower.GetComponent<TowerController> ().UpgradeTower ();
		}
	}

	public void DisplayTowerMenu(string _towerName, int _towerDamage, int _towerShootspeed, float _towerRange, float _towerCost, bool isSellEnablbed = true) {
		damageValue.text = _towerDamage.ToString();
		attackSpeedValue.text = _towerShootspeed.ToString();
		rangeValue.text = _towerRange.ToString();
		costValue.text = _towerCost.ToString();
		towerName.GetComponent<Text> ().text = _towerName;
		sellButton.SetActive (isSellEnablbed);
		towerMenuPanel.SetActive (true);

	}

	public void HideTowerMenu() {
		towerMenuPanel.SetActive (false);
	}

	public void DisplayWin() {
		waveDisplayer.GetComponent<Text> ().text = "You Win!"; 
		waveDisplayer.GetComponent<Text> ().color = new Color32 (165, 255, 165, 255); 
		waveDisplayer.SetActive (true);
	}

	public void DisplayLoose() {
		waveDisplayer.GetComponent<Text> ().text = "You Lost!"; 
		waveDisplayer.GetComponent<Text> ().color = new Color32 (255, 110, 110, 255); 
		waveDisplayer.SetActive (true);
	}

	public void ToggleEscapeMenu() {
		if (escapeMenuPanel.activeSelf) {
			escapeMenuPanel.SetActive (false);
			Time.timeScale = 1;
		} else {
			escapeMenuPanel.SetActive (true);
			Time.timeScale = 0;
		}
	}

	public void LoadMainScene() {
		GameManager.instance.MainMenuScene ();
	}
		
}