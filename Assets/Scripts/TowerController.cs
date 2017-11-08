using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour {

	private float fireCountdown = 0f;
	private Dictionary<int, int> rangeMap;		// the towerRange doest scale with the visual rangeEffect, so we have to map in manually
	private GameObject enemyContainer;
	private GameObject finish;
	private GameObject menuCanvas;

	//public float range = 2f;
	public float rangeOffset=1f;
	public float rotateSpeed = 10f;
	public float sellMultiplier = 0.5f;
	public Tower towerStats;
	public Transform target;
	public GameObject towerHead;
	public GameObject bulletPrefab;
	public GameObject bulletSpawner;
	public GameObject rangeEffect;
	public GameObject selectedEffect;

	// Use this for initialization
	void Start () {
		rangeMap = new Dictionary<int, int> ();
		rangeMap.Add (2, 5);
		rangeMap.Add (3, 7);
		rangeMap.Add (4, 9);
		enemyContainer = GameObject.FindWithTag ("EnemyContainer");
		menuCanvas = GameObject.FindWithTag ("MenuCanvas");
		finish = GameObject.FindWithTag ("Finish");

		rangeEffect.transform.localScale = new Vector3 (visualRange(towerStats.towerRange), visualRange(towerStats.towerRange), 1);
		//InvokeRepeating ("UpdateTarget", 0,0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			Vector3 dir = target.position - transform.position;
			//Quaternion lookRotation = Quaternion.LookRotation (dir);
			//Vector3 rotation = lookRotation.eulerAngles;
			//headToRotate.rotation = Quaternion.Euler (0, 0f, rotation.x);

			var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-90;
			towerHead.transform.rotation = Quaternion.Lerp(towerHead.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotateSpeed);

			if (fireCountdown <= 0f) {
				if (target.GetComponent<MobController> ().mobData.incomingDmg <= target.GetComponent<MobController> ().mobData.health) {
					Shoot ();
					fireCountdown = 1f / towerStats.towerFireRate;
				}
			}
		}

		fireCountdown -= Time.deltaTime;

		if (Input.GetMouseButtonDown (1) && GameManager.instance.isDragging == false) {
			selectTower (false);
		}

		if (Input.GetKeyDown (KeyCode.S) && GameManager.instance.isTowerSelected == true && GameManager.instance.selectedTower == gameObject) {
			SellTower ();
		}

		if (Input.GetKeyDown (KeyCode.U) && GameManager.instance.isTowerSelected == true && GameManager.instance.selectedTower == gameObject) {
			UpgradeTower ();
		}

	}

	public void ActivateTower() {
		InvokeRepeating ("UpdateTarget", 0,0.1f);
	}

	public float visualRange(float towerRangeStat) {		// converts the towerRangeStat e.g. 2 to the visual range of the rangeEffect
		return towerRangeStat*2+1;
	}

	public float actualRange(float towerRangeStat) {		// converts the towerRangeStat to the correct distance on the map the tower can shoot at
		return (towerRangeStat+0.5f)/2f;
	}

	private void UpdateTarget() {
		float shortestDistance = Mathf.Infinity;
		Transform closestEnemy = null;
		List<Transform> enemysInRange = new List<Transform> ();

		foreach (Transform enemy in enemyContainer.transform) {

			if (Vector3.Distance (enemy.position, transform.position) < actualRange(towerStats.towerRange) && enemy.GetComponent<MobController>().mobData.incomingDmg <= enemy.GetComponent<MobController>().mobData.health) {
				enemysInRange.Add(enemy);
			}

		}

		foreach (Transform enemy in enemysInRange) {
			
			float distanceEnemyToFinish = Vector3.Distance (enemy.position, finish.transform.position);

			if (distanceEnemyToFinish < shortestDistance) {
				shortestDistance = distanceEnemyToFinish;
				closestEnemy = enemy;
			}
		}

		target = closestEnemy;
	}

	private void Shoot() {
		GameObject bulletGO = Instantiate (bulletPrefab, bulletSpawner.transform.position, Quaternion.identity);
		Bullet bullet = bulletGO.GetComponent<Bullet> ();
		bullet.damage = towerStats.towerDamage;

		if (bullet != null)
			bullet.Seek (target);
	}

	public void OnMouseOver() {
		if (Input.GetMouseButtonDown (0)) {
			if (GameManager.instance.isTowerSelected == true) {
				GameManager.instance.selectedTower.GetComponent<TowerController> ().selectTower (false);
			}
			if (GameManager.instance.isDragging != true) {
				selectTower (true);
			}
		}

	}

	public void selectTower( bool active) {
		if (active == true) {
			GameManager.instance.isTowerSelected = true;
			GameManager.instance.selectedTower = gameObject;

			menuCanvas.GetComponent<MenuController> ().DisplayTowerMenu (towerStats);

			selectedEffect.SetActive (true);
			rangeEffect.SetActive (true);
		} else {
			GameManager.instance.isTowerSelected = false;
			GameManager.instance.selectedTower = null;

			menuCanvas.GetComponent<MenuController> ().HideTowerMenu();

			selectedEffect.SetActive (false);
			rangeEffect.SetActive (false);

		}
	}

	public void SellTower() {
		selectTower (false);
		GameManager.instance.money += towerStats.towerCost * sellMultiplier;
		GameManager.instance.UpdateMoney ();
		Destroy (gameObject);
	}

	public void UpgradeTower() {
		// Destroy instance and create a new or update values?

		if (towerStats.upgradeID != null && GameManager.instance.money >= towerStats.upgradeCost) {
			GameManager.instance.UpdateMoney (towerStats.upgradeCost * -1);
			towerStats = GameManager.instance.tower [towerStats.upgradeID];	// updating its stats
			menuCanvas.GetComponent<MenuController> ().DisplayTowerMenu (towerStats);
			towerHead.GetComponent<SpriteRenderer> ().sprite = towerStats.towerImageHead;
			rangeEffect.transform.localScale = new Vector3 (visualRange(towerStats.towerRange), visualRange(towerStats.towerRange), 1);

		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, actualRange(towerStats.towerRange));
	}
}

// 2:1.25
// 3:1.75
// 4:2.25
