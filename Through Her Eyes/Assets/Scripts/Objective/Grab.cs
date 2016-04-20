using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grab : Objective {

	public string item;
	
	
	void Awake() {
	}
	
	// Update is called once per frame
	void Update () {
		GameObject g = GameObject.Find ("Player").GetComponent<Player> ().HeldObject ();
		if (g != null && g.name == item)
			completed = true;
	}
	/*	public override void SetActive(bool active){
		this.active = active;
	}*/
}
