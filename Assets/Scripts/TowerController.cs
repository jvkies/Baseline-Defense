using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

	private float fireCountdown = 0f;
	private GameObject enemyContainer;
	private GameObject finish;

	public float range = 2f;
	public float rotateSpeed = 10f;
	public float fireRate = 1f;
	public Transform target;
	public Transform headToRotate;
	public GameObject bulletPrefab;
	public GameObject bulletSpawner;
	public GameObject rangeEffect;

	// Use this for initialization
	void Start () {
		enemyContainer = GameObject.FindWithTag ("EnemyContainer");
		finish = GameObject.FindWithTag ("Finish");

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
			headToRotate.rotation = Quaternion.Lerp(headToRotate.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotateSpeed);

			if (fireCountdown <= 0f) {
				Shoot ();
				fireCountdown = 1f / fireRate;
			}
			//Debug.Log (fireCountdown);
			//Debug.Log (rotation);
		}
		fireCountdown -= Time.deltaTime;

	}

	public void ActivateTower() {
		InvokeRepeating ("UpdateTarget", 0,0.1f);
	}

	private void UpdateTarget() {
		float shortestDistance = Mathf.Infinity;
		Transform closestEnemy = null;
		List<Transform> enemysInRange = new List<Transform> ();

		foreach (Transform enemy in enemyContainer.transform) {

			if (Vector3.Distance (enemy.position, transform.position) < range && enemy.GetComponent<Mob>().incomingDmg <= enemy.GetComponent<Mob>().health) {
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

		if (bullet != null)
			bullet.Seek (target);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range );
	}
}
