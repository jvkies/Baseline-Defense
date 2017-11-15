using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuController : MonoBehaviour {

	private int tipIndex = 0;
	private GameObject towerToInstantiate;
		
	public Text souls;
	public Text money;
	public GameObject towerName;
	public GameObject sellButton;
	public GameObject upgradeButton;
	public GameObject bulletTowerPrefab;
	//public GameObject bulletTowerButton;
	public GameObject rockTowerPrefab;
	public GameObject rockTowerButton;
	public GameObject waveDisplayer;
	public GameObject towerMenuPanel;
	public GameObject escapeMenuPanel;
	public GameObject upgradeDamageValue;
	public GameObject upgradeFireRateValue;
	public GameObject upgradeRangeValue;
	public GameObject upgradeCostValue;
	public Text damageValue;
	public Text fireRateValue;
	public Text rangeValue;
	public Text costValue;
	public GameObject startWaveContainer;
	public Text tipText;

	private string[] tipArray = {
		"Tip: Hold down shift to place multiple towers.",
		"Tip: Did you figure out the spawning pattern yet?",
		"Tip: Always brush your teeth before going to bed.",
	};

	void Update () {
		if (GameManager.instance.isDragging) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			GameManager.instance.draggedTower.transform.position = mousePos;
		}
	}

	public void UpdateSouls(string amount) {
		souls.text = amount;
	}

	// TODO: deprecated
	//public void UpdateHealth(string amount) {
		//health.text = amount;
	//}

	// TODO: deprecated
//	public void UpdateMoney(string amount) {
//		money.text = amount;
//	}

	public void InstantiateTower(string towerID) {
		if (towerID == "rocktower1")
			towerToInstantiate = rockTowerPrefab;
		else
			towerToInstantiate = bulletTowerPrefab;
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		GameManager.instance.draggedTower = Instantiate (towerToInstantiate, mousePos, Quaternion.identity);
		GameManager.instance.draggedTower.GetComponent<TowerController> ().towerStats = GameManager.instance.tower [towerID].Copy();	// TowerController knows what tower its holding
		GameManager.instance.isDragging = true;
	}

	public void BuildTower(string towerID) {
		if (GameManager.instance.isDragging == false && GameManager.instance.souls > GameManager.instance.tower[towerID].towerCost) {
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

	public void DisplayTowerMenu(Tower towerStats, bool isNoPreview = true) {
		damageValue.text = towerStats.towerDamage.ToString();
		fireRateValue.text = towerStats.towerFireRate.ToString();
		rangeValue.text = towerStats.towerRange.ToString();
		costValue.text = towerStats.towerCost.ToString();

		towerName.GetComponent<Text> ().text = towerStats.towerName;

		sellButton.SetActive (isNoPreview);
		upgradeButton.SetActive (isNoPreview);
		upgradeDamageValue.SetActive (isNoPreview);
		upgradeRangeValue.SetActive (isNoPreview);
		upgradeFireRateValue.SetActive (isNoPreview);
		upgradeCostValue.SetActive (isNoPreview);

		if (towerStats.upgradeID == null) {
			upgradeButton.SetActive (false);
			upgradeDamageValue.SetActive (false);
			upgradeRangeValue.SetActive (false);
			upgradeFireRateValue.SetActive (false);
			upgradeCostValue.SetActive (false);

		} else {
			Tower upgradeTower = GameManager.instance.tower [towerStats.upgradeID];
			upgradeDamageValue.GetComponent<Text> ().text = upgradeTower.towerDamage.ToString();
			upgradeFireRateValue.GetComponent<Text> ().text = upgradeTower.towerFireRate.ToString();
			upgradeRangeValue.GetComponent<Text> ().text = upgradeTower.towerRange.ToString();
			upgradeCostValue.GetComponent<Text> ().text = towerStats.upgradeCost.ToString();

		}
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
			// close escape menu
			escapeMenuPanel.SetActive (false);
			Time.timeScale = 1;
		} else {
			// open escape menu

			tipText.text = tipArray [tipIndex];
			tipIndex += 1;
			if (tipIndex >= tipArray.Length)
				tipIndex = 0;

			if (GameManager.instance.isTowerSelected)
				GameManager.instance.selectedTower.GetComponent<TowerController> ().selectTower (false);
			escapeMenuPanel.SetActive (true);
			Time.timeScale = 0;
		}
	}

	public void ExitApplication() {
		Application.Quit ();
	}

	public void LoadMainScene() {
		GameManager.instance.MainMenuScene ();
	}
		
}
