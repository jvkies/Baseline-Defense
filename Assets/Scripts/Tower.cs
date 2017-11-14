using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower {

	public string towerID;					// tower id, must be unique
	public string towerName;				// name of the tower
	public string upgradeID;				// id of the tower, this is going to upgrade to, e.g. 'bullettower2' 
	public float towerCost;					// price of the Tower to build
	public float upgradeCost;				// price to upgrade the tower
	public int towerDamage;					// damage a shot from this tower does
	public int towerLevel;					// level of the tower, usually starts at 1
	public float towerFireRate;				// frequency the tower is shooting
	public float towerRange;				// range a tower locks onto targets
	public float aoeRange;					// AoE range the tower
	public float bulletVelocity;			// velocity of projectile
	public Vector2 bulletSize;				// size of projectile
	public Sprite towerImageHead;			// image of the Towerhead

	public Tower(string _towerID, string _towerName, string _upgradeID, float _towerCost, float _upgradeCost, int _towerDamage, int _towerLevel, float _towerFireRate, float _towerRange, float _aoeRange, float _bulletVelocity, Vector2 _bulletSize, Sprite _towerImageHead) {
		towerID = _towerID;
		towerName = _towerName;
		upgradeID = _upgradeID;
		towerCost = _towerCost;
		upgradeCost = _upgradeCost;
		towerDamage = _towerDamage;
		towerLevel = _towerLevel;
		towerFireRate = _towerFireRate;
		towerRange = _towerRange;
		aoeRange = _aoeRange;
		bulletVelocity = _bulletVelocity;
		bulletSize = _bulletSize;
		towerImageHead = _towerImageHead;
	}

	public Tower Copy() {
		Tower copy = new Tower (towerID, towerName, upgradeID, towerCost, upgradeCost, towerDamage, towerLevel, towerFireRate, towerRange, aoeRange, bulletVelocity, bulletSize, towerImageHead);
		return copy;
	}
}
