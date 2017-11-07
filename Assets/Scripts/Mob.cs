using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob {

	public string mobID;					// mob id, must be unique
	public string mobName;					// name of the mob
	public int moneyWorth;
	public float moveSpeed;
	public float maxHealth;
	public float health;
	public float incomingDmg;
	public int mobHeartDamage;			// damage the mob does to the players heart
	public GameObject mobPrefab;

	public Mob(string _mobID, string _mobName, int _moneyWorth, float _moveSpeed, float _maxHealth, float _health, float _incomingDmg, int _mobHeartDamage, GameObject _mobPrefab) {
		mobID = _mobID;
		mobName = _mobName;
		moneyWorth = _moneyWorth;
		moveSpeed = _moveSpeed;
		maxHealth = _maxHealth;
		health = _health;
		incomingDmg = _incomingDmg;
		mobHeartDamage = _mobHeartDamage;
		mobPrefab = _mobPrefab;
	}

	public Mob Copy() {
		Mob copy = new Mob (mobID, mobName, moneyWorth, moveSpeed, maxHealth, health, incomingDmg, mobHeartDamage, mobPrefab);
		return copy;
	}
}
