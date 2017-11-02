using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

	private Rigidbody2D mobRb2D;
	private Transform finish;
	private GameObject waypointContainer;
	private Queue<Vector3> waypoints;
	private Vector3 nextWaypoint;

	public float moveSpeed = 100f;

	// Use this for initialization
	void Start () {
		waypointContainer = GameObject.FindWithTag("WaypointContainer");
		mobRb2D = GetComponent<Rigidbody2D> ();
		finish = GameObject.FindWithTag ("Finish").transform;

		waypoints = new Queue<Vector3> ();
		waypoints.Clear ();

		foreach (Transform t in waypointContainer.GetComponentInChildren<Transform>()) {
			Debug.Log (t);
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

	void OnTriggerEnter2D ( Collider2D other) {
		if (other.tag == "Finish") {
			Destroy (gameObject);
		}
		if (other.tag == "Waypoint") {
			nextWaypoint = waypoints.Dequeue ();
		}

	}

}
