using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRotator : MonoBehaviour {

	public float range=2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 0, 35) * Time.deltaTime);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range );
	}

}
