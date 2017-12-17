using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;
	private Transform lastTarget;

	public float aoeRange = 0.2f;

	public float velocity = 8f; 
	[HideInInspector] 
	public float damage;
	public GameObject aoeEffectPrefab;

	void Start() {
	    lastTarget = target;
	}

	// Update is called once per frame
	void Update () {
		if (target == null)  {
		    if(aoeRange > 0)
		    {
				explodeAoeBullet ();
		    }
            Destroy (gameObject);
            return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = velocity * Time.deltaTime;

		transform.Translate (dir.normalized * distanceThisFrame, Space.World);
	}
		 
	void OnTriggerEnter2D(Collider2D bulletCollision) {
		if (target != null) {
			if (bulletCollision == target.gameObject.GetComponent<Collider2D> ()) {
				target.GetComponent<MobController> ().mobData.incomingDmg -= damage;
				if (aoeRange > 0) { 
					explodeAoeBullet ();
				} else {
					target.GetComponent<MobController> ().TakeDamage (damage);
				}
				Destroy (gameObject);
			}
		} else {
			Destroy (gameObject);
		}
	}

	private void explodeAoeBullet()
	{
		//StartCoroutine( BlastEffect (aoeEffectPrefab, transform.position, aoeRange, 0.2f));
		GameObject beGO = Instantiate (aoeEffectPrefab, transform.position, Quaternion.identity);
		beGO.transform.localScale = new Vector3 (ScaleBlastradiusVisual (aoeRange), ScaleBlastradiusVisual (aoeRange), 1);
		beGO.GetComponent<SpriteRenderer> ().color = GetComponent<SpriteRenderer> ().color;

		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, aoeRange);

		for (int i = 0; i < colliders.Length; i++) {
			MobController enemyC = colliders [i].GetComponent<MobController> ();
			if (enemyC != null) {
				enemyC.TakeDamage (damage);
			}
		}
	}
				
	public void Seek(Transform _target) {
		target = _target;
		target.GetComponent<MobController> ().mobData.incomingDmg += damage;
	}

	public float ScaleBlastradiusVisual(float range) {	
		// this is not anyhow calculated, it is a mere visual approximation
		return range/3f;
	}
}