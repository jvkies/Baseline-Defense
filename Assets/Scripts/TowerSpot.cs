using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpot : MonoBehaviour {
	//IPointerClickHandler
	// Use this for initialization
	void Start () {
		
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

	public void OnMouseOver() {
		if (Input.GetMouseButtonDown (0)) {
			if (GameManager.instance.isDragging == true) {
				GameManager.instance.isDragging = false;
				GameManager.instance.draggedTower.transform.SetParent (gameObject.transform);
				GameManager.instance.draggedTower.transform.position = gameObject.transform.position;
				GameManager.instance.draggedTower.transform.localScale = new Vector3(1,1,1);
				GameManager.instance.draggedTower.layer = 0;
			}
		}
	}

}
