using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float aoeRange = 0.2f;

	public float velocity = 8f; 
	[HideInInspector] 
	public float damage;
	public GameObject aoeEffectPrefab;

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
		// Calculates Targets for AoE Damage and applies Damage
		// TODO: bullet can possibly collide with another bullet? Bother are triggers
		// TODO: collides with the first mob it hits, not neccecary the actual target
		if (aoeRange != 0) 
		{
			StartCoroutine( BlastEffect (aoeEffectPrefab, transform.position, aoeRange, 0.2f));
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
			// TODO: can cause error when the target is already dead
			target.GetComponent<MobController> ().TakeDamage (damage);
		}
			
		target.GetComponent<MobController> ().mobData.incomingDmg -= damage;
		Destroy (gameObject);
	}

	private IEnumerator BlastEffect(GameObject prefab, Vector3 pos, float radius, float duration) {
		GameObject beGO = Instantiate (prefab, pos, Quaternion.identity);
		beGO.transform.localScale = new Vector3 (ScaleBlastradiusVisual(radius), ScaleBlastradiusVisual(radius), 1);

		//SpriteRenderer beSR = beGO.GetComponent<SpriteRenderer> ();
		//beSR.color = new Color(beSR.color.r, beSR.color.g, beSR.color.b, 0.8f);

		// TODO: FRIED WHY DOSNT THIS WORK??B
//		while (beSR.color.a > 0.0f)
//		{
//			Debug.Log (beSR.color.a);
//			beSR.color = new Color(beSR.color.r, beSR.color.g, beSR.color.b, beSR.color.a - (Time.deltaTime / duration));
//		}
//		Debug.Log ("while finished");

		//be.transform.localScale = Vector3.one * actualRange(radius);
		Destroy (beGO, duration);
		yield return null;
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