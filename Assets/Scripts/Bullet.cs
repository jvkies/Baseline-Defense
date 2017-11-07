using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float bulletVelocity = 8f; 
	[HideInInspector] 
	public float damage;

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
		target.GetComponent<MobController> ().TakeDamage (damage);
		target.GetComponent<MobController> ().mobData.incomingDmg -= damage;
		Destroy (gameObject);
	}

	public void Seek(Transform _target) {
		target = _target;
		target.GetComponent<MobController> ().mobData.incomingDmg += damage;
	}
}
