using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

	private Rigidbody2D mobRb2D;
	private Transform finish;

	public float moveSpeed = 100f;

	// Use this for initialization
	void Start () {
		mobRb2D = GetComponent<Rigidbody2D> ();
		finish = GameObject.FindWithTag ("Finish").transform;


	}
	
	// Update is called once per frame
	void Update () {
		MoveTowardsTarget ();
	}

	public void MoveTowardsTarget() {
		var offset = finish.position - transform.position;

		offset = offset.normalized * (moveSpeed) * Time.deltaTime;

		if (offset.magnitude > .1f) {
			mobRb2D.velocity = new Vector2 (offset.x, offset.y);
		}
	}

	void OnTriggerEnter2D ( Collider2D other) {
		if (other.tag == "Finish") {
			Destroy (gameObject);
		}
	}

}
