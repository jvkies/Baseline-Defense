using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;
	private Transform enemyContainer;

	public float aoeRange = 0.2f;

	public float velocity = 8f; 
	[HideInInspector] 
	public float damage;

	void Start() {
		enemyContainer = GameObject.FindWithTag ("EnemyContainer").transform;

	}

	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = velocity * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame) {
			if (aoeRange != 0) {
				for (int i = 0; i < enemyContainer.childCount; i++) {
					if ((enemyContainer.GetChild (i).transform.position - transform.position).magnitude <= aoeRange)
						enemyContainer.GetChild (i).GetComponent<MobController> ().TakeDamage (damage);
				}
			}
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
