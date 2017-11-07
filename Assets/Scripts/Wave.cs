using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave {

	public int waveID;					// wave id, must be unique
	public string mobID;
	public int mobAmount;

	public Wave(int _waveID, string _mobID, int _mobAmount) {
		waveID = _waveID;
		mobID = _mobID;
		mobAmount = _mobAmount;
	}

}
