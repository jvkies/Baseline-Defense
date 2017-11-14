using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//SceneManager.sceneUnloaded += OnSceneUnload;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// TODO: BUG: walls should automatically be destroyed on scene change, but they somehow dont!?
	//void OnSceneUnload<Scene> (Scene scene) {
	//	Debug.Log ("scene unloading");
	//	Destroy (gameObject);
	//}

}
