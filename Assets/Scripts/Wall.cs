using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseOver() {
		if (Input.GetMouseButtonDown (0)) {
	//		if (GameManager.instance.isTowerSelected == true) {
	//			GameManager.instance.selectedTower.GetComponent<TowerController> ().selectTower (false);
	//		}
	//		if (GameManager.instance.isDragging != true && !menuCanvas.GetComponent<MenuController>().escapeMenuPanel.activeSelf) {
	//			selectTower (true);
	//		}
		}

	}

	public void selectTower( bool active) {
		if (active == true && !GameManager.instance.isGameLost) {
			GameManager.instance.isTowerSelected = true;
			GameManager.instance.selectedTower = gameObject;

	//		menuCanvas.GetComponent<MenuController> ().DisplayTowerMenu (towerStats);

	//		selectedEffect.SetActive (true);
	//		rangeEffect.SetActive (true);
		} else {
			GameManager.instance.isTowerSelected = false;
			GameManager.instance.selectedTower = null;

	//		menuCanvas.GetComponent<MenuController> ().HideTowerMenu();

	//		selectedEffect.SetActive (false);
	//		rangeEffect.SetActive (false);

		}
	}

}
