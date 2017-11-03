using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

	public Transform target;
	public Transform headToRotate;
	public float range = 2f;
	public float rotateSpeed = 10f;
	public float fireRate = 2f;

	private GameObject enemyContainer;
	private GameObject finish;
	private float fireCountdown = 0f;

	// Use this for initialization
	void Start () {
		enemyContainer = GameObject.FindWithTag ("EnemyContainer");
		finish = GameObject.FindWithTag ("Finish");

		InvokeRepeating ("UpdateTarget", 0,1f);
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
				Debug.Log ("resetting fireCountdown");
			}
			//Debug.Log (fireCountdown);
			fireCountdown -= Time.deltaTime;
			//Debug.Log (rotation);
		}
	}

	private void UpdateTarget() {
		float shortestDistance = Mathf.Infinity;
		Transform closestEnemy = null;
		List<Transform> enemysInRange = new List<Transform> ();

		foreach (Transform enemy in enemyContainer.transform) {

			if (Vector3.Distance (enemy.position, transform.position) < range) {
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
		Debug.Log ("peng");
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range );
	}
}
