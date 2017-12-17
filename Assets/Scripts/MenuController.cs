using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour {

	private int tipIndex = 0;
	private GameObject towerToInstantiate;
		
	public Text souls;
	public Text money;
	public Text towerName;
	public GameObject sellButton;
	public GameObject upgradeButton;
	public GameObject demolishButton;
	public GameObject bulletTowerPrefab;
	//public GameObject bulletTowerButton;
	public GameObject rockTowerPrefab;
	public GameObject rockTowerButton;
	public GameObject waveDisplayer;
	public GameObject towerMenuPanel;
	public GameObject wallMenuPanel;
	public GameObject escapeMenuPanel;
	public GameObject upgradeDamageValue;
	public GameObject upgradeFireRateValue;
	public GameObject upgradeRangeValue;
	public GameObject upgradeAoeValue;
	public GameObject upgradeCostValue;
	public Text damageValue;
	public Text fireRateValue;
	public Text rangeValue;
	public Text aoeValue;
	public Text costValue;
	public Text demolishValue;
	public GameObject startWaveContainer;
	public Text tipText;
	public Text waveButtonText;
	public GameObject endGamePanel;

	//private string[] tipArray = {
	//	"Tip: Hold down shift to place multiple towers.",
	//	"Tip: Did you figure out the spawning pattern yet?",
	//	"Tip: Always brush your teeth before going to bed.",
	//};

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
		Debug.Log ("Menu Controller sell tower");
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
		aoeValue.text = towerStats.aoeRange.ToString();
		towerName.text = towerStats.towerName;

		sellButton.SetActive (isNoPreview);
		upgradeButton.SetActive (isNoPreview);
		upgradeDamageValue.SetActive (isNoPreview);
		upgradeRangeValue.SetActive (isNoPreview);
		upgradeFireRateValue.SetActive (isNoPreview);
		upgradeAoeValue.SetActive (isNoPreview);
		upgradeCostValue.SetActive (isNoPreview);

		if (towerStats.upgradeID == null) {
			upgradeButton.SetActive (false);
			upgradeDamageValue.SetActive (false);
			upgradeRangeValue.SetActive (false);
			upgradeFireRateValue.SetActive (false);
			upgradeAoeValue.SetActive (false);
			upgradeCostValue.SetActive (false);

		} else {
			Tower upgradeTower = GameManager.instance.tower [towerStats.upgradeID];
			upgradeDamageValue.GetComponent<Text> ().text = upgradeTower.towerDamage.ToString();
			upgradeFireRateValue.GetComponent<Text> ().text = upgradeTower.towerFireRate.ToString();
			upgradeRangeValue.GetComponent<Text> ().text = upgradeTower.towerRange.ToString();
			upgradeAoeValue.GetComponent<Text> ().text = upgradeTower.aoeRange.ToString();
			upgradeCostValue.GetComponent<Text> ().text = towerStats.upgradeCost.ToString();
			if (towerStats.upgradeCost > GameManager.instance.souls) {
				upgradeButton.GetComponent<Button> ().interactable = false;
			} else {
				upgradeButton.GetComponent<Button> ().interactable = true;
			}

		}
		towerMenuPanel.SetActive (true);

	}

	public void HideTowerMenu() {
		towerMenuPanel.SetActive (false);
	}

	public void DemolishWall() {
		if (GameManager.instance.isWallSelected) {
			GameManager.instance.selectedWall.GetComponent<Wall>().DemolishWall ();
		}
	}

    public void DisplayWallMenu(int demolishCost)
    {

		demolishButton.SetActive (true);
		demolishValue.text = demolishCost.ToString();

    	if (demolishCost > GameManager.instance.souls)
    	{
			demolishButton.GetComponent<Button> ().interactable = false;
   		}
   		else
   		{
			demolishButton.GetComponent<Button> ().interactable = true;
        }
		wallMenuPanel.SetActive (true);
    }

    public void HideWallMenu()
    {
		wallMenuPanel.SetActive (false);
    }

	public void DisplayWin() {
		waveDisplayer.GetComponent<Text> ().text = "You Win!";
		waveDisplayer.GetComponent<Text> ().color = new Color32 (165, 255, 165, 255); 
		waveDisplayer.SetActive (true);
	}

	public void DisplayLoose() {
		waveDisplayer.GetComponent<Text> ().text = "Defeat"; 
		waveDisplayer.GetComponent<Text> ().color = new Color32 (255, 110, 110, 255); 
		waveDisplayer.SetActive (true);
	}

	public void CloseEscapeMenu() {
		escapeMenuPanel.SetActive (false);
		Time.timeScale = 1;
	}

	public void ToggleEscapeMenu() {
		if (escapeMenuPanel.activeSelf) {
			CloseEscapeMenu();
		} else {
			// open escape menu

			tipText.text = Tipps.tipArray [tipIndex];
			tipIndex += 1;
			if (tipIndex >= Tipps.tipArray.Length)
				tipIndex = 0;

			if (GameManager.instance.isTowerSelected)
				GameManager.instance.selectedTower.GetComponent<TowerController> ().selectTower (false);
			escapeMenuPanel.SetActive (true);
			Time.timeScale = 0;
		}
	}

	public void SetWaveButtonText(string text) {
		waveButtonText.text = text;
	}

	public void DisplayEndGamePanel() {
		endGamePanel.SetActive (true);
	}

	public void Retry() {
		CloseEscapeMenu ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void HighscoreScene() {
		SceneManager.LoadScene ("Highscores");
	}

	public void ExitApplication() {
		Application.Quit ();
	}

	public void LoadMainScene() {
		GameManager.instance.MainMenuScene ();
	}
		
}
