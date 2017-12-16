using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float aoeRange = 0.2f;

	public float velocity = 8f; 
	[HideInInspector] 
	public float damage;

	void Start() {
	}

	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = velocity * Time.deltaTime;

		transform.Translate (dir.normalized * distanceThisFrame, Space.World);
	}
		 
	void OnTriggerEnter2D(Collider2D collider2d) {
		// TODO: Jona baut hier den BlastEffect ein
//		GameObject be = Instantiate (blastEffect,transform.position, Quaternion.identity);
//		be.transform.localScale = Vector3.one * BlastRadius;
//		Destroy (be, 0.2f);

		// Calculates Targets for AoE Damage and applies Damage
		if (aoeRange != 0) 
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, aoeRange);
			Debug.Log ("enemys hit: " + colliders.Length);
			for (int i = 0; i < colliders.Length; i++) 
			{
				MobController enemyC = colliders [i].GetComponent<MobController> ();
				if (enemyC != null) 
				{
					enemyC.TakeDamage (damage);
				}
			}
		} 
		// Applies Damage to Non AoE Target
		else 
		{
			target.GetComponent<MobController> ().TakeDamage (damage);
		}
			
		target.GetComponent<MobController> ().mobData.incomingDmg -= damage;
		Destroy (gameObject);
	}
		
	public void Seek(Transform _target) {
		target = _target;
		target.GetComponent<MobController> ().mobData.incomingDmg += damage;
	}
}