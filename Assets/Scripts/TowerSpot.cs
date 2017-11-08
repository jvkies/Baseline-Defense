using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpot : MonoBehaviour {

	private SpriteRenderer sr;
	private Color defaultColor;
	private GameObject menuCanvas;
	private GameObject towerInSlot = null;

	public GameObject tower1;

	// Use this for initialization
	void Start () {
		menuCanvas = GameObject.FindWithTag ("MenuCanvas");

		sr = gameObject.GetComponent<SpriteRenderer> ();
		defaultColor = sr.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void test (PointerEventData eventData) {
		// 		if (eventData.button.ToString() == "Left") {
		if (GameManager.instance.isDragging == true) {

			Debug.Log (eventData);
			GameManager.instance.isDragging = false;
			//InventoryManager.PutDragInSlot(slotName);


		}
		Debug.Log (eventData);
		Debug.Log(gameObject.name);
	}

	private void PutDragInSlot(GameObject towerInstance) {
		towerInstance.transform.SetParent (gameObject.transform);
		towerInstance.transform.position = gameObject.transform.position;
		towerInstance.transform.localScale = new Vector3 (1, 1, 1);
		towerInstance.layer = 0;
		towerInstance.GetComponent<SpriteRenderer> ().sortingLayerName = "Objects";
		towerInstance.GetComponentsInChildren<SpriteRenderer> () [1].sortingLayerName = "Objects";
		towerInstance.GetComponent<SpriteRenderer> ().sortingOrder -= 2;
		towerInstance.GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder -= 2;
		towerInstance.GetComponent<TowerController> ().ActivateTower ();
		towerInstance.GetComponent<TowerController> ().rangeEffect.SetActive (false);
		towerInSlot = towerInstance;

		GameManager.instance.money -= towerInstance.GetComponent<TowerController> ().towerStats.towerCost;
		GameManager.instance.UpdateMoney ();

	}

	public void OnMouseOver() {
		if (GameManager.instance.isTowerSelected == true) {
			if (Input.GetMouseButtonDown (0)) {
				GameManager.instance.selectedTower.GetComponent<TowerController> ().selectTower (false);
			}
		}
		if (GameManager.instance.isDragging == true) {
			if (Input.GetMouseButtonDown (0) && towerInSlot == null) {

				PutDragInSlot (GameManager.instance.draggedTower);

				if (Input.GetKey (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
					if (GameManager.instance.money >= GameManager.instance.draggedTower.GetComponent<TowerController> ().towerStats.towerCost) {
						menuCanvas.GetComponent<MenuController>().InstantiateTower(GameManager.instance.draggedTower.GetComponent<TowerController>().towerStats.towerID);
						//Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						//GameManager.instance.draggedTower = Instantiate (tower1, mousePos, Quaternion.identity);
						//GameManager.instance.isDragging = true;
					} else {
						GameManager.instance.isDragging = false;
					}
				} else {
					GameManager.instance.isDragging = false;
				}

						
			}
		
			if (Input.GetMouseButtonDown (1)) {
				GameManager.instance.isDragging = false;
				Destroy (GameManager.instance.draggedTower);
			}	
		}
	}

	public void OnMouseEnter() {
		if (GameManager.instance.isDragging == true) {
			sr.color = new Color32(200,200,225,100);;
		}
	}
		
	public void OnMouseExit() {
		sr.color = defaultColor;

	}

}
