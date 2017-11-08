using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButtonMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
 {

	public MenuController menuScript;
	// Use this for initialization
	void Start () {
		menuScript = GameObject.FindWithTag ("MenuCanvas").GetComponent<MenuController>();
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		// TODO: im getting the information what Tower Button was hovered on from the gmaeObject.name, not good...
		Tower tower = GameManager.instance.tower [gameObject.name];
		menuScript.DisplayTowerMenu (tower, false);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (GameManager.instance.isTowerSelected) {
			menuScript.DisplayTowerMenu (GameManager.instance.selectedTower.GetComponent<TowerController>().towerStats);
		} else {
			menuScript.HideTowerMenu ();
		}
	}

}
