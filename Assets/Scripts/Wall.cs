using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Wall : MonoBehaviour {

	private GameObject menuCanvas;
	private GameObject enemyContainer;

	public GameObject wallSpot;
    public int demolishCost = 20;
    public GameObject selectedEffect;

	// Use this for initialization
	void Start () {
		menuCanvas = GameObject.FindWithTag ("MenuCanvas");
		enemyContainer = GameObject.FindWithTag ("EnemyContainer");
		wallSpot = transform.parent.gameObject;
	}

	// Update is called once per frame
	void Update () {

	}

	public void OnMouseOver() {

		if (Input.GetMouseButtonDown (0)  && !EventSystem.current.IsPointerOverGameObject ()) {
			if (GameManager.instance.isTowerSelected == true) {
				GameManager.instance.selectedTower.GetComponent<TowerController> ().selectTower (false);
			}
			else if (GameManager.instance.isWallSelected == true) {
				GameManager.instance.selectedWall.GetComponent<Wall> ().SelectWall (false);
			}
			if (GameManager.instance.isDragging != true && !menuCanvas.GetComponent<MenuController>().escapeMenuPanel.activeSelf) {
				SelectWall (true);
			}
		}

	}

	public void SelectWall( bool active) {
		if (active == true && !GameManager.instance.isGameLost) {
			GameManager.instance.isWallSelected = true;
			GameManager.instance.selectedWall = gameObject;

			selectedEffect.SetActive (true);
			menuCanvas.GetComponent<MenuController> ().DisplayWallMenu (demolishCost);
		} else {
			GameManager.instance.isWallSelected = false;
			GameManager.instance.selectedWall = null;

            selectedEffect.SetActive (false);
			menuCanvas.GetComponent<MenuController> ().HideWallMenu();
		}
	}

	public void DemolishWall()
	{
		SelectWall (false);
    	//GameManager.instance.money += towerStats.towerCost * sellMultiplier;
    	wallSpot.GetComponent<BoxCollider2D> ().enabled = true;
    	wallSpot.GetComponent<TowerSpot>().towerInSlot = null;

		GameManager.instance.UpdateSouls (((-1) * demolishCost), gameObject);
    	AstarPath.active.UpdateGraphs (gameObject.GetComponent<BoxCollider2D> ().bounds);

    	// tell every Mob to calculate a new Path
    	for (int mobNumber = 0; mobNumber < enemyContainer.transform.childCount; mobNumber++) {
    		enemyContainer.transform.GetChild (mobNumber).GetComponent<AIPath> ().SearchPath ();
    	}
	
    	Destroy (gameObject);
	}

}
