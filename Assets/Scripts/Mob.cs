using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob {

	public string mobID;					// mob id, must be unique
	public string mobName;					// name of the mob
	public int waveID;						// id of the wave the mob is spawned in
	public int moneyWorth;
	public float moveSpeed;					// TODO: use this by Astar Pathfinding
	public float maxHealth;
	public float health;
	public int armor;						// physical armor the mob has
	public float incomingDmg;
	public int mobHeartDamage;				// damage the mob does to the players heart
	public GameObject mobPrefab;

	public Mob(string _mobID, string _mobName, int _waveID, int _moneyWorth, float _moveSpeed, float _maxHealth, float _health, int _armor, float _incomingDmg, int _mobHeartDamage, GameObject _mobPrefab) {
		mobID = _mobID;
		mobName = _mobName;
		waveID = _waveID;
		moneyWorth = _moneyWorth;
		moveSpeed = _moveSpeed;
		maxHealth = _maxHealth;
		health = _health;
		armor = _armor;
		incomingDmg = _incomingDmg;
		mobHeartDamage = _mobHeartDamage;
		mobPrefab = _mobPrefab;
	}

	public Mob Copy() {
		Mob copy = new Mob (mobID, mobName, waveID, moneyWorth, moveSpeed, maxHealth, health, armor, incomingDmg, mobHeartDamage, mobPrefab);
		return copy;
	}
}
