using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobController : MonoBehaviour {

	private bool isDefeated = false;
	private Rigidbody2D mobRb2D;
	private Transform towerspotContainer;
	private GameObject healthBarInstance;
	private GameObject healthbarContainer;
	private Spawner spawnScript;

	public Mob mobData;
	public GameObject healthBarPrefab;
	public GameObject soul;


	// Use this for initialization
	void Start () {
		healthbarContainer = GameObject.FindWithTag("HealthbarContainer");
		spawnScript = GameObject.FindWithTag("Spawnblock").GetComponent<Spawner>();

		healthBarInstance = Instantiate (healthBarPrefab, gameObject.transform.position, Quaternion.identity);
		healthBarInstance.transform.SetParent (healthbarContainer.transform, false);
		healthBarInstance.GetComponent<RectTransform>().localScale = new Vector3(0.3f,0.5f,1);

		mobRb2D = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LateUpdate () {
		healthBarInstance.GetComponent<RectTransform>().position = gameObject.transform.position + new Vector3(0,0.3f,0);
	}

	public void MoveTowardsTarget(Vector3 target) {
		var offset = target - transform.position;

		offset = offset.normalized * (mobData.moveSpeed) * Time.deltaTime;

		if (offset.magnitude > .1f) {
			mobRb2D.velocity = new Vector3 (offset.x, offset.y);
		}
	}

	public void TakeDamage(float amount) {

		if ((amount - mobData.armor) < 0) {
			amount = 0;
		} else {
			amount = amount - mobData.armor;
		}
		
		mobData.health -= amount;

		healthBarInstance.GetComponentsInChildren<Image>()[1].fillAmount = mobData.health / mobData.maxHealth ;

		if (mobData.health <= 0 && !isDefeated) {
			// the mob has been killed

			isDefeated = true;		// the TakeDamage function may be called while the mob already has < 0 hp

			GameManager.instance.UpdateSouls (mobData.moneyWorth);
			if (!GameManager.instance.isGameLost)
				GameManager.instance.highscore ["mobs"] += 1;

			//GameObject soulGO = Instantiate (soul, gameObject.transform.position, Quaternion.identity);
			//soulGO.transform.SetParent (yellowCrystal.transform);

			// remove this mob from the waveMob Dict
			spawnScript.waveMob [mobData.waveID].Remove(gameObject);

			// if the last mob was removed, remove the wave from waveMob Dict
			if (spawnScript.waveMob [mobData.waveID].Count == 0) {
				Debug.Log("wave "+mobData.waveID+" clear");
				GameManager.instance.highscore["wave"] = mobData.waveID;
				spawnScript.waveMob.Remove (mobData.waveID);
			}

			Destroy (healthBarInstance);
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D ( Collider2D other) {
		if (other.tag == "Finish") {
			GameManager.instance.UpdateSouls (-mobData.mobHeartDamage);
			Destroy (healthBarInstance);
			Destroy (gameObject);
		}
	//	if (other.tag == "Waypoint") {
	//		nextWaypoint = waypoints.Dequeue ();
	//	}

	}

}
