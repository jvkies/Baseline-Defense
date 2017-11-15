using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class TowerSpot : MonoBehaviour {

	private int numberOfMobsOnSpot = 0;			// number of mobs currently collided and so on this spot
	private SpriteRenderer sr;
	private Color defaultColor;
	private GameObject menuCanvas;
	private Transform enemyContainer;

	public Transform spawnPosition;
	public Transform goalPosition;
	public GameObject towerInSlot = null;
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

	public bool IsTowerSpotClear() {
		if (numberOfMobsOnSpot == 0)
			return true;
		else
			return false;
			
	}

	public bool WouldBlockPath() {
		
		// There must be an AstarPath instance in the scene
		if (AstarPath.active == null) {
			Debug.Log ("Error: no AstarPath Object found");
			return false;				// if there is no AstarPath, nothing would block
		}

		gameObject.layer = 1;			// objects in this layer block path

		GraphUpdateObject guo = new GraphUpdateObject (gameObject.GetComponent<BoxCollider2D>().bounds);
		GraphNode spawnPointNode = AstarPath.active.GetNearest (spawnPosition.position).node;
		GraphNode goalNode = AstarPath.active.GetNearest (goalPosition.position).node;

		if (GraphUpdateUtilities.UpdateGraphsNoBlock (guo, spawnPointNode, goalNode, true)) {
			gameObject.layer = 0;
			return false;
		} else {
			gameObject.layer = 0;
			return true;
		}

	}

	private bool PutDragInSlot(GameObject towerInstance) {
		if (!WouldBlockPath () && IsTowerSpotClear()) {
			towerInstance.transform.SetParent (gameObject.transform);
			towerInstance.transform.position = gameObject.transform.position;
			towerInstance.transform.localScale = new Vector3 (1, 1, 1);
			towerInstance.GetComponent<SpriteRenderer> ().sortingLayerName = "Objects";
			towerInstance.GetComponentsInChildren<SpriteRenderer> () [1].sortingLayerName = "Objects";
			towerInstance.GetComponent<SpriteRenderer> ().sortingOrder -= 2;
			towerInstance.GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder -= 2;
			towerInstance.GetComponent<TowerController> ().ActivateTower ();
			towerInstance.GetComponent<TowerController> ().rangeEffect.SetActive (false);
			towerInstance.GetComponent<TowerController> ().towerSpot = gameObject;
			towerInSlot = towerInstance;
			towerInstance.layer = 1;
		
			gameObject.GetComponent<BoxCollider2D> ().enabled = false;

			// add the new tower as unpassable to our Navigation Grid
			AstarPath.active.UpdateGraphs (towerInstance.GetComponent<BoxCollider2D> ().bounds);

			// tell every Mob to calculate a new Path
			for (int mobNumber = 0; mobNumber < enemyContainer.childCount; mobNumber++) {
				enemyContainer.GetChild (mobNumber).GetComponent<AIPath> ().SearchPath ();
			}

			//GameManager.instance.money -= towerInstance.GetComponent<TowerController> ().towerStats.towerCost;
			GameManager.instance.UpdateSouls (-towerInstance.GetComponent<TowerController> ().towerStats.towerCost);

			return true;
		}
		return false;
	}

	public void OnMouseOver() {
		if (GameManager.instance.isTowerSelected == true) {
			if (Input.GetMouseButtonDown (0)) {
				GameManager.instance.selectedTower.GetComponent<TowerController> ().selectTower (false);
			}
		}
		if (GameManager.instance.isDragging == true) {
			if (Input.GetMouseButtonDown (0) && towerInSlot == null) {

				if (PutDragInSlot (GameManager.instance.draggedTower)) {

					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
						if (GameManager.instance.souls > GameManager.instance.draggedTower.GetComponent<TowerController> ().towerStats.towerCost) {
							menuCanvas.GetComponent<MenuController> ().InstantiateTower (GameManager.instance.draggedTower.GetComponent<TowerController> ().towerStats.towerID);
						} else {
							GameManager.instance.isDragging = false;
						}
					} else {
						GameManager.instance.isDragging = false;
					}
				}
						
			}
		
			if (Input.GetMouseButtonDown (1)) {
				GameManager.instance.isDragging = false;
				Destroy (GameManager.instance.draggedTower);
			}	
		}
	}

	void OnTriggerEnter2D ( Collider2D other) {
		if (other.tag == "Mob") {
			numberOfMobsOnSpot += 1;
		}

	}
	void OnTriggerExit2D ( Collider2D other) {
		if (other.tag == "Mob") {
			numberOfMobsOnSpot -= 1;
		}

	}


	void OnCollisionEnter2D (Collision2D col) {
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
