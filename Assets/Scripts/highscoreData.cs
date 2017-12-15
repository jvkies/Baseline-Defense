using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class highscoreData {

	public string id_highscores;
	public string playername;
	public string wave;
	public string time;
	public string mobs;
	public string version;
	public string timestamp;

	public highscoreData( string _playername, string _wave, string _time, string _mobs, string _version, string _timestamp) {
		playername = _playername;
		wave = _wave;
		time = _time;
		mobs = _mobs;
		version = _version;
		timestamp = _timestamp;
	}
}
