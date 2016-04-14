using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interact : Objective {
	
	public List<Interaction> interactions;

	void Awake() {
	}
	
	// Update is called once per frame
	void Update () {
		bool finished = true;
		foreach (Interaction i in interactions) {
			if (i.gameObject.GetComponent<Interactable>().currentState != i.state){
				finished = false;
			}
			else{
				// Has it been interacted with since it became an objective?
				if(i.freshInteraction && (!i.gameObject.GetComponent<Interactable>().IsInteracted()))
					finished = false;
			}
		}
		
		completed = finished;
	}
	
	[System.Serializable]
	public class Interaction{
		public GameObject gameObject;
		public int state;
		public bool freshInteraction;
	}
}
