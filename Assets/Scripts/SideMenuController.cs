using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SideMenuController : MonoBehaviour {

	public Text health;
	public Text money;
	public GameObject sellButton;
	public GameObject upgradeButton;
	public GameObject bulletTower1;

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
		GameManager.instance.draggedTower = Instantiate (bulletTower1, mousePos, Quaternion.identity);
		GameManager.instance.draggedTower.GetComponent<TowerController> ().towerStats = GameManager.instance.tower [towerID];	// TowerController knows what tower its holding
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



}
