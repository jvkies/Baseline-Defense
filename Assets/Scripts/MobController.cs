using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobController : MonoBehaviour {

	private Rigidbody2D mobRb2D;
	private GameObject waypointContainer;
	private Queue<Vector3> waypoints;
	private Vector3 nextWaypoint;

	public Mob mobData;
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

		offset = offset.normalized * (mobData.moveSpeed) * Time.deltaTime;

		if (offset.magnitude > .1f) {
			mobRb2D.velocity = new Vector3 (offset.x, offset.y);
		}
	}

	public void TakeDamage(float amount) {
		mobData.health -= amount;
		HealthBar.fillAmount = mobData.health / mobData.maxHealth ;

		if (mobData.health <= 0) {
			GameManager.instance.money += mobData.moneyWorth;
			GameManager.instance.UpdateMoney ();
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D ( Collider2D other) {
		if (other.tag == "Finish") {
			GameManager.instance.DecreaseLife (mobData.mobHeartDamage);
			Destroy (gameObject);
		}
		if (other.tag == "Waypoint") {
			nextWaypoint = waypoints.Dequeue ();
		}

	}

}
