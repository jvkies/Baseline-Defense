﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mob : MonoBehaviour {

	private Rigidbody2D mobRb2D;
	private GameObject waypointContainer;
	private Queue<Vector3> waypoints;
	private Vector3 nextWaypoint;

	public float moveSpeed = 10f;
	public float maxHealth = 10f;
	public float health = 10f;
	public int moneyWorth = 1;
	public float incomingDmg = 0;
	public Image HealthBar;

	// Use this for initialization
	void Start () {
		waypointContainer = GameObject.FindWithTag("WaypointContainer");
		mobRb2D = GetComponent<Rigidbody2D> ();

		waypoints = new Queue<Vector3> ();
		waypoints.Clear ();

		foreach (Transform t in waypointContainer.GetComponentInChildren<Transform>()) {
			waypoints.Enqueue (t.position);
		}

		nextWaypoint = waypoints.Dequeue ();
	}
	
	// Update is called once per frame
	void Update () {
		MoveTowardsTarget (nextWaypoint);
	}

	public void MoveTowardsTarget(Vector3 target) {
		var offset = target - transform.position;

		offset = offset.normalized * (moveSpeed) * Time.deltaTime;

		if (offset.magnitude > .1f) {
			mobRb2D.velocity = new Vector3 (offset.x, offset.y);
		}
	}

	public void TakeDamage(float amount) {
		health -= amount;
		HealthBar.fillAmount = health / maxHealth ;
		if (health <= 0) {
			GameManager.instance.money += moneyWorth;
			GameManager.instance.UpdateMoney ();
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D ( Collider2D other) {
		if (other.tag == "Finish") {
			GameManager.instance.DecreaseLife (1);
			Destroy (gameObject);
		}
		if (other.tag == "Waypoint") {
			nextWaypoint = waypoints.Dequeue ();
		}

	}

}
