using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour {

	private bool isDespawning = false;

	private Rigidbody2D rb;
	public float despawnTime = 1f;
	public float thrust = 1f;
	public GameObject yellowCrystal;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		yellowCrystal = GameObject.FindWithTag ("YellowCrystal");
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDespawning) {
			FloatTowardsHome ();
		}
	}

	public void FloatTowardsHome() {
		Vector2 dir = yellowCrystal.transform.position - transform.position;
		rb.AddRelativeForce(dir);
		//	rb.AddForce((dir + (DegreeToVector2(-30))) * 0.5f );

		if (dir.magnitude < 0.15f) {
			rb.AddRelativeForce(-dir * thrust);
		}
	}

	public static Vector2 DegreeToVector2(float degree)
	{
		return RadianToVector2(degree * Mathf.Deg2Rad);
	}

	public static Vector2 RadianToVector2(float radian)
	{
		return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	public void Despawn(GameObject position = null) {
		
		Vector2 forceDir = Vector2.up;

		if (position != null) {
			forceDir = position.transform.position - yellowCrystal.transform.position;
		}

		isDespawning = true;
		rb.AddForce(forceDir,ForceMode2D.Impulse);
		StartCoroutine(Util.FadeSpriteRendererToZeroAlpha (despawnTime, GetComponent<SpriteRenderer> ()));

		Destroy (gameObject, despawnTime);
	}


}
