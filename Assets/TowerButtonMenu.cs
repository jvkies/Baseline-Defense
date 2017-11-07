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
		menuScript.DisplayTowerMenu (tower.towerName, tower.towerDamage, tower.towerShootspeed, tower.towerRange, tower.towerCost, false);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		menuScript.HideTowerMenu ();
	}

}
