using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	private GameObject enemyContainer;
	public GameObject enemy;

	// Use this for initialization
	void Start () {
		enemyContainer = GameObject.FindWithTag("EnemyContainer");

		InvokeRepeating ("SpawnMob", 1,20);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SpawnMob() {

		GameObject mob = Instantiate (enemy, transform.position, Quaternion.identity);
		if (enemyContainer != null) {
			mob.transform.SetParent (enemyContainer.transform);
		}

	}
}
