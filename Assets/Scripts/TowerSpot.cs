using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpot : MonoBehaviour {

	private SpriteRenderer sr;
	private Color defaultColor;

	public GameObject tower1;

	// Use this for initialization
	void Start () {
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

	private void PutDragInSlot(GameObject tower) {
		tower.transform.SetParent (gameObject.transform);
		tower.transform.position = gameObject.transform.position;
		tower.transform.localScale = new Vector3 (1, 1, 1);
		tower.layer = 0;
		tower.GetComponent<SpriteRenderer> ().sortingOrder -= 2;
		tower.GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder -= 2;
		tower.GetComponent<TowerController> ().ActivateTower ();
		tower.GetComponent<TowerController> ().rangeEffect.SetActive (false);
		GameManager.instance.money -= GameManager.instance.tower["bullettower1"].towerCost;
		GameManager.instance.UpdateMoney ();

	}

	public void OnMouseOver() {
		if (GameManager.instance.isDragging == true) {
			if (Input.GetMouseButtonDown (0)) {

				PutDragInSlot (GameManager.instance.draggedTower);

				if (Input.GetKey (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
					if (GameManager.instance.money >= GameManager.instance.tower["bullettower1"].towerCost) { //TODO get towerprice from GameManager
						Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						GameManager.instance.draggedTower = Instantiate (tower1, mousePos, Quaternion.identity);
						GameManager.instance.isDragging = true;
						//GameManager.instance.money -= 10; //TODO get towerprice from GameManager
						GameManager.instance.UpdateMoney ();
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
