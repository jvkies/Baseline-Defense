using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;
	public float bulletVelocity = 8f;
	public float damage = 4f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = bulletVelocity * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame) {
			HitTarget ();
			return;
		}

		transform.Translate (dir.normalized * distanceThisFrame, Space.World);
	}

	private void HitTarget() {
		target.GetComponent<Mob> ().TakeDamage (damage);
		Destroy (gameObject);
	}

	public void Seek(Transform _target) {
		target = _target;
	}
}
