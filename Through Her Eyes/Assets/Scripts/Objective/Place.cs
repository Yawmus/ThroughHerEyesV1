using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Place : Objective {
	
	public GameObject zone;
	public List<GameObject> items = new List<GameObject>();


	void Awake() {
		if (!zone.GetComponent<BoxCollider> ().isTrigger)
			Debug.Log ("Zone needs to be a trigger!");
	}
	
	// Update is called once per frame
	void Update () {
		bool done = true;
		for (int i=0; i<items.Count; i++) {
			if (!zone.GetComponent<BoxCollider> ().bounds.Intersects (items [i].GetComponent<Collider>().bounds))
				done = false;
		}
		
		completed = done;
	}
}
