using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpot : MonoBehaviour {

	private SpriteRenderer sr;
	private Color defaultColor;
	private GameObject menuCanvas;
	private Transform enemyContainer;
	private GameObject towerInSlot = null;

	public GameObject tower1;
	public AstarPath astarPath;

	// Use this for initialization
	void Start () {
		menuCanvas = GameObject.FindWithTag ("MenuCanvas");
		enemyContainer = GameObject.FindWithTag("EnemyContainer").transform;

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

	public bool WouldBlockPath() {
		// TODO: implement WouldBlockPath
		// There must be an AstarPath instance in the scene
		if (AstarPath.active == null) return false;			// if there is no AstarPath, nothing would block
		return false;

		// We can calculate multiple paths asynchronously
	//	for (int i = 0; i < 10; i++) {
			// As there is not Seeker to keep track of the callbacks, we now need to specify the callback every time again
	//		var p = ABPath.Construct(transform.position, transform.position+transform.forward*i*10, OnPathComplete);
			// Start the path by calling the AstarPath component directly
			// AstarPath.active is the active AstarPath instance in the scene
	//		AstarPath.StartPath (p);
	//	}
	}

	private void PutDragInSlot(GameObject towerInstance) {
		// TODO: implement WouldBlockPath()
		if (!WouldBlockPath ()) {
			towerInstance.transform.SetParent (gameObject.transform);
			towerInstance.transform.position = gameObject.transform.position;
			towerInstance.transform.localScale = new Vector3 (1, 1, 1);
			towerInstance.GetComponent<SpriteRenderer> ().sortingLayerName = "Objects";
			towerInstance.GetComponentsInChildren<SpriteRenderer> () [1].sortingLayerName = "Objects";
			towerInstance.GetComponent<SpriteRenderer> ().sortingOrder -= 2;
			towerInstance.GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder -= 2;
			towerInstance.GetComponent<TowerController> ().ActivateTower ();
			towerInstance.GetComponent<TowerController> ().rangeEffect.SetActive (false);
			towerInSlot = towerInstance;
			towerInstance.layer = 1;
		

			// add the new tower as unpassable to our Navigation Grid
			AstarPath.active.UpdateGraphs (towerInstance.GetComponent<BoxCollider2D> ().bounds);

			// tell every Mob to calculate a new Path
			for (int mobNumber = 0; mobNumber < enemyContainer.childCount; mobNumber++) {
				enemyContainer.GetChild (mobNumber).GetComponent<AIPath> ().SearchPath ();
			}

			//GameManager.instance.money -= towerInstance.GetComponent<TowerController> ().towerStats.towerCost;
			GameManager.instance.UpdateSouls (-towerInstance.GetComponent<TowerController> ().towerStats.towerCost);
		}
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
					if (GameManager.instance.souls > GameManager.instance.draggedTower.GetComponent<TowerController> ().towerStats.towerCost) {
						menuCanvas.GetComponent<MenuController>().InstantiateTower(GameManager.instance.draggedTower.GetComponent<TowerController>().towerStats.towerID);
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
