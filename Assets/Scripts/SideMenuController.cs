using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SideMenuController : MonoBehaviour {

	public Text health;
	public Text money;
	public GameObject Tower1;

	private bool isDragging = false;
	private GameObject draggedTower;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isDragging) {
			var mousePos = Input.mousePosition;
			mousePos.z = 2.0f;       // we want 2m away from the camera position
			var objectPos = Camera.current.ScreenToWorldPoint(mousePos);
			draggedTower.transform.position = objectPos;
		}
	}

	public void UpdateHealth(string amount) {
		health.text = amount;
	}

	public void BuildTower(string tower) {
		Debug.Log ("building tower " + tower);

		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 2.0f;       // we want 2m away from the camera position
		Vector3 objectPos = Camera.current.ScreenToWorldPoint(mousePos);
		GameObject draggedTower = Instantiate(Tower1, objectPos, Quaternion.identity);

	}
}
