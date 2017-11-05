using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower {

	public int towerID;					// tower id, must be unique
	public string towerName;			// name of the tower
	public int towerCost;			// price of the Tower to build
	public int towerDamage;			// damage a shot from this tower does
	public int towerLevel;			// level of the tower, usually starts at 1
	public int towerShootspeed;		// frequency the tower is shooting
	public int towerRange;			// range a tower locks onto targets

	public Tower(int _towerID, string _towerName, int _towerCost, int _towerDamage, int _towerLevel, int _towerShootspeed, int _towerRange) {
		towerID = _towerID;
		towerName = _towerName;
		towerCost = _towerCost;
		towerDamage = _towerDamage;
		towerLevel = _towerLevel;
		towerShootspeed = _towerShootspeed;
		towerRange = _towerRange;
	}
}
