using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobController : MonoBehaviour {

	private Rigidbody2D mobRb2D;
//	private GameObject waypointContainer;
	private Transform towerspotContainer;
	//private Queue<Vector3> waypoints;
//	private Vector3 nextWaypoint;
	private GameObject healthBarInstance;

	public Mob mobData;
//	public Image HealthBar;
	public GameObject healthBarPrefab;
	private GameObject healthbarContainer;

	// Use this for initialization
	void Start () {
		healthbarContainer = GameObject.FindWithTag("HealthbarContainer");

		healthBarInstance = Instantiate (healthBarPrefab, gameObject.transform.position, Quaternion.identity);
		healthBarInstance.transform.SetParent (healthbarContainer.transform, false);
		healthBarInstance.GetComponent<RectTransform>().localScale = new Vector3(0.3f,0.5f,1);

//		mobData = GameManager.instance.mobs ["blob1"].Copy ();

	//	waypointContainer = GameObject.FindWithTag("WaypointContainer");
		mobRb2D = GetComponent<Rigidbody2D> ();

	//	waypoints = new Queue<Vector3> ();
	//	waypoints.Clear ();

	//	foreach (Transform t in waypointContainer.GetComponentInChildren<Transform>()) {
	//		waypoints.Enqueue (t.position);
	//	}

	//	nextWaypoint = waypoints.Dequeue ();
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
		mobData.health -= amount;
		healthBarInstance.GetComponentsInChildren<Image>()[1].fillAmount = mobData.health / mobData.maxHealth ;

		if (mobData.health <= 0) {
			//GameManager.instance.money += mobData.moneyWorth;
			GameManager.instance.UpdateSouls (mobData.moneyWorth);
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
